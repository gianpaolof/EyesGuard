namespace EyesGuard.ViewModels.Interfaces
{
    public interface IHeaderMenuViewModel
    {
        bool IsTimeItemChecked { get; set; }

         bool ManualBreakEnabled { get; set; }

         bool IsFeedbackAvailable { get; set; }
    }
}
