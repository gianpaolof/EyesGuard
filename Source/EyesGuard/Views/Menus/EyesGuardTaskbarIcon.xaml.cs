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

        private void FiveMinutesPause_Click(object sender, RoutedEventArgs e)
        {
            App.PauseProtection(TimeSpan.FromMinutes(5));
        }

        private void TenMinutesPause_Click(object sender, RoutedEventArgs e)
        {
            App.PauseProtection(TimeSpan.FromMinutes(10));
        }

        private void ThirtyMinutesPause_Click(object sender, RoutedEventArgs e)
        {
            App.PauseProtection(TimeSpan.FromMinutes(30));
        }

        private void OneHourPause_Click(object sender, RoutedEventArgs e)
        {
            App.PauseProtection(TimeSpan.FromHours(1));
        }

        private void TwoHourPause_Click(object sender, RoutedEventArgs e)
        {
            App.PauseProtection(TimeSpan.FromHours(2));
        }

        private void CustomPause_Click(object sender, RoutedEventArgs e)
        {
            Utils.GetMainWindow().MainFrame.Content = GlobalMEFContainer.Instance.GetView(MetadataConstants.CustomPause);

            if (!Utils.GetMainWindow().IsVisible)
                ChromeManager.Show();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void TaskbarIcon_ContextMenuOpening(object sender, System.Windows.Controls.ContextMenuEventArgs e)
        {
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            App.UpdateTaskbarIcon();
        }

        private void StartProtect_Click(object sender, RoutedEventArgs e)
        {
            if (App.CheckIfResting()) return;

            if (App.Configuration.ProtectionState == App.GuardStates.PausedProtecting)
                App.ResumeProtection();
            else
                App.CurrentMainPage.ProtectionState = App.GuardStates.Protecting;
        }

        private void StopProtect_Click(object sender, RoutedEventArgs e)
        {
            if (App.CheckIfResting()) return;

            if (App.Configuration.SaveStats) App.UpdateIntruptOfStats(App.GuardStates.NotProtecting);
            App.CurrentMainPage.ProtectionState = App.GuardStates.NotProtecting;
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            Utils.GetMainWindow().MainFrame.Content = GlobalMEFContainer.Instance.GetView(MetadataConstants.SettingsPage);

            if (!Utils.GetMainWindow().IsVisible)
                ChromeManager.Show();
        }
    }
}
