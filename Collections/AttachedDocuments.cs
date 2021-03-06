// <copyright file="AttachedDocuments.cs" company="Jack Badger Ltd">
// Copyright (c) Jack Badger Ltd. All rights reserved.
// </copyright>

namespace JBLib.Collections
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using JBLib.Collections.Generic;
    using JBLib.Extensions;
    using SldWorks;

    [ComVisible(true)]
    [Guid("FF5D24B2-47E6-437A-9512-23A08933A1D4")]
    public enum AttachedDocumentOption
    {
        Ignore = FilterOption.Ignore,
        Select = FilterOption.Select,
    }

    [ComVisible(true)]
    [Guid("AF7F78E3-52E4-4533-9D5F-70FDDAF06A0A")]
    [ClassInterface(ClassInterfaceType.None)]
    public class AttachedDocuments : IAttachedDocuments
    {
        private readonly FilteredEnumerable<string, (string filepath, bool isLinked)> docs;

        public AttachedDocuments()
        {
            docs = new FilteredEnumerable<string, (string filepath, bool isLinked)>(keyGenerator: GetFileExtension);
        }

        /// <inheritdoc/>
        public int SelectAttachments(IModelDoc2 model) => docs.AddRange(model?.GetAttachmentsEx());

        public void ClearSelections() => docs.Clear();

        /// <inheritdoc/>
        public void SelectionCriteria(string fileExtensions, AttachedDocumentOption option)
        {
            docs.FilterSet = fileExtensions == string.Empty ? new HashSet<string>()
                : new HashSet<string>(fileExtensions.Split(new char[] { ' ', ',' }));

            docs.Behaviour = (FilterOption)option;
        }

        /// <inheritdoc/>
        public int SelectedTotalCount() => docs.TotalItemCount;

        /// <inheritdoc/>
        public string[] SelectedFileExtensions() => docs.Keys.ToArray();

        /// <inheritdoc/>
        public int SelectedCount(string fileExtension) => docs[fileExtension].Count();

        /// <inheritdoc/>
        public int SelectedDocuments(string fileExtension, out string[] filepaths, out bool[] isLinked)
        {
            var filepathsList = new List<string>();
            var isLinkedList = new List<bool>();

            foreach (var (f, l) in docs[fileExtension])
            {
                filepathsList.Add(f);
                isLinkedList.Add(l);
            }

            filepaths = filepathsList.ToArray();
            isLinked = isLinkedList.ToArray();

            return filepaths.Length;
        }

        private string GetFileExtension((string filepath, bool) item)
        {
            string ext = Path.GetExtension(item.filepath);
            return ext == string.Empty ? "_NO_EXTENSION_" : ext.Substring(1);
        }
    }
}
