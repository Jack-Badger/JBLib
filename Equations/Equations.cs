// <copyright file="Equations.cs" company="Jack Badger Ltd">
// Copyright (c) Jack Badger Ltd. All rights reserved.
// </copyright>

namespace JBLib.Equations
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    [JsonConverter(typeof(EquationsJsonConverter))]
    public class Equations : IEnumerable<Equation>
    {
        private readonly IList<Equation> list = new List<Equation>();

        public int Count => list.Count;

        public Equations()
        {
        }

        public Equations(IEnumerable<Equation> equations)
        {
            foreach (var equation in equations)
            {
                if (!TryAdd(equation))
                {
                    throw new ArgumentException(nameof(equations));
                }
            }
        }

        /// <summary>
        /// Add the equation to the list if it doesn't contain an equation with the same name.
        /// </summary>
        /// <param name="equation">The equation.</param>
        /// <returns>Returns true if the equation was added, false if not.</returns>
        public bool TryAdd(Equation equation)
        {
            if (list.Contains(equation))
            {
                return false;
            }

            list.Add(equation);
            return true;
        }

        public IEnumerator<Equation> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)list).GetEnumerator();
        }
    }
}
