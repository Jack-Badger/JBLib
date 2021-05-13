﻿// <copyright file="Selections.cs" company="Jack Badger Ltd">
// Copyright (c) Jack Badger Ltd. All rights reserved.
// </copyright>

namespace JBLib
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using SldWorks;
    using SwConst;

    /// <summary>
    ///   <br />
    /// </summary>
    [ComVisible(true)]
    [Guid("ACB9D121-9547-4C9F-AD62-295E55D972FC")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("JBLib.Selections")]
    public class Selections : ISelections
    {
        private readonly ISet<swSelectType_e> typesToCollect = new HashSet<swSelectType_e>();

        public Selections()
        {
        }

        /// <inheritdoc/>
        public bool CollectAllTypes { get; set; } = true;

        /// <summary>
        /// Collects the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for Collect
        public void MarkTypeForCollection([MarshalAs(UnmanagedType.I4)] swSelectType_e type)
        {
            if (type == swSelectType_e.swSelNOTHING)
            {
                typesToCollect.Clear();
            }
            else
            {
                typesToCollect.Add(type);
            }
        }

        /// <inheritdoc/>
        public Scripting.Dictionary CreateScriptingDictionary(ISelectionMgr selMgr)
        {
            var scriptDict = new Scripting.Dictionary();

            int count = selMgr.GetSelectedObjectCount2(-1);

            if (count > 0)
            {
                var dict = new Dictionary<swSelectType_e, List<object>>();

                for (int i = 1; i <= count; i++)
                {
                    swSelectType_e key = (swSelectType_e)selMgr.GetSelectedObjectType3(i, -1);

                    if (CollectAllTypes || typesToCollect.Contains(key))
                    {
                        if (!dict.ContainsKey(key))
                        {
                            dict.Add(key, new List<object>());
                        }

                        object @object = selMgr.GetSelectedObject6(i, -1);

                        dict[key].Add(@object);
                    }
                }

                foreach (var key in dict.Keys)
                {
                    scriptDict.Add(key, dict[key].ToArray());
                }
            }

            return scriptDict;
        }
    }
}
