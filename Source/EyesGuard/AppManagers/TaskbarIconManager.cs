using EyesGuard.Extensions;
using EyesGuard.MEF;
using EyesGuard.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static EyesGuard.App;

namespace EyesGuard.AppManagers
{
    public static class TaskbarIconManager
    {
        public static void UpdateTaskbarIcon()
        {
            INotifyIconViewModel ic = GlobalMEFContainer.Instance.GetExport<INotifyIconViewModel>();

            if (Configuration.ProtectionState == GuardStates.Protecting)
            {
                ic.DarkBrush = "EyesGuard.SolidColorBrushes.TaskbarIcon.Protecting.DarkBrush".Translate<SolidColorBrush>();
                ic.LowBrush = "EyesGuard.SolidColorBrushes.TaskbarIcon.Protecting.LowBrush".Translate<SolidColorBrush>();
                ic.Source = "Images.Bitmap.EyesGuard.TaskbarIcon.Protecting".Translate<BitmapImage>();
                ic.StartProtectVisibility = Visibility.Collapsed;
                ic.StopProtectVisibility = Visibility.Visible;
                ic.Title = LocalizedEnvironment.Translation.ShellExtensions.TaskbarIcon.Protected;
            }
            else if (Configuration.ProtectionState == GuardStates.PausedProtecting)
            {
                ic.DarkBrush = "EyesGuard.SolidColorBrushes.TaskbarIcon.PausedProtecting.DarkBrush".Translate<SolidColorBrush>();
                ic.LowBrush = "EyesGuard.SolidColorBrushes.TaskbarIcon.PausedProtecting.LowBrush".Translate<SolidColorBrush>();
                ic.Source = "Images.Bitmap.EyesGuard.TaskbarIcon.PausedProtecting".Translate<BitmapImage>();
                ic.StartProtectVisibility = Visibility.Visible;
                ic.StopProtectVisibility = Visibility.Collapsed;
                ic.Title = LocalizedEnvironment.Translation.ShellExtensions.TaskbarIcon.PausedProtected;
            }
            else if (Configuration.ProtectionState == GuardStates.NotProtecting)
            {
                ic.DarkBrush = "EyesGuard.SolidColorBrushes.TaskbarIcon.NotProtecting.DarkBrush".Translate<SolidColorBrush>();
                ic.LowBrush = "EyesGuard.SolidColorBrushes.TaskbarIcon.NotProtecting.LowBrush".Translate<SolidColorBrush>();
                ic.Source = "Images.Bitmap.EyesGuard.TaskbarIcon.NotProtecting".Translate<BitmapImage>();
                ic.StartProtectVisibility = Visibility.Visible;
                ic.StopProtectVisibility = Visibility.Collapsed;
                ic.Title = LocalizedEnvironment.Translation.ShellExtensions.TaskbarIcon.NotProtected;
            }
        }
    }
}
