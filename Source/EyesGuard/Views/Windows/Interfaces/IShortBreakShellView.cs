using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EyesGuard.Views.Windows.Interfaces
{
    public interface IShortBreakShellView
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
