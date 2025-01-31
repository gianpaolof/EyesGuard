﻿using EyesGuard.AppManagers;
using EyesGuard.Configurations;
using EyesGuard.MEF;
using EyesGuard.ViewModels.Interfaces;
using EyesGuard.Views.Pages;
using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Threading;
using static EyesGuard.Data.LanguageLoader;

namespace EyesGuard
{
    public partial class App
    {
        public static double SystemDpiFactor { get; set; }

        public static IdleDetector SystemIdleDetector { get; set; }

        public static MainPage CurrentMainPage { get; set; }

        public static TaskbarIcon TaskbarIcon { get; set; }

        public static bool ShortBreakShownOnce = false;

        public static Configuration Configuration { get; set; } = new Configuration();

        public static LocalizedEnvironment LocalizedEnvironment { get; set; }

        public static CultureInfo CurrentLocale { get; private set; }

        public static bool LaunchMinimized { get; set; } = true;

        public static bool IsProtectionPaused
        {
            get { return _isProtectionPaused; }
            set
            {
                _isProtectionPaused = value;
                IShortLongBreakTimeRemainingViewModel slbt = GlobalMEFContainer.Instance.GetExport<IShortLongBreakTimeRemainingViewModel>();
                slbt.IsProtectionPaused = value;
                INotifyIconViewModel ic = GlobalMEFContainer.Instance.GetExport<INotifyIconViewModel>();
                ic.PausedVisibility = value ? Visibility.Visible : Visibility.Collapsed;
                SystemIdleDetector.EnableRaisingEvents = !value;
            }
        }

        public static bool AppIsInIdleState =>
            Configuration.SystemIdleDetectionEnabled
            && CurrentMainPage?.ProtectionState == GuardStates.Protecting
            && SystemIdleDetector.IsSystemIdle();

        public static TimeSpan PauseProtectionSpan { get; set; } = TimeSpan.FromSeconds(0);
  
        public bool BasePrequirementsLoaded { get; private set; } = false;
    }
}
