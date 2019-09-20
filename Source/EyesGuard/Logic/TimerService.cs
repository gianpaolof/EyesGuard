using EyesGuard.MEF;
using EyesGuard.ViewModels.Interfaces;
using EyesGuard.Views.Animations;
using EyesGuard.Views.Windows;
using EyesGuard.Views.Windows.Interfaces;
using FormatWith;
using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using static EyesGuard.App;

namespace EyesGuard.Logic
{
    [Export(typeof(ITimerService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class TimerService : ITimerService
    {
        #region private fields
        private static DispatcherTimer ShortBreakHandler { get; set; } = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1) };
        private static DispatcherTimer ShortDurationCounter { get; set; } = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1) };
        private static DispatcherTimer LongBreakHandler { get; set; } = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1) };
        private static DispatcherTimer PauseHandler { get; set; } = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1) };
        private static DispatcherTimer LongDurationCounter { get; set; } = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1) };

        private IHeaderMenuViewModel header = GlobalMEFContainer.Instance.GetExport<IHeaderMenuViewModel>();
        private IShortLongBreakTimeRemainingViewModel slbt = GlobalMEFContainer.Instance.GetExport<IShortLongBreakTimeRemainingViewModel>();
        private INotifyIconViewModel icon = GlobalMEFContainer.Instance.GetExport<INotifyIconViewModel>();
        private IShortBreakViewModel shortBreak = GlobalMEFContainer.Instance.GetExport<IShortBreakViewModel>();
        private ShortBreakWindow shortBreakView = GlobalMEFContainer.Instance.ViewContentLoader.GetView(MetadataConstants.ShortBreakWindow) as ShortBreakWindow;
        private LongBreakWindow longBreakView = GlobalMEFContainer.Instance.ViewContentLoader.GetView(MetadataConstants.ShortBreakWindow) as LongBreakWindow;
        private ILongBreakViewModel longBreak = GlobalMEFContainer.Instance.GetExport<ILongBreakViewModel>();
        #endregion

        public void Init()
        {
            NextShortBreak = App.Configuration.ShortBreakGap;
            NextLongBreak = App.Configuration.LongBreakGap;
            ShortBreakVisibleTime = App.Configuration.ShortBreakDuration;
            LongBreakVisibleTime = App.Configuration.LongBreakDuration;


            UpdateShortTimeString();
            UpdateLongTimeString();
            UpdateKeyTimeVisible();
            UpdateStats();

            ShortBreakHandler.Tick += ShortBreakHandler_Tick;
            LongBreakHandler.Tick += LongBreakHandler_Tick;
            PauseHandler.Tick += PauseHandler_Tick;

            ShortDurationCounter.Tick += ShortDurationCounter_Tick;
            LongDurationCounter.Tick += LongDurationCounter_Tick;

        }

        public void StartService()
        {
            ShortBreakHandler.Start();
            LongBreakHandler.Start();
        }

        public void StopService()
        {
            ShortBreakHandler.Stop();
            LongBreakHandler.Stop();
        }

        public void StartShortHandler()
        {
            ShortBreakHandler.Start();
        }

        public void StartLongHandler()
        {
            LongBreakHandler.Start();
        }

        public void StartPauseHandler()
        {
            PauseHandler.Start();
        }

        public void StopPauseHandler()
        {
            PauseHandler.Stop();
        }

        public void DoShortBreak()
        {
            PrepareForBreak();
            StartShortBreak();
        }

        public void DoLongBreak()
        {
            PrepareForBreak();
            StartLongBreak();
        }

        #region Timing and Control :: Handlers

        private void PauseHandler_Tick(object sender, EventArgs e)
        {
            PauseProtectionSpan = PauseProtectionSpan.Subtract(TimeSpan.FromSeconds(1));
            UpdatePauseTimeString();

            if ((int)PauseProtectionSpan.TotalMilliseconds == 0)
            {
                ResumeProtection();
            }
        }

        private void ShortBreakHandler_Tick(object sender, EventArgs e)
        {
            if (TimersAreEligibleToCountdown)
            {
                NextShortBreak = NextShortBreak.Subtract(TimeSpan.FromSeconds(1));
                UpdateShortTimeString();

                if ((int)NextShortBreak.TotalSeconds == 0)
                {
                    PrepareForBreak();
                    StartShortBreak();
                }
            }
        }
 
        public void StartShortBreak()
        {
            ShortBreakShownOnce = true;
            shortBreak.TimeRemaining = ((int)ShortBreakVisibleTime.TotalSeconds).ToString();
            shortBreak.ShortMessage = GetShortWindowMessage();


            shortBreakView.DataContext = shortBreak;
            ShortBreakVisibleTime = App.Configuration.ShortBreakDuration;
            shortBreakView.ShowAnimation();
            ShortDurationCounter.Start();
        }

        private void PrepareForBreak()
        {
            StopService();
            header.ManualBreakEnabled = false;
            slbt.NextShortBreak = LocalizedEnvironment.Translation.EyesGuard.Resting;
            icon.NextShortBreak = LocalizedEnvironment.Translation.EyesGuard.Resting;
            NextShortBreak = App.Configuration.ShortBreakGap;
        }

        public void StartLongBreak()
        {
            NextLongBreak = App.Configuration.LongBreakGap;
            longBreak.TimeRemaining = LocalizedEnvironment.Translation.EyesGuard.LongBreakTimeRemaining.FormatWith(new
            {
                LongBreakVisibleTime.Hours,
                LongBreakVisibleTime.Minutes,
                LongBreakVisibleTime.Seconds
            });
            longBreak.CanCancel = (App.Configuration.ForceUserToBreak) ? Visibility.Collapsed : Visibility.Visible;

           
            if (shortBreakView.IsVisible ) shortBreakView.HideAnimation();

            ShortDurationCounter.Stop();

            longBreakView.DataContext = longBreak;
            LongBreakVisibleTime = App.Configuration.LongBreakDuration;
            longBreakView.ShowAnimation();
            LongDurationCounter.Start();
        }
        private void LongBreakHandler_Tick(object sender, EventArgs e)
        {
            if (TimersAreEligibleToCountdown)
            {
                NextLongBreak = NextLongBreak.Subtract(TimeSpan.FromSeconds(1));
                UpdateLongTimeString();

                if (App.Configuration.AlertBeforeLongBreak && (int)NextLongBreak.TotalSeconds == 60)
                {
                    App.TaskbarIcon.ShowBalloonTip(
                        LocalizedEnvironment.Translation.EyesGuard.Notifications.LongBreakAlert.Title,
                        LocalizedEnvironment.Translation.EyesGuard.Notifications.LongBreakAlert.Message,
                        BalloonIcon.Info);
                }

                if ((int)NextLongBreak.TotalSeconds == 0)
                {
                    PrepareForBreak();
                    StartLongBreak();
                }
            }
        }

 
        #endregion

        #region Timing and Control :: During Rest

        private void ShortDurationCounter_Tick(object sender, EventArgs e)
        {
            ShortBreakVisibleTime = ShortBreakVisibleTime.Subtract(TimeSpan.FromSeconds(1));
            shortBreak.TimeRemaining = ((int)ShortBreakVisibleTime.TotalSeconds).ToString();
            if ((int)ShortBreakVisibleTime.TotalSeconds == 0)
            {
                EndShortBreak();
            }
        }

        private void EndShortBreak()
        {
            if (App.Configuration.SaveStats)
            {
                App.Configuration.ShortBreaksCompleted++;
                UpdateStats();
            }
            slbt.NextShortBreak = LocalizedEnvironment.Translation.EyesGuard.Waiting;
            icon.NextShortBreak = LocalizedEnvironment.Translation.EyesGuard.Waiting;


            shortBreakView.HideAnimation();
            
            if (!App.Configuration.OnlyOneShortBreak && App.Configuration.ProtectionState == GuardStates.Protecting)
            {
                ShortBreakHandler.Start();
            }
            LongBreakHandler.Start();
            ShortDurationCounter.Stop();

            header.ManualBreakEnabled = true;
        }

        private void LongDurationCounter_Tick(object sender, EventArgs e)
        {
            LongBreakVisibleTime = LongBreakVisibleTime.Subtract(TimeSpan.FromSeconds(1));
            longBreak.TimeRemaining = LocalizedEnvironment.Translation.EyesGuard.LongBreakTimeRemaining.FormatWith(new
            {
                LongBreakVisibleTime.Hours,
                LongBreakVisibleTime.Minutes,
                LongBreakVisibleTime.Seconds
            });

            if ((int)LongBreakVisibleTime.TotalSeconds == 0)
            {
                 EndLongBreak();
            }
        }

        private void EndLongBreak()
        {
            
            if (App.Configuration.SaveStats)
            {
                App.Configuration.LongBreaksCompleted++;
                UpdateStats();
            }

            longBreakView.HideAnimation();
           
            ShortBreakShownOnce = false;
            if (App.Configuration.ProtectionState == GuardStates.Protecting)
            {
                ShortBreakHandler.Start();
                LongBreakHandler.Start();
            }
            LongDurationCounter.Stop();

            header.ManualBreakEnabled = true;
        }

        #endregion
    }
}
