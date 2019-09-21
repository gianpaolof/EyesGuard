using EyesGuard.Logic;
using EyesGuard.ViewModels.Interfaces;
using System;
using System.ComponentModel.Composition;

namespace EyesGuard.ViewModels
{
    [Export(typeof(IHeaderMenuViewModel))]
    public class HeaderMenuViewModel : ViewModelBase, IHeaderMenuViewModel,IPartImportsSatisfiedNotification
    {
       
        public HeaderMenuViewModel()
        {
            IsTimeItemChecked = true;
            ManualBreakEnabled = true;
            IsFeedbackAvailable = true;
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
            ManualBreakEnabled = true;
        }

        private void Timer_LongBreakStarted(object sender, EventArgs e)
        {
            ManualBreakEnabled = false;
        }

        private void Timer_ShortBreakEnded(object sender, EventArgs e)
        {
            ManualBreakEnabled = true;
        }

        private void Timer_ShortBreakStarted(object sender, EventArgs e)
        {
            ManualBreakEnabled = false;
        }

        public bool IsTimeItemChecked
        {
            get { return GetValue(() => IsTimeItemChecked); }
            set { SetValue(() => IsTimeItemChecked, value); }
        }

        public bool ManualBreakEnabled
        {
            get { return GetValue(() => ManualBreakEnabled); }
            set { SetValue(() => ManualBreakEnabled, value); }
        }

        public bool IsFeedbackAvailable
        {
            get { return GetValue(() => IsFeedbackAvailable); }
            set { SetValue(() => IsFeedbackAvailable, value); }
        }

       
    }
}
