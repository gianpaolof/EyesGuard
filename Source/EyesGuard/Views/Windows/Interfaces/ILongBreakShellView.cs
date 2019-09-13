using System.Windows;

namespace EyesGuard.Views.Windows.Interfaces
{
    public interface ILongBreakShellView
    {
        void Show();

        void Close();

        void BringIntoView();

        bool Focus();

        object DataContext { get; set; }

        Window GetWindow();

        bool LetItClose { get; set; }
    }
}
