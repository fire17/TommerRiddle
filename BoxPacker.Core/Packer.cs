using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoxPacker.Core
{
    public class Packer
    {
        public Box Pack(List<Box> boxes)
        {
            // calculate total box area and maximum box width
            var area = 0.0;
            var maxWidth = 0.0;

            foreach (var box in boxes)
            {
                area += box.Width * box.Height;
                maxWidth = Math.Max(maxWidth, box.Width);
            }

            // sort the boxes for insertion by height, descending
            boxes.Sort((rectangle1, rectangle2) => (int)(rectangle2.Height - rectangle1.Height));
            // boxes.sort((a, b) => b.h - a.h);

            // aim for a squarish resulting container,
            // slightly adjusted for sub-100% space utilization
            var startWidth = Math.Max(Math.Ceiling(Math.Sqrt(area / 0.95)), maxWidth);

            // start with a single empty space, unbounded at the bottom
            var spaces = new List<Box>
            {
                new Box
                {
                    Width = startWidth,
                    Height = double.PositiveInfinity
                }
            };

            double width = 0;
            double height = 0;

            foreach (var box in boxes)
            {
                for (var i = spaces.Count - 1; i >= 0; i--)
                {
                    var space = spaces[i];

                    // look for empty spaces that can accommodate the current box
                    if (box.Width > space.Width || box.Height > space.Height) continue;

                    // found the space; add the box to its top-left corner
                    // |-------|-------|
                    // |  box  |       |
                    // |_______|       |
                    // |         space |
                    // |_______________|
                    box.X = space.X;
                    box.Y = space.Y;

                    height = Math.Max(height, box.Y + box.Height);
                    width = Math.Max(width, box.X + box.Width);

                    if (box.Width.Equals(space.Width) && box.Height.Equals(space.Height))
                    {
                        // space matches the box exactly; remove it
                        var last = spaces.Last();
                        spaces.RemoveAt(spaces.Count - 1);
                        if (i < spaces.Count) spaces[i] = last;
                    }
                    else if (box.Height.Equals(space.Height))
                    {
                        // space matches the box height; update it accordingly
                        // |-------|---------------|
                        // |  box  | updated space |
                        // |_______|_______________|
                        space.X += box.Width;
                        space.Width -= box.Width;
                    }
                    else if (box.Width.Equals(space.Width))
                    {
                        // space matches the box width; update it accordingly
                        // |---------------|
                        // |      box      |
                        // |_______________|
                        // | updated space |
                        // |_______________|
                        space.Y += box.Height;
                        space.Height -= box.Height;
                    }
                    else
                    {
                        // otherwise the box splits the space into two spaces
                        // |-------|-----------|
                        // |  box  | new space |
                        // |_______|___________|
                        // | updated space     |
                        // |___________________|
                        spaces.Add(new Box
                        {
                            X = space.X + box.Width,
                            Y = space.Y,
                            Width = space.Width - box.Width,
                            Height = box.Height
                        });

                        space.Y += box.Height;
                        space.Height -= box.Height;
                    }

                    break;
                }
            }

            return new Box
            {
                Width = width,
                Height = height
            };
        }
    }
}
