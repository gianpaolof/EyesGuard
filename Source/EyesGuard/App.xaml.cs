using EyesGuard.MEF;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;

namespace EyesGuard
{
    public class BindingErrorException : Exception
    {
        public string SourceObject { get; set; }
        public string SourceProperty { get; set; }
        public string TargetElement { get; set; }
        public string TargetProperty { get; set; }

        public BindingErrorException()
            : base() { }

        public BindingErrorException(string message)
            : base(message) { }

    }

    public class BindingErrorTraceListener : TraceListener
    {
        private const string BindingErrorPattern = @"^BindingExpression path error(?:.+)'(.+)' property not found(?:.+)object[\s']+(.+?)'(?:.+)target element is '(.+?)'(?:.+)target property is '(.+?)'(?:.+)$";

        public override void Write(string s) { }

        public override void WriteLine(string message)
        {
            var xx = new BindingErrorException(message);

            var match = Regex.Match(message, BindingErrorPattern);
            if (match.Success)
            {
                xx.SourceObject = match.Groups[2].ToString();
                xx.SourceProperty = match.Groups[1].ToString();
                xx.TargetElement = match.Groups[3].ToString();
                xx.TargetProperty = match.Groups[4].ToString();
            }

            //throw xx;
        }
    }

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

            EnableTracing();

            base.OnStartup(e);

        }

        protected override void OnExit(ExitEventArgs e)
        {
            GlobalMEFContainer.Instance.Dispose();
            base.OnExit(e);
        }

        private void EnableTracing()
        {
            PresentationTraceSources.Refresh();
            PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Error;
            PresentationTraceSources.DataBindingSource.Listeners.Add(new BindingErrorTraceListener());

            DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        private void ComposeAssemblyCatalog()
        {
            Assembly ea = Assembly.GetExecutingAssembly();
            string path = ea.Location;
            List<Exception> exceptions = new List<Exception>();
            AggregateCatalog aggregateCatalog = new AggregateCatalog();

            aggregateCatalog.Catalogs.Add(new AssemblyCatalog(ea));

            var files = Directory.EnumerateFiles(Path.GetDirectoryName(path), "*.dll", SearchOption.AllDirectories);

  
            foreach (var file in files)
            {
                try
                {
                    var assemblyCatalog = new AssemblyCatalog(file);

                    aggregateCatalog.Catalogs.Add(assemblyCatalog);
                }
                catch (ReflectionTypeLoadException ex)
                {
                    foreach (var exception in ex.LoaderExceptions)
                    {
                        exceptions.Add(exception);
                    }
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            var c  = new CompositionContainer(aggregateCatalog);
            c.ComposeParts(this);

            var cc = new CompositionContainer(aggregateCatalog);
            var vcl = new ViewContentLoader();
            cc.ComposeParts(vcl);

            //DumpMEF.Dump(path);

            GlobalMEFContainer.Instance.AddViewContentLoader(vcl);
            GlobalMEFContainer.Instance.AddContainer(c);
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            if (e.Exception is BindingErrorException bex)
            {
                MessageBox.Show($"Binding error. {bex.SourceObject}.{bex.SourceProperty} => {bex.TargetElement}.{bex.TargetProperty}");
            }
        }
    }
}
