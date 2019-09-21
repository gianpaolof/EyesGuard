using EyesGuard.MEF;
using EyesGuard.ViewModels.Interfaces;
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
    [ExtensionMetadata(MetadataConstants.StatisticsPage)]
    public partial class Statistics : Page, IContent
    {
        [ImportingConstructor]
        public Statistics(IStatsViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
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
