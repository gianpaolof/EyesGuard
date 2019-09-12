using EyesGuard.MEF;
using EyesGuard.Views.Windows;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EyesGuard.Views.Pages
{
    /// <summary>
    /// Interaction logic for Statistics.xaml
    /// </summary>
    [Export(typeof(IContent))]
    [ExtensionMetadata(MetadataConstants.SettingsPage)]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class Statistics : Page, IContent
    {
        public Statistics()
        {
            InitializeComponent();
            DataContext = App.UIViewModels.Stats;
        }

        public void OnNavigatedFrom()
        {
            
        }

        public void OnNavigatedTo()
        {
            
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                this.Background = Brushes.Transparent;
            }
        }
    }
}
