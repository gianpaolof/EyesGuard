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
        private IShortBreakShellView shortBreakView = GlobalMEFContainer.Instance.GetExport<IShortBreakShellView>();
        private ILongBreakShellView longBreakView = GlobalMEFContainer.Instance.GetExport<ILongBreakShellView>();
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

           
            header.ManualBreakEnabled = false;
           
            slbt.NextShortBreak = App.LocalizedEnvironment.Translation.EyesGuard.Resting;
           
            icon.NextShortBreak = LocalizedEnvironment.Translation.EyesGuard.Resting;

            NextShortBreak = App.Configuration.ShortBreakGap;
            ShortBreakShownOnce = true;

           
            shortBreak.TimeRemaining = ((int)ShortBreakVisibleTime.TotalSeconds).ToString();
            shortBreak.ShortMessage = GetShortWindowMessage();

           
            shortBreakView.DataContext = shortBreak;

            ShortBreakVisibleTime = App.Configuration.ShortBreakDuration;

            try
            {
                await shortBreakView.GetWindow().ShowUsingLinearAnimationAsync();
                shortBreakView.Show();
                shortBreakView.BringIntoView();
                shortBreakView.Focus();
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
           
           
            ShortBreakHandler.Stop();
            LongBreakHandler.Stop();
            
            header.ManualBreakEnabled = false;
            slbt.NextLongBreak = LocalizedEnvironment.Translation.EyesGuard.Resting;
  
            icon.NextLongBreak = LocalizedEnvironment.Translation.EyesGuard.Resting;

            NextShortBreak = App.Configuration.ShortBreakGap;
            NextLongBreak = App.Configuration.LongBreakGap;

           
            longBreak.TimeRemaining = LocalizedEnvironment.Translation.EyesGuard.LongBreakTimeRemaining.FormatWith(new
            {
                LongBreakVisibleTime.Hours,
                LongBreakVisibleTime.Minutes,
                LongBreakVisibleTime.Seconds
            });
            longBreak.CanCancel = (App.Configuration.ForceUserToBreak) ? Visibility.Collapsed : Visibility.Visible;


            longBreakView.DataContext = longBreak;

            LongBreakVisibleTime = App.Configuration.LongBreakDuration;

            if (shortBreakView != null)
            {
                shortBreakView.LetItClose = true;
                shortBreakView.Close();
            }
            ShortDurationCounter.Stop();
            await longBreakView.GetWindow().ShowUsingLinearAnimationAsync();
            longBreakView.Show();
            longBreakView.BringIntoView();
            longBreakView.Focus();

            LongDurationCounter.Start();
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

        private async void EndShortBreak()
        {
            if (App.Configuration.SaveStats)
            {
                App.Configuration.ShortBreaksCompleted++;
                UpdateStats();
            }
            slbt.NextShortBreak = LocalizedEnvironment.Translation.EyesGuard.Waiting;
            icon.NextShortBreak = LocalizedEnvironment.Translation.EyesGuard.Waiting;

          
            await shortBreakView.GetWindow().HideUsingLinearAnimationAsync();
            if (shortBreakView != null)
            {
                shortBreakView.LetItClose = true;
                shortBreakView.Close();
            }
            if (!App.Configuration.OnlyOneShortBreak && App.Configuration.ProtectionState == GuardStates.Protecting)
            {
                ShortBreakHandler.Start();
            }
            LongBreakHandler.Start();
            ShortDurationCounter.Stop();

            header.ManualBreakEnabled = true;
        }

        private async void LongDurationCounter_Tick(object sender, EventArgs e)
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
                await EndLongBreak();
            }
        }

        private async Task EndLongBreak()
        {
            longBreakView.LetItClose = true;
            if (App.Configuration.SaveStats)
            {
                App.Configuration.LongBreaksCompleted++;
                UpdateStats();
            }
            await longBreakView.GetWindow().HideUsingLinearAnimationAsync();
            longBreakView.Close();
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
