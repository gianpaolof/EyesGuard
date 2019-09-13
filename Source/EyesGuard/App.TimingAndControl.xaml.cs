using EyesGuard.MEF;
using EyesGuard.ViewModels.Interfaces;
using EyesGuard.Views.Animations;
using EyesGuard.Views.Pages;
using EyesGuard.Views.Windows.Interfaces;
using FormatWith;
using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace EyesGuard
{
    public partial class App
    {
        #region Timing and Control :: Common

        /// <summary>
        /// This method prevents user to change protection status in resting mode
        /// </summary>
        /// <returns></returns>
        public static bool CheckIfResting(bool showWarning = true)
        {
            IShortBreakShellView sv = GlobalMEFContainer.Instance.GetExport<IShortBreakShellView>();
            ILongBreakShellView lv = GlobalMEFContainer.Instance.GetExport<ILongBreakShellView>();

            if (sv != null || lv != null)
            {
                if (showWarning)
                    App.ShowWarning(App.LocalizedEnvironment.Translation.EyesGuard.WaitUnitlEndOfBreak, WarningPage.PageStates.Warning);
                return true;
            }
            return false;
        }

        #endregion

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
            UIViewModels.ShortLongBreakTimeRemaining.NextShortBreak = App.LocalizedEnvironment.Translation.EyesGuard.Resting;
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
            UIViewModels.ShortLongBreakTimeRemaining.NextLongBreak = LocalizedEnvironment.Translation.EyesGuard.Resting;
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
            vm.CanCancel =  (Configuration.ForceUserToBreak) ? Visibility.Collapsed : Visibility.Visible;

            
            lv.DataContext = vm;

            LongBreakVisibleTime = App.Configuration.LongBreakDuration;

            if (v != null)
            {
                v.LetItClose = true;
                v.Close();
                //v = null;
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
            if (Configuration.SaveStats)
            {
                Configuration.ShortBreaksCompleted++;
                UpdateStats();
            }

            UIViewModels.ShortLongBreakTimeRemaining.NextShortBreak = LocalizedEnvironment.Translation.EyesGuard.Waiting;
            UIViewModels.NotifyIcon.NextShortBreak = LocalizedEnvironment.Translation.EyesGuard.Waiting;

            IShortBreakShellView v = GlobalMEFContainer.Instance.GetExport<IShortBreakShellView>();

            await v.GetWindow().HideUsingLinearAnimationAsync();
            if (v != null)
            {
                v.LetItClose = true;
                v.Close();
                //v = null;
            }
            if (!App.Configuration.OnlyOneShortBreak && Configuration.ProtectionState == GuardStates.Protecting)
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
            vm.TimeRemaining =LocalizedEnvironment.Translation.EyesGuard.LongBreakTimeRemaining.FormatWith(new
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
            if (Configuration.SaveStats)
            {
                Configuration.LongBreaksCompleted++;
                UpdateStats();
            }
            await lv.GetWindow().HideUsingLinearAnimationAsync();
            lv.Close();
            ShortBreakShownOnce = false;
            if (Configuration.ProtectionState == GuardStates.Protecting)
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
