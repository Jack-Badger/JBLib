// <copyright file="EvaluationEngine.cs" company="Jack Badger Ltd">
// Copyright (c) Jack Badger Ltd. All rights reserved.
// </copyright>

namespace JBLib
{
    using JBLib.Evaluate;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    [ComVisible(true)]
    [ProgId("JBLib.Equations")]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("BABB2BBA-C5CB-45FD-B4C8-BD000EFAB404")]
    public class EvaluationEngine : IEvaluationEngine
    {
        private readonly IDictionary<string, double> symbols;

        private Eval eval;

        public EvaluationEngine()
        {
            eval = new Eval();
            eval.ProcessFunction += Eval_ProcessFunction;
            eval.ProcessSymbol += Eval_ProcessSymbol;

            symbols = new Dictionary<string, double>()
            {
                { "pi", Math.PI },
                { "rob", 369 },
            };
        }

        /// <inheritdoc/>
        public string Eval(string equation)
        {
            try
            {
                var result = eval.Execute(equation);
                return $"{result}";
            }
            catch (EvalException evalException)
            {
                return evalException.Message;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        private void Eval_ProcessSymbol(object sender, SymbolEventArgs e)
        {
            if (symbols.ContainsKey(e.Name))
            {
                e.Result = symbols[e.Name];
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
