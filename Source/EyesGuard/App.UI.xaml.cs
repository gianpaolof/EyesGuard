using EyesGuard.MEF;
using EyesGuard.Views.Pages;
using System.Windows.Controls;

namespace EyesGuard
{
    using static EyesGuard.Views.Pages.WarningPage;

    public partial class App
    {
        public static void ShowWarning(
            string message,
            PageStates state = PageStates.Warning,
            Page navPage = null,
            string pageTitle = "")
        {
            try
            {
                navPage = navPage ?? (Page)Utils.GetMainWindow().MainFrame.Content;

                Utils.GetMainWindow().MainFrame.Navigate(new WarningPage(navPage, message, state, pageTitle));
            }
            catch { }
        }

        public static string GetShortWindowMessage()
        {
            var messagesBase = (Configuration.UseLanguageProvedidShortMessages) ?
                LocalizedEnvironment.Translation.EyesGuard.ShortMessageSuggestions :
                Configuration.CustomShortMessages;

            ShortMessageIteration++;
            ShortMessageIteration %= messagesBase.Count;

            return messagesBase[ShortMessageIteration];
        }
    }
}
