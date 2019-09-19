namespace EyesGuard.Logic
{
    public interface ITimerService
    {
        void StartService();
        void Init();

        void DoShortBreak();

        void DoLongBreak();

    }
}
