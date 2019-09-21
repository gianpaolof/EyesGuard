using EyesGuard.Views.Animations;
using System.Windows;

namespace EyesGuard.Views.Windows
{
    public class BreakWindow : Window
    {
 
        public async void HideWindow() => await (this as Window).HideUsingLinearAnimationAsync();

        public async void ShowWindow()
        {
                await (this as Window).ShowUsingLinearAnimationAsync();
                Show();
                BringIntoView();
                Focus();
        }
    }
}
