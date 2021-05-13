using JBLib.Extensions;
using SldWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace JBLib
{
    [ComVisible(true)]
    [Guid("AF7F78E3-52E4-4533-9D5F-70FDDAF06A0A")]
    [ClassInterface(ClassInterfaceType.None)]
    public class ModelUpdater : IModelUpdater
    {
        protected ISldWorks app;

        protected Attachments Attachments { get; private set; }

        public bool Initialize(ISldWorks app)
        {
            this.app = app;

            if (app.IActiveDoc2 is IModelDoc2 model)
            {
                var attachments = model.GetAttachmentsEx();
                if (attachments.Any())
                {
                    Attachments = new Attachments(attachments);
                    return true;
                }
            }
            return false;
        }

        public int AttachmentsCount() => Attachments.TotalCount;

        public string[] AttachmentExtensions() => Attachments.Extensions.ToArray();

        public string[] GetAttachments(string extension, out bool[] linked)
        {
            var filepaths = new List<string>();
            var isLinked = new List<bool>();

            foreach(var (f, l) in Attachments[extension])
            {
                filepaths.Add(f);
                isLinked.Add(l);
            }

            linked = isLinked.ToArray();

            return filepaths.ToArray();
        }

    }
}
