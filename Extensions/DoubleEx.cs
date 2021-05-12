using System;

namespace JBLib.Extensions
{
    public static class DoubleEx
    {
        public static double ToRadians(this double value) => value * Math.PI / 180;

        public static double ToDegrees(this double value) => value * 180 / Math.PI;
    }
}