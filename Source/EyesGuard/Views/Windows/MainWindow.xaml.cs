using EyesGuard.AppManagers;
using EyesGuard.Configurations;
using EyesGuard.MEF;
using EyesGuard.ViewModels;
using EyesGuard.ViewModels.Interfaces;
using EyesGuard.Views.Animations;
using EyesGuard.Views.Pages;
using EyesGuard.Views.Windows.Interfaces;
using System;
using System.ComponentModel.Composition;
using System.Reflection;
using System.Runtime.Versioning;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace EyesGuard.Views.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Export(typeof(IShellView)), PartCreationPolicy(CreationPolicy.Shared)]
    public partial class MainWindow : Window, IShellView
    {
        public MainWindow()
        {
            InitializeComponent();

            if (App.LaunchMinimized)
                this.Hide();
            string ver = Assembly.GetEntryAssembly().GetCustomAttribute<TargetFrameworkAttribute>().FrameworkName;
            version.Text = ver;
        }

        [Import]
        public IHeaderMenuViewModel Header { get; set; }

        public MainWindow GetMainWindow()
        {
            return this;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            ChromeManager.Hide();
        }

        private void MaxRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private bool minimizing = false;

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (!minimizing)
            {
                minimizing = true;

                WindowState = WindowState.Minimized;
                minimizing = false;
            }
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                MaximizeRect.Visibility = Visibility.Collapsed;
                RestoreCanvas.Visibility = Visibility.Visible;
            }
            else
            {
                MaximizeRect.Visibility = Visibility.Visible;
                RestoreCanvas.Visibility = Visibility.Collapsed;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) { }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Configuration.LoadSettingsFromFile();
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            CurrentPageTitleBlock.Text = ((Page)MainFrame.Content).Title;
            CurrentPageTitleBlock.MarginFadeInAnimation(new Thickness(0), new Thickness(20, 0, 0, 0), TimeSpan.FromMilliseconds(500));

            MainFrame.MarginFadeInAnimation(new Thickness(0), new Thickness(20, 0, 0, 0), TimeSpan.FromMilliseconds(500));
        }

        private void Title_Click(object sender, RoutedEventArgs e)
        {
            Utils.GetMainWindow().MainFrame.Content = GlobalMEFContainer.Instance.GetView(MetadataConstants.MainPage);
        }
    }
}
