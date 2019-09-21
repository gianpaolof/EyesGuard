using System;
using System.ComponentModel.Composition.Hosting;

namespace EyesGuard.MEF
{
    /// <summary>
    /// This class is a singleton container for all MEF exports (views, viewmodels)
    /// </summary>
    public class GlobalMEFContainer : IDisposable
    {

        /// <summary>
        /// The loader from which it is possible to get the views
        /// </summary>
        public ViewContentLoader ViewContentLoader { get; private set; }

        public static GlobalMEFContainer Instance { get; } = new GlobalMEFContainer();

        private CompositionContainer container;

        static GlobalMEFContainer()
        {
        }

        private GlobalMEFContainer()
        {
        }

        public void AddViewContentLoader(ViewContentLoader c)
        {
            ViewContentLoader = c;
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



            return vm;
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
