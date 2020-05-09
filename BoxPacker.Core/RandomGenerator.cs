using System;
using System.Collections.Generic;
using System.Text;

namespace BoxPacker.Core
{
    public static class RandomGenerator
    {
        public static int GetRandomBetween0And1(Random random)
        {
            return random.Next(0, 10) / 10;
        }

        public static string GetRandomColorHex(Random random)
        {
            return $"#{random.Next(0x1000000):X6}";
        }
    }
}
