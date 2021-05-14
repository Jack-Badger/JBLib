// <copyright file="FeatureEx.cs" company="Jack Badger Ltd">
// Copyright (c) Jack Badger Ltd. All rights reserved.
// </copyright>

namespace JBLib.Extensions
{
    using SldWorks;
    using SwConst;
    using System.Collections.Generic;
    using System.Linq;

    public static class FeatureEx
    {
        public static IEnumerable<IFeature> FilterByName(this IEnumerable<IFeature> features, string wildcard)
            => features.Where(feature => feature.Name.EqualsWildcard(wildcard));

        public static IEnumerable<IFeature> FilterBySelectType(this IEnumerable<IFeature> features, swSelectType_e type)
            => features.Where(feature => (feature as IEntity).GetType() == (int)type);

        public static IEnumerable<IFeature> FilterByTypeName(this IEnumerable<IFeature> features, string type)
            => features.Where(feature => feature.GetTypeName2() == type);

        //// this is daft hey?  what did I add?
        // public static IEnumerable<T> Filter<T>(this IEnumerable<T> ts, Func<T, bool> predicate)
        //    => ts.Where(t => predicate(t));
    }
}
