using EyesGuard.ViewModels.Interfaces;
using EyesGuard.Views.Windows.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyesGuard.ViewModels
{
    [Export(typeof(IMainWindowViewModel))]
    public class MainWindowViewModel : IMainWindowViewModel
    {
        private readonly IShellView view;

        [ImportingConstructor]
        public MainWindowViewModel(IShellView v)
        {
            view = v;
        }

        public void Show()
        {
            view.Show();
        }

        public IShellView GetShell()
        {
            return view;
        }
    }
}
