// <copyright file="JBSW.cs" company="Jack Badger Ltd">
// Copyright (c) Jack Badger Ltd. All rights reserved.
// </copyright>

namespace JBLib
{
    using System.Runtime.InteropServices;
    using SldWorks;
    using SwConst;

    [ComVisible(true)]
    [Guid("66AF1BD8-D9A0-4156-85C4-7356A8066785")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("JBLib.Class1")]
    public class JBSW : IJBSW
    {
        private ISldWorks app;

        public JBSW()
        {
        }

        /// <inheritdoc/>
        public string Name { get; set; }

        /// <inheritdoc/>
        public void Init(ISldWorks app)

            => this.app = app ?? throw new System.ArgumentNullException(nameof(app));

        /// <inheritdoc/>
        public swMessageBoxResult_e MessageBox(string message, swMessageBoxIcon_e icon, swMessageBoxBtn_e btn)
        {
            return (swMessageBoxResult_e)app.SendMsgToUser2(message, (int)icon, (int)btn);
        }

        /// <inheritdoc/>
        public object ReturnStuff()
        {
            return new object[] { 1, 2, 3 };
        }
    }
}
