using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace EyesGuard.MEF
{
    /// <summary>
    /// Loadr of the Views
    /// </summary>
    public class ViewContentLoader 
    {

        [ImportMany]
        public IEnumerable<ExportFactory<IContent, IContentMetadata>> ViewExports
        {
            get;
            set;
        }


        public object GetView(string uri)
        {
            // Get the factory for the View. 
            var viewMapping = ViewExports.FirstOrDefault(o =>
             o.Metadata.ViewUri == uri);

            if (viewMapping == null)
                throw new InvalidOperationException(
                 String.Format("Unable to navigate to: {0}. " +
                    "Could not locate the View.",
                    uri));

            var viewFactory = viewMapping.CreateExport();
            var view = viewFactory.Value;
            return view;
        }
    }
}
