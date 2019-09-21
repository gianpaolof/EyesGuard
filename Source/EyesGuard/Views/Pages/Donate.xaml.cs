using EyesGuard.MEF;
using EyesGuard.Views.Windows;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EyesGuard.Views.Pages
{
    /// <summary>
    /// Interaction logic for Donate.xaml
    /// </summary>
    [Export(typeof(IContent))]
    [ExtensionMetadata(MetadataConstants.DonatePage)]
    public partial class Donate : Page, IContent
    {
        public Donate()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                this.Background = Brushes.Transparent;
            }
        }

        private void DonateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start($"https://donorbox.org/eyes-guard-donate");
            } catch { }
            var donateText = App.LocalizedEnvironment.Translation.Application.Donate.DonationButtonClicked.Content;
            App.ShowWarning(
                $"{donateText.Thanks}\n{donateText.Redirect}\n\n{donateText.FeedbackNotice}"
                , WarningPage.PageStates.Donate, new MainPage());
        }

        public void OnNavigatedFrom()
        {
            
        }

        public void OnNavigatedTo()
        {
           
        }
    }
}
