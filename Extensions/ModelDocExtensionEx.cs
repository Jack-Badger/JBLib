using SldWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JBLib.Extensions
{
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
