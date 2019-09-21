using EyesGuard.ViewModels.Interfaces;
using System.ComponentModel.Composition;

namespace EyesGuard.ViewModels
{
    [Export(typeof(IStatsViewModel))]
    public class StatsViewModel : ViewModelBase, IStatsViewModel
    {
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
    }
}
