using EyesGuard.MEF;
using EyesGuard.ViewModels.Interfaces;
using EyesGuard.Views;
using EyesGuard.Views.Animations;
using EyesGuard.Views.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyesGuard.AppManagers
{
    public static class ChromeManager
    {
        private static bool _hiding = false;

        public async static void Hide()
        {
            if (!_hiding)
            {
                IMainWindowViewModel vm = GlobalMEFContainer.Instance.GetExport<IMainWindowViewModel>();
                MainWindow v = vm.GetShell().GetMainWindow();
                _hiding = true;
                await v.HideUsingLinearAnimationAsync();
                v.Hide();
                _hiding = false;
            }
        }

        private static bool _closing = false;

        public async static void Close()
        {
            if (!_closing)
            {
                _closing = true;
                IMainWindowViewModel vm = GlobalMEFContainer.Instance.GetExport<IMainWindowViewModel>();
                MainWindow v = vm.GetShell().GetMainWindow();
                await v.HideUsingLinearAnimationAsync(200);
                v.Close();
                _closing = false;
            }
        }

        private static bool _showing = false;

        public async static void Show()
        {
            if (!_showing)
            {
                _showing = true;
                IMainWindowViewModel vm = GlobalMEFContainer.Instance.GetExport<IMainWindowViewModel>();
                MainWindow v = vm.GetShell().GetMainWindow();
                v.Show();
                await v.ShowUsingLinearAnimationAsync();
                v.Show();
                v.BringIntoView();
                _showing = false;
            }
        }
    }
}
