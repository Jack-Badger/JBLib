// <copyright file="ModelDocEx.cs" company="Jack Badger Ltd">
// Copyright (c) Jack Badger Ltd. All rights reserved.
// </copyright>

namespace JBLib.Extensions
{
    using SldWorks;
    using System.Collections.Generic;

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

        public static IEnumerable<(string filename, bool linked)>
            GetAttachmentsEx(this IModelDoc2 model) => model.Extension.GetAttachmentsEx();
    }
}
