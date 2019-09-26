using EyesGuard.AppManagers;
using EyesGuard.MEF;
using EyesGuard.Views.Pages;
using EyesGuard.Views.Windows;
using System;
using System.Windows;

namespace EyesGuard.Views.Menus
{
    public partial class EyesGuardTaskbarIcon
    {
        private void TaskbarIcon_TrayMouseDoubleClick(object sender, System.Windows.RoutedEventArgs e)
        {
            if(!Utils.GetMainWindow().IsVisible)
            ChromeManager.Show();
        }



        private void TaskbarIcon_ContextMenuOpening(object sender, System.Windows.Controls.ContextMenuEventArgs e)
        {
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            App.UpdateTaskbarIcon();
        }

       
    }
}
