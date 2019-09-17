namespace EyesGuard.Logic
{
    public interface ITimerService
    {
        void Start();
        void Init();

        void DoShortBreak();

        void DoLongBreak();

    }
}
