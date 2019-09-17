using EyesGuard.MEF;
using EyesGuard.ViewModels.Interfaces;
using EyesGuard.Views.Animations;
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
    public class TimerService : ITimerService
    {
        static DispatcherTimer ShortBreakHandler { get; set; } = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1) };
        static DispatcherTimer ShortDurationCounter { get; set; } = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1) };
        static DispatcherTimer LongBreakHandler { get; set; } = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1) };
        static DispatcherTimer PauseHandler { get; set; } = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1) };
        static DispatcherTimer LongDurationCounter { get; set; } = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1) };


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

        public void Start()
        {
            ShortBreakHandler.Start();
            LongBreakHandler.Start();
        }

        public void DoShortBreak()
        {
            StartShortBreak();
        }

        public void DoLongBreak()
        {
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
                    StartShortBreak();
                }
            }
        }

        public async void StartShortBreak()
        {
            ShortBreakHandler.Stop();
            LongBreakHandler.Stop();

            UIViewModels.HeaderMenu.ManualBreakEnabled = false;
            IShortLongBreakTimeRemainingViewModel slbt = GlobalMEFContainer.Instance.GetExport<IShortLongBreakTimeRemainingViewModel>();
            slbt.NextShortBreak = App.LocalizedEnvironment.Translation.EyesGuard.Resting;
            UIViewModels.NotifyIcon.NextShortBreak = LocalizedEnvironment.Translation.EyesGuard.Resting;

            NextShortBreak = App.Configuration.ShortBreakGap;
            ShortBreakShownOnce = true;

            IShortBreakViewModel vm = GlobalMEFContainer.Instance.GetExport<IShortBreakViewModel>();
            vm.TimeRemaining = ((int)ShortBreakVisibleTime.TotalSeconds).ToString();
            vm.ShortMessage = GetShortWindowMessage();

            IShortBreakShellView v = GlobalMEFContainer.Instance.GetExport<IShortBreakShellView>();
            v.DataContext = vm;

            ShortBreakVisibleTime = App.Configuration.ShortBreakDuration;

            try
            {
                await v.GetWindow().ShowUsingLinearAnimationAsync();
                v.Show();
                v.BringIntoView();
                v.Focus();
            }
            catch { }

            ShortDurationCounter.Start();
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
                    StartLongBreak();
                }
            }
        }

        public async void StartLongBreak()
        {
            IShortBreakShellView v = GlobalMEFContainer.Instance.GetExport<IShortBreakShellView>();
            ILongBreakShellView lv = GlobalMEFContainer.Instance.GetExport<ILongBreakShellView>();
            ShortBreakHandler.Stop();
            LongBreakHandler.Stop();
            UIViewModels.HeaderMenu.ManualBreakEnabled = false;
            IShortLongBreakTimeRemainingViewModel slbt = GlobalMEFContainer.Instance.GetExport<IShortLongBreakTimeRemainingViewModel>();
            slbt.NextLongBreak = LocalizedEnvironment.Translation.EyesGuard.Resting;
            UIViewModels.NotifyIcon.NextLongBreak = LocalizedEnvironment.Translation.EyesGuard.Resting;

            NextShortBreak = App.Configuration.ShortBreakGap;
            NextLongBreak = App.Configuration.LongBreakGap;

            ILongBreakViewModel vm = GlobalMEFContainer.Instance.GetExport<ILongBreakViewModel>();
            vm.TimeRemaining = LocalizedEnvironment.Translation.EyesGuard.LongBreakTimeRemaining.FormatWith(new
            {
                LongBreakVisibleTime.Hours,
                LongBreakVisibleTime.Minutes,
                LongBreakVisibleTime.Seconds
            });
            vm.CanCancel = (App.Configuration.ForceUserToBreak) ? Visibility.Collapsed : Visibility.Visible;


            lv.DataContext = vm;

            LongBreakVisibleTime = App.Configuration.LongBreakDuration;

            if (v != null)
            {
                v.LetItClose = true;
                v.Close();
            }
            ShortDurationCounter.Stop();
            await lv.GetWindow().ShowUsingLinearAnimationAsync();
            lv.Show();
            lv.BringIntoView();
            lv.Focus();

            LongDurationCounter.Start();
        }

        #endregion

        #region Timing and Control :: During Rest

        private void ShortDurationCounter_Tick(object sender, EventArgs e)
        {
            ShortBreakVisibleTime = ShortBreakVisibleTime.Subtract(TimeSpan.FromSeconds(1));
            IShortBreakViewModel vm = GlobalMEFContainer.Instance.GetExport<IShortBreakViewModel>();
            vm.TimeRemaining = ((int)ShortBreakVisibleTime.TotalSeconds).ToString();
            if ((int)ShortBreakVisibleTime.TotalSeconds == 0)
            {
                EndShortBreak();
            }
        }

        private async void EndShortBreak()
        {
            if (App.Configuration.SaveStats)
            {
                App.Configuration.ShortBreaksCompleted++;
                UpdateStats();
            }
            IShortLongBreakTimeRemainingViewModel slbt = GlobalMEFContainer.Instance.GetExport<IShortLongBreakTimeRemainingViewModel>();
            slbt.NextShortBreak = LocalizedEnvironment.Translation.EyesGuard.Waiting;
            UIViewModels.NotifyIcon.NextShortBreak = LocalizedEnvironment.Translation.EyesGuard.Waiting;

            IShortBreakShellView v = GlobalMEFContainer.Instance.GetExport<IShortBreakShellView>();

            await v.GetWindow().HideUsingLinearAnimationAsync();
            if (v != null)
            {
                v.LetItClose = true;
                v.Close();
            }
            if (!App.Configuration.OnlyOneShortBreak && App.Configuration.ProtectionState == GuardStates.Protecting)
            {
                ShortBreakHandler.Start();
            }
            LongBreakHandler.Start();
            ShortDurationCounter.Stop();

            UIViewModels.HeaderMenu.ManualBreakEnabled = true;
        }

        private async void LongDurationCounter_Tick(object sender, EventArgs e)
        {
            LongBreakVisibleTime = LongBreakVisibleTime.Subtract(TimeSpan.FromSeconds(1));
            ILongBreakViewModel vm = GlobalMEFContainer.Instance.GetExport<ILongBreakViewModel>();
            vm.TimeRemaining = LocalizedEnvironment.Translation.EyesGuard.LongBreakTimeRemaining.FormatWith(new
            {
                LongBreakVisibleTime.Hours,
                LongBreakVisibleTime.Minutes,
                LongBreakVisibleTime.Seconds
            });

            if ((int)LongBreakVisibleTime.TotalSeconds == 0)
            {
                await EndLongBreak();
            }
        }

        private async Task EndLongBreak()
        {
            ILongBreakShellView lv = GlobalMEFContainer.Instance.GetExport<ILongBreakShellView>();
            lv.LetItClose = true;
            if (App.Configuration.SaveStats)
            {
                App.Configuration.LongBreaksCompleted++;
                UpdateStats();
            }
            await lv.GetWindow().HideUsingLinearAnimationAsync();
            lv.Close();
            ShortBreakShownOnce = false;
            if (App.Configuration.ProtectionState == GuardStates.Protecting)
            {
                ShortBreakHandler.Start();
                LongBreakHandler.Start();
            }
            LongDurationCounter.Stop();

            UIViewModels.HeaderMenu.ManualBreakEnabled = true;
        }

        #endregion
    }
}
