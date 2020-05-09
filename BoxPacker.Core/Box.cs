using System;
using System.Collections.Generic;
using System.Text;

namespace BoxPacker.Core
{
    public class Box
    {
        public int Id { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public string ColorHex { get; set; }
    }
}
