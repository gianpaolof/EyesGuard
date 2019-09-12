using System;
using System.ComponentModel.Composition;

namespace EyesGuard.MEF
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ExtensionMetadataAttribute : ExportAttribute
    {
        public string ViewUri { get; private set; }

        public ExtensionMetadataAttribute(string uri) : base(typeof(IContentMetadata))
        {
            this.ViewUri = uri;
        }
    }
}
