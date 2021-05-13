using SldWorks;
using System.Runtime.InteropServices;

namespace JBLib
{
    [ComVisible(true)]
    [Guid("97717CFA-B0E8-4C46-863F-287099ACEF88")]
    [TypeLibImportClass(typeof(ModelUpdater))]
    public interface IModelUpdater
    {
        string[] AttachmentExtensions();
        int AttachmentsCount();
        string[] GetAttachments(string extension, out bool[] linked);
        bool Initialize(ISldWorks app);
    }
}