using EyesGuard.Views.Animations;
using EyesGuard.Views.Windows.Interfaces;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Input;
using static EyesGuard.App;

namespace EyesGuard.Views.Windows
{
    /// <summary>
    /// Interaction logic for LongBreakWindow.xaml
    /// </summary>
    [Export(typeof(ILongBreakShellView)), PartCreationPolicy(CreationPolicy.Shared)]
    public partial class LongBreakWindow : Window, ILongBreakShellView
    {
        public LongBreakWindow()
        {
            InitializeComponent();
        }

        public bool LetItClose { get; set; } = false;

        private async void CloseLongBreak_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LetItClose = true;
                if (App.Configuration.SaveStats)
                {
                    App.Configuration.LongBreaksFailed++;
                    App.Configuration.SaveSettingsToFile();
                }
                await (this as Window).HideUsingLinearAnimationAsync();
                Close();
                App.ShortBreakShownOnce = false;
                if (App.Configuration.ProtectionState == GuardStates.Protecting)
                {
                    App.ShortBreakHandler.Start();
                    App.LongBreakHandler.Start();
                }
                LongDurationCounter.Stop();
                App.UIViewModels.HeaderMenu.ManualBreakEnabled = true;
            }
            catch { }
        }

        public Window GetWindow()
        {
            return this;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!LetItClose)
                e.Cancel = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.Configuration.ForceUserToBreak)
                Cursor = Cursors.None;
        }

    }
}
