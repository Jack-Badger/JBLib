// <copyright file="ISelections.cs" company="Jack Badger Ltd">
// Copyright (c) Jack Badger Ltd. All rights reserved.
// </copyright>

namespace JBLib
{
    using SldWorks;
    using SwConst;
    using System.Runtime.InteropServices;

    /// <summary>
    ///   <br />
    /// </summary>
    [ComVisible(true)]
    [Guid("6C8F4B36-F364-4252-BCA4-7D024EFDB009")]
    [TypeLibImportClass(typeof(Selections))]
    public interface ISelections
    {
        /// <summary>
        /// Gets or sets a value indicating whether [collect all types].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [collect all types]; otherwise, <c>false</c>.
        /// </value>
        bool CollectAllTypes { get; set; }

        /// <summary>
        /// Collects the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        void MarkTypeForCollection([MarshalAs(UnmanagedType.I4)] swSelectType_e type);

        /// <summary>Populates the specified sel MGR.</summary>
        /// <param name="selMgr">The Selection Manager.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Scripting.Dictionary CreateScriptingDictionary(ISelectionMgr selMgr);
    }
}