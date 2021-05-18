// <copyright file="EquationSolver.cs" company="Jack Badger Ltd">
// Copyright (c) Jack Badger Ltd. All rights reserved.
// </copyright>

namespace JBLib.Equations
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using JBLib.Evaluate;

    [ComVisible(true)]
    [ProgId("JBLib.Equations")]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("BABB2BBA-C5CB-45FD-B4C8-BD000EFAB404")]
    public class EquationSolver : IEquationSolver
    {
        private readonly IDictionary<string, double> globalSymbols;
        private readonly IDictionary<string, double> localSymbols;

        private Eval eval;

        public EquationSolver()
        {
            eval = new Eval();
            eval.ProcessFunction += Eval_ProcessFunction;
            eval.ProcessSymbol += Eval_ProcessSymbol;

            globalSymbols = new Dictionary<string, double>()
            {
                { "pi", Math.PI },
                { "rob", 369 },
                { "D2@Sketch_1", 1 },
            };

            localSymbols = new Dictionary<string, double>();
        }

        public bool AddLocalSymbol(string name, double value)
        {
            if (globalSymbols.ContainsKey(name) || localSymbols.ContainsKey(name))
            {
                return false;
            }

            localSymbols.Add(name, value);

            return true;
        }

        public void ClearLocalSymbols()
        {
            localSymbols.Clear();
        }

        /// <inheritdoc/>
        public double Eval(string equation) => eval.Execute(equation);

        private void Eval_ProcessSymbol(object sender, SymbolEventArgs e)
        {
            if (globalSymbols.ContainsKey(e.Name))
            {
                e.Result = globalSymbols[e.Name];
            }
            else if (localSymbols.ContainsKey(e.Name))
            {
                e.Result = localSymbols[e.Name];
            }
            else
            {
                e.Status = SymbolStatus.UndefinedSymbol;
            }
        }

        private void Eval_ProcessFunction(object sender, FunctionEventArgs e)
            => (e.Result, e.Status) = MathDelegates.Execute(e.Name, e.Parameters);
    }
}
