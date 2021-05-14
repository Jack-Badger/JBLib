// <copyright file="IDocument.cs" company="Jack Badger Ltd">
// Copyright (c) Jack Badger Ltd. All rights reserved.
// </copyright>

namespace JBLib
{
    using SldWorks;
    using SwConst;
    using System.Runtime.InteropServices;

    [ComVisible(true)]
    [Guid("A7FC0406-56F0-477D-82C5-B2BE236EEB35")]
    [TypeLibImportClass(typeof(Document))]

    public interface IDocument
    {
        IModelDoc2 Model { get; }

        IFeature[] FilterFeatures([MarshalAs(UnmanagedType.I4)] swSelectType_e type, string name);

        void Init(IModelDoc2 model);
    }
}