using EyesGuard.Logic;
using EyesGuard.ViewModels.Interfaces;
using System.ComponentModel.Composition;

namespace EyesGuard.ViewModels
{
    [Export(typeof(IShortBreakViewModel))]
    public class ShortBreakViewModel : ViewModelBase, IShortBreakViewModel, IPartImportsSatisfiedNotification
    {
        public ShortBreakViewModel()
        {
            //Ticker t = new Ticker();
            ShortMessage = string.Empty; ;
            TimeRemaining = string.Empty; ;
        }

        [Import]
        private ITimerService Timer { get; set; }

        public string ShortMessage
        {
            get { return GetValue(() => ShortMessage); }
            set { SetValue(() => ShortMessage, value); }
        }

        public string TimeRemaining
        {
            get { return GetValue(() => TimeRemaining); }
            set { SetValue(() => TimeRemaining, value); }
        }

        public void OnImportsSatisfied()
        {
            Timer.ShortBreakStarted += Timer_ShortBreakStarted;
            Timer.ShortBreakTick += Timer_ShortBreakTick;
        }

        private void Timer_ShortBreakTick(object sender, System.EventArgs e)
        {
            TimeRemaining = ((int)App.ShortBreakVisibleTime.TotalSeconds).ToString();
        }

        private void Timer_ShortBreakStarted(object sender, System.EventArgs e)
        {
            TimeRemaining = ((int)App.ShortBreakVisibleTime.TotalSeconds).ToString();
            ShortMessage = App.GetShortWindowMessage();
        }
    }
}
