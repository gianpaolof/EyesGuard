using EyesGuard.AppManagers;
using EyesGuard.MEF;
using EyesGuard.Views.Animations;
using EyesGuard.Views.Pages;
using EyesGuard.Views.Windows;
using System;
using System.Windows;
using System.Windows.Controls;

namespace EyesGuard.Views.Menus
{
    public partial class HeaderMenu : UserControl
    {
        

        public HeaderMenu()
        {
            InitializeComponent();

            DataContext = App.UIViewModels.HeaderMenu;
        }

        private void GoToStatictictsPage_Click(object sender, RoutedEventArgs e)
        {
            var vc = GlobalMEFContainer.Instance.ViewContentLoader;

            Utils.GetMainWindow().MainFrame.Content = vc.GetView(MetadataConstants.StatisticsPage);
        }

        private void GoToEyeSightImprove_Click(object sender, RoutedEventArgs e)
        {
        }

        private void GoToSettingsPage_Click(object sender, RoutedEventArgs e)
        {
            var vc = GlobalMEFContainer.Instance.ViewContentLoader;

            Utils.GetMainWindow().MainFrame.Content = vc.GetView(MetadataConstants.SettingsPage);
        }

        private void GoToMainPage_Click(object sender, RoutedEventArgs e)
        {
            var vc = GlobalMEFContainer.Instance.ViewContentLoader;

            Utils.GetMainWindow().MainFrame.Content = vc.GetView(MetadataConstants.MainPage);
        }

        private void ShowHideTimeRemaining_Click(object sender, RoutedEventArgs e)
        {
            App.Configuration.KeyTimesVisible = (App.Configuration.KeyTimesVisible) ? false : true;
            App.Configuration.SaveSettingsToFile();
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
            var vc = GlobalMEFContainer.Instance.ViewContentLoader;

            Utils.GetMainWindow().MainFrame.Content = vc.GetView (MetadataConstants.CustomPause);
        }

        private async void ApplicationExit_Click(object sender, RoutedEventArgs e)
        {
            await Utils.GetMainWindow().HideUsingLinearAnimationAsync();
            App.Current.Shutdown();
        }

        private void HideApp_Click(object sender, RoutedEventArgs e)
        {
            ChromeManager.Hide();
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            App.ShowWarning(
                App.LocalizedEnvironment.Translation.Application.HelpPageText,
                WarningPage.PageStates.Info);
        }

        private void Resources_Click(object sender, RoutedEventArgs e)
        {
            var resourcesBase = App.LocalizedEnvironment.Translation.Application.Resources;
            App.ShowWarning(
                $"    - {resourcesBase.Content.Icons}\n    - {resourcesBase.Content.UIKit}"
                , WarningPage.PageStates.Info);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var aboutBase = App.LocalizedEnvironment.Translation.Application.About;
            App.ShowWarning(
                $"{aboutBase.Content.InnerTitle}\n\n   {aboutBase.Content.PublisherInfo}\n"
                , WarningPage.PageStates.About);
        }

        private void StartShortBreak_Click(object sender, RoutedEventArgs e)
        {
            App.AsApp().StartShortBreak();
        }

        private void StartLongBreak_Click(object sender, RoutedEventArgs e)
        {
            App.AsApp().StartLongBreak();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            var aboutBase = App.LocalizedEnvironment.Translation.Application.About;
            App.ShowWarning(
                $"{aboutBase.Content.InnerTitle}\n\n   {aboutBase.Content.PublisherInfo}\n"
                , WarningPage.PageStates.About);
        }

        private void Donate_Click(object sender, RoutedEventArgs e)
        {
            var vc = GlobalMEFContainer.Instance.ViewContentLoader;

            Utils.GetMainWindow().MainFrame.Content = vc.GetView(MetadataConstants.DonatePage);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void Feedback_Menu_Click(object sender, RoutedEventArgs e)
        {
            var vc = GlobalMEFContainer.Instance.ViewContentLoader;

            Utils.GetMainWindow().MainFrame.Content = vc.GetView(MetadataConstants.FeedbackPage);
        }
    }
}
