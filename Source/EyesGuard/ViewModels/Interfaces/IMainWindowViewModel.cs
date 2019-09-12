using EyesGuard.Views.Windows.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyesGuard.ViewModels.Interfaces
{
    public interface IMainWindowViewModel
    {
        void Show();
        IShellView GetShell();
    }
}
