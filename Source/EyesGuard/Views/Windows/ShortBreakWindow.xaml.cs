using EyesGuard.Logic;
using EyesGuard.MEF;
using EyesGuard.ViewModels.Interfaces;
using EyesGuard.Views.Windows.Interfaces;
using System.ComponentModel.Composition;
using System.Windows;

namespace EyesGuard.Views.Windows
{
    /// <summary>
    /// Interaction logic for ShortBreakWindow.xaml
    /// </summary>

    [Export(typeof(IContent))]
    [ExtensionMetadata(MetadataConstants.ShortBreakWindow)]
    public partial class ShortBreakWindow : BreakWindow, IBreakShellView, IContent, IPartImportsSatisfiedNotification
    {
        public ShortBreakWindow()
        {
            InitializeComponent();


        }

        public bool LetItClose { get; set; } = false;

        [Import]
        private ITimerService Timer { get; set; }


        [Import]
        private IShortBreakViewModel ShortBreakVM { get; set; }


        public void HideAnimation()
        {
            LetItClose = true;
            base.HideWindow();
        }

        public void OnImportsSatisfied()
        {
            Timer.ShortBreakStarted += Timer_ShortBreakStarted;
            Timer.ShortBreakEnded += Timer_ShortBreakEnded;
            DataContext = ShortBreakVM;
        }

        private void Timer_ShortBreakEnded(object sender, System.EventArgs e)
        {
            HideAnimation();
        }

        private void Timer_ShortBreakStarted(object sender, System.EventArgs e)
        {
            ShowAnimation();
        }

        public void OnNavigatedFrom()
        {
            throw new System.NotImplementedException();
        }

        public void OnNavigatedTo()
        {
            throw new System.NotImplementedException();
        }

        public void ShowAnimation()
        {
            base.ShowWindow();
        }

        bool IBreakShellView.IsVisible()
        {
            return IsVisible;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!LetItClose)
                e.Cancel = true;
        }
    }
}
