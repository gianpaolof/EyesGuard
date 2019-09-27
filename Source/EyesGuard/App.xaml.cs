using EyesGuard.MEF;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using System.Windows;

namespace EyesGuard
{


    public partial class App : Application
    {

        public static App AsApp() => (App)Current;

        public enum GuardStates
        {
            PausedProtecting, Protecting, NotProtecting
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            ComposeAssemblyCatalog();
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            GlobalMEFContainer.Instance.Dispose();
            base.OnExit(e);
        }

       
        private void ComposeAssemblyCatalog()
        {
            Assembly ea = Assembly.GetExecutingAssembly();
            string path = ea.Location;
            List<Exception> exceptions = new List<Exception>();
            AggregateCatalog aggregateCatalog = new AggregateCatalog();

            aggregateCatalog.Catalogs.Add(new AssemblyCatalog(ea));

            var c = new CompositionContainer(aggregateCatalog);
            c.ComposeParts(this);

            //DumpMEF.Dump(path);

            GlobalMEFContainer.Instance.AddContainer(c);
        }
    }
}
