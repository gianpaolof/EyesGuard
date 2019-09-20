namespace EyesGuard.Logic
{
    public interface ITimerService
    {
        void StartService();

        void StopService();

        void Init();

        void DoShortBreak();

        void DoLongBreak();

        void StartShortHandler();

        void StartLongHandler();

        void StartPauseHandler();

        void StopPauseHandler();

    }
}
