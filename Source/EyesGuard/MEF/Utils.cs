using EyesGuard.ViewModels.Interfaces;
using EyesGuard.Views.Windows;

namespace EyesGuard.MEF
{ 
    public static class Utils
    {
        public static MainWindow GetMainWindow()
        {
            IMainWindowViewModel vm = GlobalMEFContainer.Instance.GetExport<IMainWindowViewModel>();
            return vm.GetShell().GetMainWindow();
        }
    }
}
