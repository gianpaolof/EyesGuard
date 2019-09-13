using System.Windows;

namespace EyesGuard.ViewModels.Interfaces
{
    public interface ILongBreakViewModel
    {
        string TimeRemaining { get; set; }

        Visibility CanCancel { get; set; }
    }
}
