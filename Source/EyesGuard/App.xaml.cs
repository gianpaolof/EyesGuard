using EyesGuard.Views.Windows;
using System;
using System.Diagnostics;
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

            throw xx;
        }
    }

    public partial class App : Application
    {
        public static App AsApp() => (App)Current;

        public static MainWindow GetMainWindow() => (MainWindow)App.Current.MainWindow;

        public enum GuardStates
        {
            PausedProtecting, Protecting, NotProtecting
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            PresentationTraceSources.Refresh();
            PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Error;
            PresentationTraceSources.DataBindingSource.Listeners.Add(new BindingErrorTraceListener());

            DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            var bex = e.Exception as BindingErrorException;
            if (bex != null)
            {
                MessageBox.Show($"Binding error. {bex.SourceObject}.{bex.SourceProperty} => {bex.TargetElement}.{bex.TargetProperty}");
            }
        }
    }
}
