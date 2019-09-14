using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace EyesGuard.Logic
{
    public class Ticker : ObservableObject
    {
        public static TimeSpan ShortBreakVisibleTime { get; set; } = App.Configuration.ShortBreakDuration;
        public static DispatcherTimer ShortDurationCounter { get; set; } = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1) };

        public Ticker()
        {
            //ShortBreakVisibleTime = ShortBreakVisibleTime.Subtract(TimeSpan.FromSeconds(1));
           
            TimeRemaining = ((int)ShortBreakVisibleTime.TotalSeconds).ToString();

            

            ShortDurationCounter.Tick += ShortDurationCounter_Tick;

            ShortDurationCounter.Start();
        }

        private void ShortDurationCounter_Tick(object sender, EventArgs e)
        {
            ShortBreakVisibleTime = ShortBreakVisibleTime.Subtract(TimeSpan.FromSeconds(1));
            TimeRemaining = ((int)ShortBreakVisibleTime.TotalSeconds).ToString();
            if ((int)ShortBreakVisibleTime.TotalSeconds == 0)
            {
                ShortDurationCounter.Stop();
            }

        }

        public string TimeRemaining {
            get { return ""; }

            set { System.Console.Out.WriteLine(value); ; }
        }
    }
}
