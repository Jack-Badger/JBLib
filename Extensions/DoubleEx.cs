// <copyright file="DoubleEx.cs" company="Jack Badger Ltd">
// Copyright (c) Jack Badger Ltd. All rights reserved.
// </copyright>

namespace JBLib.Extensions
{
    using System;

    public static class DoubleEx
    {
        public static double ToRadians(this double value) => value * Math.PI / 180;

        public static double ToDegrees(this double value) => value * 180 / Math.PI;
    }
}