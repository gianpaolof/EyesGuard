using System;

namespace EyesGuard.Logic
{
    public interface ITimerService
    {
        void Init();
        void StartService();

        void StopService();

        void StartShortBreak();

        void StartLongBreak();

        void StartShortHandler();

        void StartLongHandler();

        void StartPauseHandler();

        void StopPauseHandler();

        event EventHandler Initialized;

        event EventHandler ShortBreakStarted;
        event EventHandler ShortBreakEnded;
        event EventHandler ShortDurationTick;

        event EventHandler LongBreakStarted;
        event EventHandler LongBreakEnded;
        event EventHandler LongDurationTick;

        event EventHandler LongBreakTick;
        event EventHandler ShortBreakTick;

    }
}
