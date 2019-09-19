using System.Windows;

namespace EyesGuard.Views.Windows.Interfaces
{
    public interface IBreakShellView
    {
        void ShowAnimation();

        void HideAnimation();


        object DataContext { get; set; }


        bool LetItClose { get; set; }
    }
}
