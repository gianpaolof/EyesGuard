using EyesGuard.AppManagers;
using EyesGuard.Logic;
using EyesGuard.MEF;
using EyesGuard.ViewModels.Interfaces;
using FormatWith;
using System.Windows;

namespace EyesGuard
{
    public partial class App
    {
        public static void UpdateLongTimeString()
        {
            if (NextLongBreak.TotalSeconds < 60)
            {
                IShortLongBreakTimeRemainingViewModel slbt = GlobalMEFContainer.Instance.GetExport<IShortLongBreakTimeRemainingViewModel>();
                slbt.NextLongBreak =
                    LocalizedEnvironment.Translation.EyesGuard.TimeRemaining.LongBreak.Seconds.FormatWith(new
                    {
                        Seconds = (int)NextLongBreak.TotalSeconds
                    });
            }
            else
            {
                IShortLongBreakTimeRemainingViewModel slbt = GlobalMEFContainer.Instance.GetExport<IShortLongBreakTimeRemainingViewModel>();
                slbt.NextLongBreak =
                    LocalizedEnvironment.Translation.EyesGuard.TimeRemaining.LongBreak.Minutes.FormatWith(new
                    {
                        Minutes = (int)NextLongBreak.TotalMinutes
                    });
            }
            INotifyIconViewModel ic = GlobalMEFContainer.Instance.GetExport<INotifyIconViewModel>();
            ic.NextLongBreak =
                $"{NextLongBreak.Hours}:{NextLongBreak.Minutes}:{NextLongBreak.Seconds}";
        }

        public static void UpdateShortTimeString()
        {
            if (NextShortBreak.TotalSeconds < 60)
            {
                IShortLongBreakTimeRemainingViewModel slbt = GlobalMEFContainer.Instance.GetExport<IShortLongBreakTimeRemainingViewModel>();
                slbt.NextShortBreak =
                    LocalizedEnvironment.Translation.EyesGuard.TimeRemaining.ShortBreak.Seconds.FormatWith(new
                    {
                        Seconds = (int)NextShortBreak.TotalSeconds
                    });
            }
            else
            {
                IShortLongBreakTimeRemainingViewModel slbt = GlobalMEFContainer.Instance.GetExport<IShortLongBreakTimeRemainingViewModel>();
                slbt.NextShortBreak =
                    LocalizedEnvironment.Translation.EyesGuard.TimeRemaining.ShortBreak.Minutes.FormatWith(new
                    {
                        Minutes = (int)NextShortBreak.TotalMinutes
                    });
            }
            INotifyIconViewModel ic = GlobalMEFContainer.Instance.GetExport<INotifyIconViewModel>();
            ic.NextShortBreak =
                $"{NextShortBreak.Hours}:{NextShortBreak.Minutes}:{NextShortBreak.Seconds}";
        }

        public static void UpdatePauseTimeString()
        {
            if (PauseProtectionSpan.TotalSeconds < 60)
            {
                IShortLongBreakTimeRemainingViewModel slbt = GlobalMEFContainer.Instance.GetExport<IShortLongBreakTimeRemainingViewModel>();
                slbt.PauseTime =
                    LocalizedEnvironment.Translation.EyesGuard.TimeRemaining.PauseTime.Seconds.FormatWith(new
                    {
                        Seconds = (int)PauseProtectionSpan.TotalSeconds
                    });
            }
            else
            {
                IShortLongBreakTimeRemainingViewModel slbt = GlobalMEFContainer.Instance.GetExport<IShortLongBreakTimeRemainingViewModel>();
                slbt.PauseTime =
                    LocalizedEnvironment.Translation.EyesGuard.TimeRemaining.PauseTime.Minutes.FormatWith(new
                    {
                        Minutes = (int)PauseProtectionSpan.TotalMinutes
                    });
            }
            INotifyIconViewModel ic = GlobalMEFContainer.Instance.GetExport<INotifyIconViewModel>();
            ic.PauseRemaining =
                $"{PauseProtectionSpan.Hours}:{PauseProtectionSpan.Minutes}:{PauseProtectionSpan.Seconds}";
        }

        public static void UpdateTimeHandlers()
        {
            if (Configuration.ProtectionState == GuardStates.Protecting)
            {
                ITimerService timingService = GlobalMEFContainer.Instance.GetExport<ITimerService>();

                if (!(Configuration.OnlyOneShortBreak && ShortBreakShownOnce))
                    timingService.StartShortHandler();

                timingService.StartLongHandler();
            }
            else if (Configuration.ProtectionState == GuardStates.NotProtecting)
            {
                ITimerService timingService = GlobalMEFContainer.Instance.GetExport<ITimerService>();
                timingService.StopService();
            }
        }

        public static void UpdateKeyTimeVisible()
        {
            IShortLongBreakTimeRemainingViewModel slbt = GlobalMEFContainer.Instance.GetExport<IShortLongBreakTimeRemainingViewModel>();
            IHeaderMenuViewModel h = GlobalMEFContainer.Instance.GetExport<IHeaderMenuViewModel>();
            if (Configuration.KeyTimesVisible)
            {
                slbt.TimeRemainingVisibility = Visibility.Visible;
                h.IsTimeItemChecked = true;
            }
            else
            {
                slbt.TimeRemainingVisibility = Visibility.Collapsed;
                h.IsTimeItemChecked = false;
            }
        }

        public static void UpdateLongShortVisibility()
        {
            if (Configuration.ProtectionState == GuardStates.Protecting)
            {
                IShortLongBreakTimeRemainingViewModel slbt = GlobalMEFContainer.Instance.GetExport<IShortLongBreakTimeRemainingViewModel>();
                slbt.LongShortVisibility = Visibility.Visible;
            }
            else
            {
                IShortLongBreakTimeRemainingViewModel slbt = GlobalMEFContainer.Instance.GetExport<IShortLongBreakTimeRemainingViewModel>();
                slbt.LongShortVisibility = Visibility.Collapsed;
            }
        }

        public static void UpdateTaskbarIcon()
        {
            TaskbarIconManager.UpdateTaskbarIcon();
        }

        public static void UpdateIntruptOfStats(GuardStates state)
        {
            if (state == GuardStates.PausedProtecting)
            {
                Configuration.PauseCount++;
                Configuration.SaveSettingsToFile();
                IStatsViewModel stats = GlobalMEFContainer.Instance.GetExport<IStatsViewModel>();
                stats.PauseCount = Configuration.PauseCount;
            }
            else if (state == GuardStates.NotProtecting)
            {
                Configuration.StopCount++;
                Configuration.SaveSettingsToFile();
                IStatsViewModel stats = GlobalMEFContainer.Instance.GetExport<IStatsViewModel>();
                stats.StopCount = Configuration.StopCount;
            }
        }
    }
}
