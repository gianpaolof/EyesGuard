using EyesGuard.Extensions;
using EyesGuard.Logic;
using EyesGuard.MEF;
using EyesGuard.ViewModels.Interfaces;
using EyesGuard.Views.Pages;
using EyesGuard.Views.Windows;
using EyesGuard.Views.Windows.Interfaces;
using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using static EyesGuard.Data.LanguageLoader;

namespace EyesGuard
{
    public partial class App
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Check if application is running by startup
            if (e.Args.Length > 0 && e.Args[0] == "/auto")
            {
                LaunchMinimized = true;
            }

            Configurations.Configuration.InitializeLocalFolder();
            Configurations.Configuration.LoadSettingsFromFile();

            InitalizeLocalizedEnvironment();

            if (programInstancesCount > 1)
            {
                MessageBox.Show(LocalizedEnvironment.Translation.Application.DoNotRunMultipleInstances);
                Shutdown();
            }

            InitializeIdleDetector(Configuration.SystemIdleDetectionEnabled);

            ToolTipService.ShowDurationProperty.OverrideMetadata(
                typeof(DependencyObject), new FrameworkPropertyMetadata(int.MaxValue));

            if (Configuration.CustomShortMessages.Length == 0)
            {
                Configuration.CustomShortMessages = new string[]
                {
                    "Stare far-off"
                };
            }

            BasePrequirementsLoaded = true;

            // Ignore paused protecting state
            if (Configuration.ProtectionState == GuardStates.PausedProtecting)
                Configuration.ProtectionState = GuardStates.Protecting;

            if ((int)Configuration.ShortBreakGap.TotalMinutes < 1)
                Configuration.ShortBreakGap = new TimeSpan(0, 1, 0);

            if ((int)Configuration.LongBreakGap.TotalMinutes < 5)
                Configuration.LongBreakGap = new TimeSpan(0, 5, 0);

            if ((int)Configuration.ShortBreakDuration.TotalSeconds < 2)
                Configuration.ShortBreakDuration = new TimeSpan(0, 0, 2);

            if ((int)Configuration.LongBreakDuration.TotalSeconds < 5)
                Configuration.LongBreakDuration = new TimeSpan(0, 0, 5);

            Configuration.SaveSettingsToFile();

            ITimerService timingService = GlobalMEFContainer.Instance.GetExport<ITimerService>();
            timingService.Init();

            if (App.Configuration.ProtectionState == GuardStates.Protecting)
            {
                timingService.StartService();           
            }


            TaskbarIcon = "App.GlobalTaskbarIcon".Translate<TaskbarIcon>();

            INotifyIconViewModel ic = GlobalMEFContainer.Instance.GetExport<INotifyIconViewModel>();
            TaskbarIcon.DataContext = ic;

            UpdateTaskbarIcon();

            if (TaskbarIcon != null && !Configuration.TrayNotificationSaidBefore)
            {
                TaskbarIcon.ShowBalloonTip(
                    LocalizedEnvironment.Translation.Application.Notifications.FirstLaunch.Title,
                    LocalizedEnvironment.Translation.Application.Notifications.FirstLaunch.Message,
                    BalloonIcon.Info);

                Configuration.TrayNotificationSaidBefore = true;
                Configuration.SaveSettingsToFile();
            }
        }

        private void InitalizeLocalizedEnvironment()
        {
            string @default = FsLanguageLoader.DefaultLocale;
            var currentLocale = Configuration.ApplicationLocale;
            CurrentLocale = new CultureInfo(currentLocale);

            if (DesignerExtensions.IsRunningInVisualStudioDesigner)
            {
                LocalizedEnvironment = FsLanguageLoader.CreateEnvironment(currentLocale, designMode: true);
                CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(@default);
                CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(@default);
            }
            else
            {
                if (currentLocale == @default)
                {
                    LocalizedEnvironment = FsLanguageLoader.DefaultEnvironment;
                    CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(@default);
                    CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(@default);
                }
                if (FsLanguageLoader.IsCultureSupportedAndExists(currentLocale))
                {
                    LocalizedEnvironment = FsLanguageLoader.CreateEnvironment(currentLocale);
                    CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(currentLocale);
                    CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(currentLocale);
                }
                else
                {
                    Configuration.ApplicationLocale = @default;
                    Configuration.SaveSettingsToFile();
                    LocalizedEnvironment = FsLanguageLoader.DefaultEnvironment;
                    CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(@default);
                    CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(@default);
                }
            }
        }

        private void InitializeIdleDetector(bool initialStart)
        {
            try
            {
                SystemIdleDetector = new IdleDetector()
                {
                    IdleThreshold = EyesGuardIdleDetectionThreshold,
                    DeferUpdate = false,
                    EnableRaisingEvents = initialStart
                };
                SystemIdleDetector.IdleStateChanged += SystemIdleDetector_IdleStateChanged;

                if (initialStart && SystemIdleDetector.State == IdleDetectorState.Stopped)
                    _ = SystemIdleDetector.RequestStart();
            }
            catch { }
        }

        private void SystemIdleDetector_IdleStateChanged(object sender, IdleStateChangedEventArgs e)
        {
            UpdateIdleActions();
        }

        public static void UpdateIdleActions()
        {
            if (CheckIfResting(showWarning: false)) return;
            IShortLongBreakTimeRemainingViewModel slbt = GlobalMEFContainer.Instance.GetExport<IShortLongBreakTimeRemainingViewModel>();
            slbt.IdleVisibility = (AppIsInIdleState) ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// This method prevents user to change protection status in resting mode
        /// </summary>
        /// <returns></returns>
        public static bool CheckIfResting(bool showWarning = true)
        {
            ShortBreakWindow shortBreakView = GlobalMEFContainer.Instance.ViewContentLoader.GetView(MetadataConstants.ShortBreakWindow) as ShortBreakWindow;
            LongBreakWindow longBreakView = GlobalMEFContainer.Instance.ViewContentLoader.GetView(MetadataConstants.ShortBreakWindow) as LongBreakWindow;



            if (shortBreakView.IsVisible || longBreakView.IsVisible)
            {
                if (showWarning)
                    App.ShowWarning(App.LocalizedEnvironment.Translation.EyesGuard.WaitUnitlEndOfBreak, WarningPage.PageStates.Warning);
                return true;
            }
            return false;
        }

    }
}
