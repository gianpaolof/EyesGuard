using EyesGuard.Logic;
using EyesGuard.ViewModels.Interfaces;
using System;
using System.ComponentModel.Composition;
using System.Windows;

namespace EyesGuard.ViewModels
{
    [Export(typeof(ILongBreakViewModel))]
    public class LongBreakWindowViewModel : ViewModelBase, ILongBreakViewModel
    {
        ITimerService timer;
        [ImportingConstructor]
        public LongBreakWindowViewModel(ITimerService t)
        {
            TimeRemaining = string.Empty;
            CanCancel = Visibility.Visible;
            timer = t;
            timer.ShortBreakStarted += Timer_ShortBreakStarted;
        }

        private void Timer_ShortBreakStarted(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public string TimeRemaining
        {
            get { return GetValue(() => TimeRemaining); }
            set { SetValue(() => TimeRemaining, value); }
        }

        public Visibility CanCancel
        {
            get { return GetValue(() => CanCancel); }
            set { SetValue(() => CanCancel, value); }

        }
    }
}
