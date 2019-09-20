using EyesGuard.Logic;
using EyesGuard.MEF;
using System;

namespace EyesGuard
{
    public partial class App
    {
        public static void PauseProtection(TimeSpan pauseDuration)
        {
            if (CheckIfResting()) return;

            if (Configuration.SaveStats)
            {
                UpdateIntruptOfStats(GuardStates.PausedProtecting);
            }
            PauseProtectionSpan = pauseDuration;
            UpdatePauseTimeString();
            IsProtectionPaused = true;
            ITimerService t = GlobalMEFContainer.Instance.GetExport<ITimerService>();

            t.StartPauseHandler();

            CurrentMainPage.ProtectionState = GuardStates.PausedProtecting;
        }

        public static void ResumeProtection()
        {
            PauseProtectionSpan = TimeSpan.Zero;
            IsProtectionPaused = false;
            ITimerService t = GlobalMEFContainer.Instance.GetExport<ITimerService>();

            t.StopPauseHandler();

            CurrentMainPage.ProtectionState = GuardStates.Protecting;
        }
    }
}
