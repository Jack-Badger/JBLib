using System.Runtime.InteropServices;

namespace JBLib
{
    [ComVisible(true)]
    [Guid("3B9DC0F1-51BD-4B98-871E-365965CB1C41")]
    [TypeLibImportClass(typeof(EvaluationEngine))]
    public interface IEvaluationEngine
    {
        string Eval(string equation);
    }
}