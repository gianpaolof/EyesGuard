using EyesGuard.Logic;
using EyesGuard.ViewModels.Interfaces;
using System.ComponentModel.Composition;

namespace EyesGuard.ViewModels
{
    [Export(typeof(IShortBreakViewModel))]
    public class ShortBreakViewModel : ViewModelBase, IShortBreakViewModel
    {
        public ShortBreakViewModel()
        {
            //Ticker t = new Ticker();
            ShortMessage = string.Empty; ;
            TimeRemaining = string.Empty; ;
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
