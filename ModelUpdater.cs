// <copyright file="ModelUpdater.cs" company="Jack Badger Ltd">
// Copyright (c) Jack Badger Ltd. All rights reserved.
// </copyright>

namespace JBLib
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using JBLib.Extensions;
    using SldWorks;

    [ComVisible(true)]
    [Guid("AF7F78E3-52E4-4533-9D5F-70FDDAF06A0A")]
    [ClassInterface(ClassInterfaceType.None)]
    public class ModelUpdater : IModelUpdater
    {
        private ISldWorks app;

        protected Attachments Attachments { get; private set; }

        /// <inheritdoc/>
        public bool Initialize(ISldWorks app)
        {
            this.app = app;

            if (app.IActiveDoc2 is IModelDoc2 model)
            {
                var attachments = model.GetAttachmentsEx();
                if (attachments.Any())
                {
                    Attachments = new Attachments(attachments);
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc/>
        public int AttachedDocumentsCount() => Attachments.TotalCount;

        /// <inheritdoc/>
        public string[] AttachedDocumentExtensions() => Attachments.Extensions.ToArray();

        /// <inheritdoc/>
        public string[] AttachedDocuments(string fileExtension, out bool[] isLinkedArray)
        {
            var filePathsList = new List<string>();
            var isLinkedList = new List<bool>();

            foreach (var (f, l) in Attachments[fileExtension])
            {
                filePathsList.Add(f);
                isLinkedList.Add(l);
            }

            isLinkedArray = isLinkedList.ToArray();

            return filePathsList.ToArray();
        }
    }
}
