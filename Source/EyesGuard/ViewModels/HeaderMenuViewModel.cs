using EyesGuard.ViewModels.Interfaces;
using System.ComponentModel.Composition;

namespace EyesGuard.ViewModels
{
    [Export(typeof(IHeaderMenuViewModel))]
    public class HeaderMenuViewModel : ViewModelBase, IHeaderMenuViewModel
    {
       
        public HeaderMenuViewModel()
        {
            IsTimeItemChecked = true;
            ManualBreakEnabled = true;
            IsFeedbackAvailable = true;
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
