using EyesGuard.Logic;
using EyesGuard.MEF;
using EyesGuard.ViewModels.Interfaces;
using System.ComponentModel.Composition;

namespace EyesGuard.ViewModels
{
    [Export(typeof(IShortBreakViewModel))]
    public class ShortBreakViewModel : ViewModelBase, IShortBreakViewModel
    {
        private ITimerService timer;

        [ImportingConstructor]
        public ShortBreakViewModel(ITimerService t)
        {
            //Ticker t = new Ticker();
            ShortMessage = string.Empty;
            TimeRemaining = string.Empty;
            timer = t;
            timer.ShortBreakStarted += Timer_ShortBreakStarted;
            timer.ShortBreakTick += Timer_ShortBreakTick;
        }

        private void Timer_ShortBreakTick(object sender, System.EventArgs e)
        {
            TimeRemaining = ((int)App.ShortBreakVisibleTime.TotalSeconds).ToString();
        }

        private void Timer_ShortBreakStarted(object sender, System.EventArgs e)
        {

            //update
            TimeRemaining = ((int)App.Configuration.ShortBreakDuration.TotalSeconds).ToString();
            ShortMessage = App.GetShortWindowMessage();

        }

        public void OnImportsSatisfied()
        {
            throw new System.NotImplementedException();
        }

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
    }
}
