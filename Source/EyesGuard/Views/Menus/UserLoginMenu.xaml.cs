using EyesGuard.Views.Pages;
using System.Windows;
using System.Windows.Controls;

namespace EyesGuard.Views.Menus
{
    public partial class UserLoginMenu : UserControl
    {
        public UserLoginMenu()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            App.ShowWarning(App.LocalizedEnvironment.Translation.Application.LoginWarning, WarningPage.PageStates.Info);
        }
    }
}
