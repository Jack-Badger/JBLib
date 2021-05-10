using SldWorks;
using SwConst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JBLib.Extensions;
using System.Runtime.InteropServices;

namespace JBLib
{
    [ComVisible(true)]
    [Guid("82F99B69-DFB1-438A-8E02-9C02C5D69827")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("JBLib.Document")]
    public class Document : IDocument
    {
        private IModelDoc2 model;

        public Document()
        {
        }

        public IModelDoc2 Model { get => model; private set => model = value; }

        public void Init(IModelDoc2 model)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
        }

        public IFeature[] FilterFeatures([MarshalAs(UnmanagedType.I4)] swSelectType_e type, string name)
            => model.Features().FilterBySelectType(type).FilterByName(name).ToArray();

    }
}
