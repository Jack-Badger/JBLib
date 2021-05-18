// <copyright file="IAttachedDocuments.cs" company="Jack Badger Ltd">
// Copyright (c) Jack Badger Ltd. All rights reserved.
// </copyright>

namespace JBLib.Collections
{
    using System.Runtime.InteropServices;
    using SldWorks;

    [ComVisible(true)]
    [Guid("97717CFA-B0E8-4C46-863F-287099ACEF88")]
    [TypeLibImportClass(typeof(AttachedDocuments))]
    public interface IAttachedDocuments
    {
        void ClearSelections();

        int SelectAttachments(IModelDoc2 model);

        int SelectedCount(string fileExtension);

        int SelectedDocuments(string fileExtension, out string[] filePaths, out bool[] isLinked);

        string[] SelectedFileExtensions();

        int SelectedTotalCount();

        void SelectionCriteria(string fileExtensions, [MarshalAs(UnmanagedType.I4)] AttachedDocumentOption option);
    }
}