using SldWorks;
using SwConst;
using System.Runtime.InteropServices;

namespace JBLib
{
    [ComVisible(true)]
    [Guid("66AF1BD8-D9A0-4156-85C4-7356A8066785")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("JBLib.Class1")]
    public class JBSW : IJBSW
    {
        private ISldWorks _app;

        public JBSW()
        {
        }

        public void Init(ISldWorks app)
            
            => _app = app ?? throw new System.ArgumentNullException(nameof(app));

        public swMessageBoxResult_e MessageBox(string message, swMessageBoxIcon_e icon, swMessageBoxBtn_e btn)
        {
            return (swMessageBoxResult_e)_app.SendMsgToUser2(message, (int)icon, (int)btn);
        }

        public object ReturnStuff()
        {
            return new object[] { 1, 2, 3 };
        }

        public string Name { get; set; }
    }
}
