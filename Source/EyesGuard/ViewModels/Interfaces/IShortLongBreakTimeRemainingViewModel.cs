using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EyesGuard.ViewModels.Interfaces
{
    public interface IShortLongBreakTimeRemainingViewModel
    {
        string NextShortBreak { get; set;}
       


         string NextLongBreak { get; set; }



        string PauseTime { get; set; }


        Visibility TimeRemainingVisibility { get; set; }

         bool IsProtectionPaused { get; set; }


         Visibility PauseVisibility { get; set; } 

 
         Visibility LongShortVisibility { get; set; }




         Visibility IdleVisibility { get; set; }

    }
}
