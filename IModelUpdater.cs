// <copyright file="IModelUpdater.cs" company="Jack Badger Ltd">
// Copyright (c) Jack Badger Ltd. All rights reserved.
// </copyright>

namespace JBLib
{
    using SldWorks;
    using System.Runtime.InteropServices;

    [ComVisible(true)]
    [Guid("97717CFA-B0E8-4C46-863F-287099ACEF88")]
    [TypeLibImportClass(typeof(ModelUpdater))]
    public interface IModelUpdater
    {
        string[] AttachedDocumentExtensions();

        int AttachedDocumentsCount();

        string[] AttachedDocuments(string fileExtension, out bool[] isLinked);

        bool Initialize(ISldWorks app);
    }
}