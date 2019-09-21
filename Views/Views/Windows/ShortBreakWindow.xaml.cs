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
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class ShortBreakWindow : BreakWindow, IBreakShellView, IContent, IPartImportsSatisfiedNotification
    {
        ITimerService timer;
        public ShortBreakWindow()
        {
            InitializeComponent();


        }

        [Import]
        private ITimerService Timer { get; set; }

        [Import]
        private IShortBreakViewModel VM { get; set; }

        public void OnImportsSatisfied()
        {
            DataContext = VM;
            timer = Timer;
            timer.ShortBreakStarted += Timer_ShortBreakStarted;
            timer.ShortBreakEnded += Timer_ShortBreakEnded;
        }

        private void Timer_ShortBreakEnded(object sender, System.EventArgs e)
        {
            HideAnimation();
        }

        private void Timer_ShortBreakStarted(object sender, System.EventArgs e)
        {
            ShowAnimation();
        }

        public bool LetItClose { get; set; } = false;


        public void HideAnimation()
        {
            LetItClose = true;
            base.HideWindow();
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
