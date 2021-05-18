// <copyright file="Equation.cs" company="Jack Badger Ltd">
// Copyright (c) Jack Badger Ltd. All rights reserved.
// </copyright>

namespace JBLib.Equations
{
    using System;
    using System.Text.Json.Serialization;

    [JsonConverter(typeof(EquationJsonConverter))]
    public struct Equation : IEquatable<Equation>
    {
        public string ParameterName { get; private set; }

        public string Expression { get; private set; }

        public Equation(string equation)
        {
            int i = equation.IndexOf('=');

            if (i < 0)
            {
                throw new ArgumentException($"Invalid Syntax. {equation}", nameof(equation));
            }

            if (i == 0)
            {
                throw new ArgumentException($"Empty Name. {equation}", nameof(equation));
            }

            if (i == equation.Length - 1)
            {
                throw new ArgumentException($"Empty Expression {equation}", nameof(equation));
            }

            ParameterName = equation.Substring(0, i).Trim();
            Expression = equation.Substring(i + 1).Trim();
        }

        /// <summary>
        /// Only checks the <see cref="ParameterName"/>.
        /// </summary>
        public bool Equals(Equation other) => ParameterName.Equals(other.ParameterName, StringComparison.OrdinalIgnoreCase);

        public override string ToString() => $"{ParameterName} = {Expression}";

        public override bool Equals(object obj) => obj is Equation equation && Equals(equation);

        public override int GetHashCode() => ParameterName.GetHashCode();
    }
}
