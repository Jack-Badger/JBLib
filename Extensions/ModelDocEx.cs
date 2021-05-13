using SldWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JBLib.Extensions
{
    public static class ModelDocEx
    {
        public static IEnumerable<IFeature> Features(this IModelDoc2 model)
        {
            if (model?.IFirstFeature() is IFeature feature)
            {
                do
                {
                    yield return feature;

                    feature = feature.IGetNextFeature();
                }
                while (feature != null);
            }
            else
            {
                yield return null;
            }

            yield break;
        }

        public static IEnumerable<(string filename, bool linked)> GetAttachmentsEx(this IModelDoc2 model) => model.Extension.GetAttachmentsEx();

    }
}
