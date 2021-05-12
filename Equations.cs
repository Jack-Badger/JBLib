using JBLib.Evaluate;
using SldWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace JBLib
{
    [ComVisible(true)]
    [ProgId("JBLib.Equations")]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("BABB2BBA-C5CB-45FD-B4C8-BD000EFAB404")]
    public class Equations : IEquations
    {
        private ISldWorks app;

        protected Eval eval;

        private IDictionary<string, double> symbols;

        public Equations()
        {
            eval = new Eval();
            eval.ProcessFunction += Eval_ProcessFunction;
            eval.ProcessSymbol += Eval_ProcessSymbol;

            symbols = new Dictionary<string, double>()
            {
                {"pi", Math.PI },
                {"rob", 369 },
            };
        }

        public void Init(ISldWorks app)
        {
            this.app = app ?? throw new ArgumentNullException(nameof(app));
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
    }
}
