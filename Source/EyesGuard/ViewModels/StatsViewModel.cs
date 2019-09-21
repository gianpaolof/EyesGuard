using EyesGuard.Configurations;
using EyesGuard.Logic;
using EyesGuard.ViewModels.Interfaces;
using System;
using System.ComponentModel.Composition;

namespace EyesGuard.ViewModels
{
    [Export(typeof(IStatsViewModel))]
    public class StatsViewModel : ViewModelBase, IStatsViewModel,IPartImportsSatisfiedNotification
    {

        [Import]
        private ITimerService Timer { get; set; }

        public long ShortCount
        {
            get { return GetValue(() => ShortCount); }
            set { SetValue(() => ShortCount, value); }
        }

        public long LongCompletedCount
        {
            get { return GetValue(() => LongCompletedCount); }
            set { SetValue(() => LongCompletedCount, value); }
        }

        public long LongFailedCount
        {

            get { return GetValue(() => LongFailedCount); }
            set { SetValue(() => LongFailedCount, value); }
        }

        public long PauseCount
        {
            get { return GetValue(() => PauseCount); }
            set { SetValue(() => PauseCount, value); }
        }

        public long StopCount
        {
            get { return GetValue(() => PauseCount); }
            set { SetValue(() => PauseCount, value); }
        }

        public void OnImportsSatisfied()
        {
            Timer.ShortBreakEnded += Timer_ShortBreakEnded;
            Timer.LongBreakEnded += Timer_LongBreakEnded;
            Timer.Initialized += Timer_Initialized;
        }

        private void Timer_Initialized(object sender, EventArgs e)
        {
            UpdateStats();
        }

        private void Timer_LongBreakEnded(object sender, System.EventArgs e)
        {

            if (App.Configuration.SaveStats)
            {
                App.Configuration.LongBreaksCompleted++;
                UpdateStats();
            }
        }

        private void Timer_ShortBreakEnded(object sender, System.EventArgs e)
        {

            if (App.Configuration.SaveStats)
            {
                App.Configuration.ShortBreaksCompleted++;
                UpdateStats();
            }
        }

        public void UpdateStats()
        {
            App.Configuration.SaveSettingsToFile();
          
            ShortCount = App.Configuration.ShortBreaksCompleted;
            LongCompletedCount = App.Configuration.LongBreaksCompleted;
            LongFailedCount = App.Configuration.LongBreaksFailed;
            PauseCount = App.Configuration.PauseCount;
            StopCount = App.Configuration.StopCount;
        }
    }
}
