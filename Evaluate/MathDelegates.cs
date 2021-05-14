// <copyright file="MathDelegates.cs" company="Jack Badger Ltd">
// Copyright (c) Jack Badger Ltd. All rights reserved.
// </copyright>

namespace JBLib.Evaluate
{
    using JBLib.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class MathDelegates
    {
        private static readonly IDictionary<string, Delegate> Dictionary
            = new Dictionary<string, Delegate>(StringComparer.OrdinalIgnoreCase)
        {
            { "abs", new Func<double, double>(d => Math.Abs(d)) },
            { "acos", new Func<double, double>(d => Math.Acos(d).ToDegrees()) },
            { "asin", new Func<double, double>(d => Math.Asin(d).ToDegrees()) },
            { "atan", new Func<double, double>(d => Math.Atan(d).ToDegrees()) },
            { "atan2", new Func<double, double, double>((y, x) => Math.Atan2(y, x)) },
            { "ceiling", new Func<double, double>(a => Math.Ceiling(a)) },
            { "cos", new Func<double, double>(d => Math.Cos(d.ToRadians())) },
            { "exp", new Func<double, double>(d => Math.Exp(d)) },
            { "floor", new Func<double, double>(d => Math.Floor(d)) },
            { "ln", new Func<double, double>(a => Math.Log(a)) },
            { "log", new Func<double, double, double>((a, b) => Math.Log(a, b)) },
            { "log10", new Func<double, double>(d => Math.Log10(d)) },
            { "max", new Func<double, double, double>((v1, v2) => Math.Max(v1, v2)) },
            { "min", new Func<double, double, double>((v1, v2) => Math.Min(v1, v2)) },
            { "pow", new Func<double, double, double>((x, y) => Math.Pow(x, y)) },
            { "round", new Func<double, double>(a => Math.Round(a)) },
            { "sin", new Func<double, double>(d => Math.Sin(d.ToRadians())) },
            { "sqrt", new Func<double, double>(d => Math.Sqrt(d)) },
            { "tan", new Func<double, double>(d => Math.Tan(d.ToRadians())) },
            { "truncate", new Func<double, double>(d => Math.Truncate(d)) },
        };

        public static (double result, FunctionStatus status) Execute(string name, IEnumerable<double> args)
        {
            if (Dictionary.TryGetValue(name, out Delegate function))
            {
                try
                {
                    return ((double)function.DynamicInvoke(args.Cast<object>().ToArray()), FunctionStatus.OK);
                }
                catch (MemberAccessException)
                {
                    return (double.NaN, FunctionStatus.WrongParameterCount);
                }
            }
            else
            {
                return (double.NaN, FunctionStatus.UndefinedFunction);
            }
        }
    }
}
