using EyesGuard.Logic;
using EyesGuard.ViewModels.Interfaces;
using System;
using System.ComponentModel.Composition;
using System.Windows;
using static EyesGuard.App;

namespace EyesGuard.ViewModels
{
    [Export(typeof(IShortLongBreakTimeRemainingViewModel))]
    public class ShortLongBreakTimeRemainingViewModel : ViewModelBase, IShortLongBreakTimeRemainingViewModel
    {
        ITimerService timer;
        [ImportingConstructor]
        public ShortLongBreakTimeRemainingViewModel(ITimerService t)
        {
           NextShortBreak = string.Empty; ;
           NextLongBreak = string.Empty; ;
           PauseTime = string.Empty; ;
           TimeRemainingVisibility = Visibility.Visible;
            timer = t;
            timer.ShortBreakStarted += Timer_ShortBreakStarted;
            timer.ShortBreakEnded += Timer_ShortBreakEnded;
        }

        private void Timer_ShortBreakEnded(object sender, EventArgs e)
        {
            NextShortBreak = LocalizedEnvironment.Translation.EyesGuard.Waiting;
        }

        private void Timer_ShortBreakStarted(object sender, EventArgs e)
        {
            NextShortBreak = LocalizedEnvironment.Translation.EyesGuard.Resting;
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
