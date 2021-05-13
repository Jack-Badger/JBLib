// <copyright file="EvalException.cs" company="Jack Badger Ltd">
// Copyright (c) Jack Badger Ltd. All rights reserved.
// </copyright>

namespace JBLib.Evaluate
{
    using System;

    public class EvalException : Exception
    {
        public EvalException(string message, int column)
            : base(message)
        {
            Column = column;
        }

        public int Column { get; set; }
    }
}
