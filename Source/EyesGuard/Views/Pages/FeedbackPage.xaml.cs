using EyesGuard.MEF;
using EyesGuard.Views.Windows;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EyesGuard.Views.Pages
{
    /// <summary>
    /// Interaction logic for FeedbackPage.xaml
    /// </summary>
    [Export(typeof(IContent))]
    [ExtensionMetadata(MetadataConstants.FeedbackPage)]
    public partial class FeedbackPage : Page, IContent
    {
        public FeedbackPage()
        {
            InitializeComponent();
        }

        public void OnNavigatedFrom()
        {
            throw new System.NotImplementedException();
        }

        public void OnNavigatedTo()
        {
            throw new System.NotImplementedException();
        }

        private void Feedback_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() => Process.Start("https://github.com/0xaryan/EyesGuard/issues"));

            Utils.GetMainWindow().MainFrame.Content = GlobalMEFContainer.Instance.GetView(MetadataConstants.MainPage);
        }
    }
}
