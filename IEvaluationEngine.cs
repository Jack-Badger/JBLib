// <copyright file="IEvaluationEngine.cs" company="Jack Badger Ltd">
// Copyright (c) Jack Badger Ltd. All rights reserved.
// </copyright>

namespace JBLib
{
    using System.Runtime.InteropServices;

    [ComVisible(true)]
    [Guid("3B9DC0F1-51BD-4B98-871E-365965CB1C41")]
    [TypeLibImportClass(typeof(EvaluationEngine))]
    public interface IEvaluationEngine
    {
        string Eval(string equation);
    }
}