using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using EyesGuard.Views.Animations;

namespace EyesGuard.Views.Windows
{
    public class BreakWindow : Window
    {
 
        public async void HideWindow()
        {
            await (this as Window).HideUsingLinearAnimationAsync();
            base.Close();
        }

        public async void ShowWindow()
        {
            try
            {
                await (this as Window).ShowUsingLinearAnimationAsync();
                Show();
                BringIntoView();
                Focus();
            }
            catch { }

        }
    }
}
