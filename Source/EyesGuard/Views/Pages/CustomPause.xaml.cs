using EyesGuard.MEF;
using EyesGuard.ViewModels;
using EyesGuard.Views.Windows;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EyesGuard.Views.Pages
{
    /// <summary>
    /// Interaction logic for CustomPause.xaml
    /// </summary>
    [Export(typeof(IContent))]
    [ExtensionMetadata(MetadataConstants.CustomPause)]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class CustomPause : Page , IContent, IPartImportsSatisfiedNotification
    {
        public CustomPause()
        {
            InitializeComponent();
        }

        [Import]
        private CustomPauseViewModel CustomPauseVM { get; set;}

        public void OnImportsSatisfied()
        {
            DataContext = CustomPauseVM;
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
