using System.Windows;
using System.Windows.Media;

namespace EyesGuard.ViewModels.Interfaces
{
    public interface INotifyIconViewModel
    {
         SolidColorBrush DarkBrush { get; set; }
        

         SolidColorBrush LowBrush { get; set; }


        ImageSource Source { get; set; }


        Visibility StartProtectVisibility { get; set; }

        Visibility StopProtectVisibility { get; set; }

        string Title { get; set; }


        string NextShortBreak { get; set; }


        string NextLongBreak { get; set; }

        Visibility PausedVisibility { get; set; }

        string PauseRemaining { get; set; }


    }
}
