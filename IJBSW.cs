// <copyright file="IJBSW.cs" company="Jack Badger Ltd">
// Copyright (c) Jack Badger Ltd. All rights reserved.
// </copyright>

namespace JBLib
{
    using System.Runtime.InteropServices;
    using SldWorks;
    using SwConst;

    [ComVisible(true)]
    [Guid("34344400-AD16-4AF8-9988-6B684EBF7643")]
    [TypeLibImportClass(typeof(JBSW))]
    public interface IJBSW
    {
        string Name { get; set; }

        void Init(ISldWorks app);

        [return: MarshalAs(UnmanagedType.I4)]
        swMessageBoxResult_e MessageBox(
            string message,
            [MarshalAs(UnmanagedType.I4)] swMessageBoxIcon_e icon,
            [MarshalAs(UnmanagedType.I4)] swMessageBoxBtn_e btn);

        object ReturnStuff();
    }
}