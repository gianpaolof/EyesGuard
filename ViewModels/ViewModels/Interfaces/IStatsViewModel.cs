using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyesGuard.ViewModels.Interfaces
{
    public interface IStatsViewModel
    {
        long ShortCount { get; set; }


        long LongCompletedCount { get; set; }

        long LongFailedCount { get; set; }


        long PauseCount { get; set; }


        long StopCount { get; set; }

    }
}
