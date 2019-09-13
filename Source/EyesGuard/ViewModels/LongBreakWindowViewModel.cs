using EyesGuard.ViewModels.Interfaces;
using System.ComponentModel.Composition;
using System.Windows;

namespace EyesGuard.ViewModels
{
    [Export(typeof(ILongBreakViewModel))]
    public class LongBreakWindowViewModel : ViewModelBase, ILongBreakViewModel
    {
        public LongBreakWindowViewModel()
        {
            TimeRemaining = string.Empty;
            CanCancel = Visibility.Visible;
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
