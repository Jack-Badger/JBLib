// <copyright file="IModelUpdater.cs" company="Jack Badger Ltd">
// Copyright (c) Jack Badger Ltd. All rights reserved.
// </copyright>

namespace JBLib
{
    using System.Runtime.InteropServices;
    using SldWorks;

    [ComVisible(true)]
    [Guid("97717CFA-B0E8-4C46-863F-287099ACEF88")]
    [TypeLibImportClass(typeof(ModelUpdater))]
    public interface IModelUpdater
    {
        string[] AttachmentExtensions();

        int AttachmentsCount();

        string[] GetAttachments(string extension, out bool[] linked);

        bool Initialize(ISldWorks app);
    }
}