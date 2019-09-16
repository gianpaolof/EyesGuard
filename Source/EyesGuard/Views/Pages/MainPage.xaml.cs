using EyesGuard.MEF;
using EyesGuard.ViewModels.Interfaces;
using EyesGuard.Views.Animations;
using EyesGuard.Views.Windows;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static EyesGuard.App;

namespace EyesGuard.Views.Pages
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    [Export(typeof(IContent))]
    [ExtensionMetadata(MetadataConstants.MainPage)]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class MainPage : Page, IContent
    {
        public GuardStates ProtectionState
        {
            get { return (GuardStates)GetValue(ProtectionStateProperty); }
            set { SetValue(ProtectionStateProperty, value);
                UpdatePageText();

                // Ignore paused protecting
                App.Configuration.ProtectionState = value;

                App.Configuration.SaveSettingsToFile();
            }
        }
        public static readonly DependencyProperty ProtectionStateProperty =
            DependencyProperty.Register("ProtectionState", typeof(GuardStates), typeof(MainPage), new PropertyMetadata(GuardStates.Protecting));

        public MainPage()
        {
            InitializeComponent();
            CurrentMainPage = this;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                this.Background = Brushes.Transparent;
            }

            ProtectionState = App.Configuration.ProtectionState;
            IShortLongBreakTimeRemainingViewModel slbt = GlobalMEFContainer.Instance.GetExport<IShortLongBreakTimeRemainingViewModel>();
            DataContext = slbt;
        }

        private void GuardButton_Click(object sender, RoutedEventArgs e)
        {
            if (App.CheckIfResting()) return;

            if (ProtectionState == GuardStates.Protecting)
            {
                ProtectionState = GuardStates.NotProtecting;
                if(App.Configuration.SaveStats) UpdateIntruptOfStats(GuardStates.NotProtecting);
            }
            else if (ProtectionState == GuardStates.NotProtecting)
            {
                ProtectionState = GuardStates.Protecting;
            }
            else if (ProtectionState == GuardStates.PausedProtecting)
            {
                App.ResumeProtection();
            }
        }

        private async void UpdatePageText()
        {
            PageText.Opacity = 0;

            if (ProtectionState == GuardStates.Protecting)
            {
                PageText.Text = App.LocalizedEnvironment.Translation.EyesGuard.GuardStatus.Running;
            }
            else if (ProtectionState == GuardStates.NotProtecting)
            {
                PageText.Text = App.LocalizedEnvironment.Translation.EyesGuard.GuardStatus.Stopped;
            }
            else if (ProtectionState == GuardStates.PausedProtecting)
            {
                PageText.Text = App.LocalizedEnvironment.Translation.EyesGuard.GuardStatus.Paused;
            }

            await PageText.ShowUsingLinearAnimationAsync();
        }

        private void StackPanel_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if((bool)e.NewValue)
            {
                foreach (UIElement uie in ((StackPanel)sender).Children)
                {
                    uie.ShowUsingLinearAnimationAsync();
                }
            }
        }

        public void OnNavigatedFrom()
        {
            
        }

        public void OnNavigatedTo()
        {
           
        }
    }
}
