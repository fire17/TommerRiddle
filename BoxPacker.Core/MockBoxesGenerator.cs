using System;
using System.Collections.Generic;

namespace BoxPacker.Core
{
    public static class MockBoxesGenerator
    {
        public static List<Box> Generate()
        {
            var rectangles = new List<Box>();

            var random = new Random();
            int lastId = 0;
            rectangles.AddRange(GetEqualSizeBoxes(RandomGenerator.GetRandomBetween0And1(random) * 20, 100, 100, random,
                ref lastId));
            rectangles.AddRange(GetEqualSizeBoxes(RandomGenerator.GetRandomBetween0And1(random) * 10, 60, 60, random,
                ref lastId));
            rectangles.AddRange(GetEqualSizeBoxes(RandomGenerator.GetRandomBetween0And1(random) * 40, 50, 20, random,
                ref lastId));
            rectangles.AddRange(GetEqualSizeBoxes(50 + RandomGenerator.GetRandomBetween0And1(random) * 50, 20, 50,
                random, ref lastId));
            rectangles.AddRange(GetEqualSizeBoxes(RandomGenerator.GetRandomBetween0And1(random) * 20, 100, 100, random,
                ref lastId));

            if (RandomGenerator.GetRandomBetween0And1(random) > 0.5)
            {
                rectangles.Add(BuildBox(250, 250, random, ref lastId));
            }

            if (RandomGenerator.GetRandomBetween0And1(random) > 0.5)
            {
                rectangles.Add(BuildBox(250, 100, random, ref lastId));
            }

            if (RandomGenerator.GetRandomBetween0And1(random) > 0.5)
            {
                rectangles.Add(BuildBox(100, 250, random, ref lastId));
            }

            if (RandomGenerator.GetRandomBetween0And1(random) > 0.5)
            {
                rectangles.Add(BuildBox(400, 80, random, ref lastId));
            }

            if (RandomGenerator.GetRandomBetween0And1(random) > 0.5)
            {
                rectangles.Add(BuildBox(80, 400, random, ref lastId));
            }

            rectangles.AddRange(GetEqualSizeBoxes(300 + RandomGenerator.GetRandomBetween0And1(random) * 200, 10, 10,
                random, ref lastId));
            rectangles.AddRange(GetEqualSizeBoxes(500 + RandomGenerator.GetRandomBetween0And1(random) * 500, 5, 5,
                random, ref lastId));

            return rectangles;
        }

        private static List<Box> GetEqualSizeBoxes(int amount, int width, int height, Random random, ref int lastId)
        {
            var rectangles = new List<Box>();

            for (var i = amount; i >= 0; i--)
            {
                rectangles.Add(BuildBox(width, height, random, ref lastId));
            }

            return rectangles;
        }

        private static Box BuildBox(int width, int height, Random random, ref int lastId)
        {
            lastId++;
            return new Box
            {
                Width = width,
                Height = height,
                ColorHex = RandomGenerator.GetRandomColorHex(random),
                Id = lastId
            };
        }
    }
}