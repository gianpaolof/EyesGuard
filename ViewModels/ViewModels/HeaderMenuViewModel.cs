using EyesGuard.Logic;
using EyesGuard.ViewModels.Interfaces;
using System;
using System.ComponentModel.Composition;

namespace EyesGuard.ViewModels
{
    [Export(typeof(IHeaderMenuViewModel))]
    public class HeaderMenuViewModel : ViewModelBase, IHeaderMenuViewModel
    {
        ITimerService timer;
        [ImportingConstructor]
        public HeaderMenuViewModel(ITimerService t)
        {
            IsTimeItemChecked = true;
            ManualBreakEnabled = true;
            IsFeedbackAvailable = true;
            timer = t;
            timer.ShortBreakStarted += Timer_ShortBreakStarted;
            timer.ShortBreakEnded += Timer_ShortBreakEnded;
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
