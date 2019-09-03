using EyesGuard.Extensions;
using EyesGuard.Localization;
using EyesGuard.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EyesGuard.Views.Pages
{
    /// <summary>
    /// Interaction logic for CustomPause.xaml
    /// </summary>
    public partial class CustomPause : Page
    {
        public CustomPause()
        {
            InitializeComponent();
            DataContext = App.UIViewModels.CustomPause;
            App.UIViewModels.CustomPause.OnLoad();
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
