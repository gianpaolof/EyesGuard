using System.Windows.Input;

namespace EyesGuard.ViewModels.Interfaces
{
    public interface IHeaderMenuViewModel
    {
        bool IsTimeItemChecked { get; set; }
        bool ManualBreakEnabled { get; set; }
        bool IsFeedbackAvailable { get; set; }
        ICommand StartShortBreak { get;}
        ICommand StartLongBreak { get; }
        ICommand GoToPage { get; }
        ICommand Pause { get; }
        ICommand Hide { get; }
        ICommand Exit { get; }
        ICommand Help { get; }
        ICommand Resources { get; }
        ICommand About { get; }
        ICommand Menu { get; }
        ICommand ShowHideTimeRemaining { get; }
    }
}
