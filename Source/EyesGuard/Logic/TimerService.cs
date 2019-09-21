using EyesGuard.MEF;
using EyesGuard.ViewModels.Interfaces;
using EyesGuard.Views.Windows;
using FormatWith;
using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.ComponentModel.Composition;
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


        public event EventHandler ShortBreakStarted;
        public event EventHandler ShortBreakEnded;
        public event EventHandler ShortBreakTick;

        public event EventHandler LongBreakStarted;
        public event EventHandler LongBreakEnded;
        public event EventHandler LongBreakTick;
        public event EventHandler Initialized;
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

            ShortBreakHandler.Tick += ShortBreakHandler_Tick;
            LongBreakHandler.Tick += LongBreakHandler_Tick;
            PauseHandler.Tick += PauseHandler_Tick;

            ShortDurationCounter.Tick += ShortDurationCounter_Tick;
            LongDurationCounter.Tick += LongDurationCounter_Tick;

            if (Initialized is object)
            {
                Initialized(this, EventArgs.Empty);
            }
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

        public void StartShortBreak()
        {
            StopService();
            NextShortBreak = App.Configuration.ShortBreakGap;
            ShortBreakShownOnce = true;

            if (ShortBreakStarted is object)
            {
                ShortBreakStarted(this, EventArgs.Empty);
            }

            ShortBreakVisibleTime = App.Configuration.ShortBreakDuration;

            ShortDurationCounter.Start();
        }

        public void StartLongBreak()
        {
            StopService();
            NextLongBreak = App.Configuration.LongBreakGap;

            if (LongBreakStarted is object)
            {
                LongBreakStarted(this, EventArgs.Empty);
            }

            ShortDurationCounter.Stop();

            LongBreakVisibleTime = App.Configuration.LongBreakDuration;
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
                    StartLongBreak();
                }
            }
        }


        #endregion

        #region Timing and Control :: During Rest

        private void ShortDurationCounter_Tick(object sender, EventArgs e)
        {
            ShortBreakVisibleTime = ShortBreakVisibleTime.Subtract(TimeSpan.FromSeconds(1));

            if (ShortBreakTick is object)
            {
                ShortBreakTick(this, EventArgs.Empty);
            }


            if ((int)ShortBreakVisibleTime.TotalSeconds == 0)
            {
                EndShortBreak();
            }
        }

        private void EndShortBreak()
        {
            if (ShortBreakEnded is object)
            {
                ShortBreakEnded(this, EventArgs.Empty);
            }

            if (!App.Configuration.OnlyOneShortBreak && App.Configuration.ProtectionState == GuardStates.Protecting)
            {
                ShortBreakHandler.Start();
            }
            LongBreakHandler.Start();
            ShortDurationCounter.Stop();

        }

        private void LongDurationCounter_Tick(object sender, EventArgs e)
        {
            LongBreakVisibleTime = LongBreakVisibleTime.Subtract(TimeSpan.FromSeconds(1));

            if (LongBreakTick is object)
            {
                LongBreakTick(this, EventArgs.Empty);
            }



            if ((int)LongBreakVisibleTime.TotalSeconds == 0)
            {
                EndLongBreak();
            }
        }

        private void EndLongBreak()
        {

            if (LongBreakEnded is object)
            {
                LongBreakEnded(this, EventArgs.Empty);
            }

            ShortBreakShownOnce = false;
            if (App.Configuration.ProtectionState == GuardStates.Protecting)
            {
                ShortBreakHandler.Start();
                LongBreakHandler.Start();
            }
            LongDurationCounter.Stop();
        }

        #endregion
    }
}
