using EyesGuard.Logic;
using EyesGuard.ViewModels.Interfaces;
using System;
using System.ComponentModel.Composition;
using System.Windows;

namespace EyesGuard.ViewModels
{
    [Export(typeof(IShortLongBreakTimeRemainingViewModel))]
    public class ShortLongBreakTimeRemainingViewModel : ViewModelBase, IShortLongBreakTimeRemainingViewModel,IPartImportsSatisfiedNotification
    {
        public ShortLongBreakTimeRemainingViewModel()
        {
           NextShortBreak = string.Empty; ;
           NextLongBreak = string.Empty; ;
           PauseTime = string.Empty; ;
           TimeRemainingVisibility = Visibility.Visible;
        }


        [Import]
        private ITimerService Timer { get; set; }

        public void OnImportsSatisfied()
        {
            Timer.ShortBreakStarted += Timer_ShortBreakStarted;
            Timer.ShortBreakEnded += Timer_ShortBreakEnded;
            Timer.LongBreakStarted += Timer_LongBreakStarted;
            Timer.LongBreakEnded += Timer_LongBreakEnded;
        }

        private void Timer_LongBreakEnded(object sender, EventArgs e)
        {
            NextShortBreak = App.LocalizedEnvironment.Translation.EyesGuard.Waiting;
        }

        private void Timer_LongBreakStarted(object sender, EventArgs e)
        {
            NextShortBreak = App.LocalizedEnvironment.Translation.EyesGuard.Resting;
        }

        private void Timer_ShortBreakEnded(object sender, EventArgs e)
        {
            NextShortBreak = App.LocalizedEnvironment.Translation.EyesGuard.Waiting;
        }

        private void Timer_ShortBreakStarted(object sender, EventArgs e)
        {
            NextShortBreak = App.LocalizedEnvironment.Translation.EyesGuard.Resting;
        }

        public string NextShortBreak {
            get { return GetValue(() => NextShortBreak); }
            set { SetValue(() => NextShortBreak, value); }
        }


        public string NextLongBreak
        {
            get { return GetValue(() => NextLongBreak); }
            set { SetValue(() => NextLongBreak, value); }
        }


        public string PauseTime
        {
            get { return GetValue(() => PauseTime); }
            set { SetValue(() => PauseTime, value); }
        }

        public Visibility TimeRemainingVisibility
        {
            get { return GetValue(() => TimeRemainingVisibility); }
            set { SetValue(() => TimeRemainingVisibility, value); }
        }

        private bool _protectionPause = false;
        public bool IsProtectionPaused
        {
            get => _protectionPause;

            set
            {
                SetField(ref _protectionPause, value);
                if (value)
                {
                    PauseVisibility = Visibility.Visible;
                    LongShortVisibility = Visibility.Collapsed;
                }
                else
                {
                    PauseVisibility = Visibility.Collapsed;
                    LongShortVisibility = Visibility.Visible;
                }

                OnPropertyChanged(nameof(PauseVisibility));
                OnPropertyChanged(nameof(LongShortVisibility));
            }
        }

        public Visibility PauseVisibility { get; set; } = Visibility.Collapsed;

        private Visibility longShortVisibility { get; set; } = Visibility.Visible;

        public Visibility LongShortVisibility {
            get => longShortVisibility;
            set {
                longShortVisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility idleVisibility { get; set; } = Visibility.Collapsed;
        public Visibility IdleVisibility
        {
            get => idleVisibility;
            set
            {
                idleVisibility = value;
                OnPropertyChanged();
            }
        }
    }
}
