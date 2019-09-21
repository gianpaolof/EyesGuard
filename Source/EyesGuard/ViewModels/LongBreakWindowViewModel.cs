using EyesGuard.Logic;
using EyesGuard.ViewModels.Interfaces;
using System.ComponentModel.Composition;
using System.Windows;
using static EyesGuard.App;
using FormatWith;
namespace EyesGuard.ViewModels
{
    [Export(typeof(ILongBreakViewModel))]
    public class LongBreakWindowViewModel : ViewModelBase, ILongBreakViewModel,IPartImportsSatisfiedNotification
    {
        public LongBreakWindowViewModel()
        {
            TimeRemaining = string.Empty;
            CanCancel = Visibility.Visible;
        }
        [Import]
        private ITimerService Timer { get; set; }
        public void OnImportsSatisfied()
        {
            Timer.LongBreakStarted += Timer_LongBreakStarted; 
            Timer.LongBreakTick += Timer_LongBreakTick;
        }

        private void Timer_LongBreakTick(object sender, System.EventArgs e)
        {
            TimeRemaining = App.LocalizedEnvironment.Translation.EyesGuard.LongBreakTimeRemaining.FormatWith(new
            {
                App.LongBreakVisibleTime.Hours,
                App.LongBreakVisibleTime.Minutes,
                App.LongBreakVisibleTime.Seconds
            });
        }

        private void Timer_LongBreakStarted(object sender, System.EventArgs e)
        {
            TimeRemaining = App.LocalizedEnvironment.Translation.EyesGuard.LongBreakTimeRemaining.FormatWith(new
            {
                App.LongBreakVisibleTime.Hours,
                App.LongBreakVisibleTime.Minutes,
                App.LongBreakVisibleTime.Seconds
            });
            CanCancel = (App.Configuration.ForceUserToBreak) ? Visibility.Collapsed : Visibility.Visible;

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
