// <copyright file="ModelDocExtensionEx.cs" company="Jack Badger Ltd">
// Copyright (c) Jack Badger Ltd. All rights reserved.
// </copyright>

namespace JBLib.Extensions
{
    using SldWorks;
    using System.Collections.Generic;
    using System.Linq;

    public static class ModelDocExtensionEx
    {
        public static IEnumerable<(string filename, bool linked)> GetAttachmentsEx(this IModelDocExtension modelDocExtension)
        {
            if (modelDocExtension.GetAttachmentCount() == 0)
            {
                return Enumerable.Empty<(string, bool)>();
            }

            var filenames = (string[])modelDocExtension.GetAttachments(out object areLinked);

            return filenames.Zip(areLinked as bool[], (filename, isLinked) => (filename, isLinked));
        }
    }
}
