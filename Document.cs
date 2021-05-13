// <copyright file="Document.cs" company="Jack Badger Ltd">
// Copyright (c) Jack Badger Ltd. All rights reserved.
// </copyright>

namespace JBLib
{
    using System;
    using System.Linq;
    using System.Runtime.InteropServices;
    using JBLib.Extensions;
    using SldWorks;
    using SwConst;

    [ComVisible(true)]
    [Guid("82F99B69-DFB1-438A-8E02-9C02C5D69827")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("JBLib.Document")]
    public class Document : IDocument
    {
        private IModelDoc2 model;

        public Document()
        {
        }

        /// <inheritdoc/>
        public IModelDoc2 Model { get => model; private set => model = value; }

        /// <inheritdoc/>
        public void Init(IModelDoc2 model)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
        }

        /// <inheritdoc/>
        public IFeature[] FilterFeatures([MarshalAs(UnmanagedType.I4)] swSelectType_e type, string name)
            => model.Features().FilterBySelectType(type).FilterByName(name).ToArray();
    }
}
