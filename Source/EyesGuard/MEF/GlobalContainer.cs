using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using Xunit;

namespace EyesGuard.MEF
{
    /// <summary>
    /// This class is a singleton container for all MEF exports (views, viewmodels)
    /// </summary>
    public class GlobalMEFContainer : IDisposable
    {

        [ImportMany(typeof(IContent))]
        public IEnumerable<ExportFactory<IContent, IContentMetadata>> ViewExports
        {
            get;
            set;
        }
        public static GlobalMEFContainer Instance { get; } = new GlobalMEFContainer();

        private CompositionContainer container;

        static GlobalMEFContainer()
        {
        }

        private GlobalMEFContainer()
        {
        }

        public void AddContainer(CompositionContainer c)
        {
            container = c;
        }

        /// <summary>
        /// Returns the exported object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetExport<T>() where T : class
        {
            T vm = container.GetExportedValue<T>();
            Assert.NotNull(vm);

            return vm;
        }

        public object GetView(string uri)
        {
            var obj = container.GetExports<IContent, IContentMetadata>()
                           .Where(e => e.Metadata.ViewUri.Equals(uri))
                           .Select(e => e.Value)
                           .FirstOrDefault();


            return obj;
        }

   




      

    #region IDisposable Support
    private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    container.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~GlobalMEFContainer()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion


    }
}
