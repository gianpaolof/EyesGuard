using EyesGuard.AppManagers;
using EyesGuard.Logic;
using EyesGuard.MEF;
using EyesGuard.ViewModels.Interfaces;
using EyesGuard.Views.Animations;
using EyesGuard.Views.Pages;
using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Input;

namespace EyesGuard.ViewModels
{
    [Export(typeof(IHeaderMenuViewModel))]
    public class HeaderMenuViewModel : ViewModelBase, IHeaderMenuViewModel,IPartImportsSatisfiedNotification
    {
       
        public HeaderMenuViewModel()
        {
            var vc = GlobalMEFContainer.Instance;

            IsTimeItemChecked = true;
            ManualBreakEnabled = true;
            IsFeedbackAvailable = true;
            StartShortBreak = new RelayCommand(() => Timer.StartShortBreak());
            StartLongBreak = new RelayCommand(() => Timer.StartLongBreak());
            GoToPage = new RelayCommand<string>(page => Utils.GetMainWindow().MainFrame.Content = vc.GetView(page));
            Pause = new RelayCommand<int>(time=> App.PauseProtection(TimeSpan.FromMinutes(time)));
            Hide = new RelayCommand(() => ChromeManager.Hide());
            Exit = new RelayCommand(async () => await ExitAppAsync());
            Help = new RelayCommand(() => HelpClick());
            Resources = new RelayCommand(() => ResourceClick());
            Menu = new RelayCommand(() => MenuItem_Click());
            About = new RelayCommand(() => About_Click());
            ShowHideTimeRemaining = new RelayCommand(() => ShowHideTime());
        }

       
        public ICommand StartShortBreak { get; }
        public ICommand StartLongBreak { get; }
        public ICommand GoToPage { get; }
        public ICommand Pause { get; }
        public ICommand Hide { get; }
        public ICommand Exit { get; }
        public ICommand Help { get; }
        public ICommand Resources { get; }
        public ICommand About { get; }
        public ICommand Menu { get; }
        public ICommand ShowHideTimeRemaining { get; }

        [Import]
        private ITimerService Timer { get; set; }

        public void OnImportsSatisfied()
        {
            Timer.ShortBreakStarted += Timer_ShortBreakStarted;
            Timer.ShortBreakEnded += Timer_ShortBreakEnded;
            Timer.LongBreakStarted += Timer_LongBreakStarted;
            Timer.LongBreakEnded += Timer_LongBreakEnded;
            Timer.Initialized += Timer_Initialized;
        }


        public bool IsTimeItemChecked
        {
            get { return GetValue(() => IsTimeItemChecked); }
            set { SetValue(() => IsTimeItemChecked, value); }
        }

        public bool ManualBreakEnabled
        {
            get { return GetValue(() => ManualBreakEnabled); }
            set { SetValue(() => ManualBreakEnabled, value); }
        }

        public bool IsFeedbackAvailable
        {
            get { return GetValue(() => IsFeedbackAvailable); }
            set { SetValue(() => IsFeedbackAvailable, value); }
        }
        private void ShowHideTime()
        {
            App.Configuration.KeyTimesVisible = (App.Configuration.KeyTimesVisible) ? false : true;
            App.Configuration.SaveSettingsToFile();
        }

        private void HelpClick()
        {
            App.ShowWarning(App.LocalizedEnvironment.Translation.Application.HelpPageText,
                                     WarningPage.PageStates.Info);
        }
        private void ResourceClick()
        {
            var resourcesBase = App.LocalizedEnvironment.Translation.Application.Resources;
            App.ShowWarning(
                $"    - {resourcesBase.Content.Icons}\n    - {resourcesBase.Content.UIKit}"
                , WarningPage.PageStates.Info);
        }

        private void MenuItem_Click()
        {
            var aboutBase = App.LocalizedEnvironment.Translation.Application.About;
            App.ShowWarning(
                $"{aboutBase.Content.InnerTitle}\n\n   {aboutBase.Content.PublisherInfo}\n"
                , WarningPage.PageStates.About);
        }


        private void About_Click()
        {
            var aboutBase = App.LocalizedEnvironment.Translation.Application.About;
            App.ShowWarning(
                $"{aboutBase.Content.InnerTitle}\n\n   {aboutBase.Content.PublisherInfo}\n"
                , WarningPage.PageStates.About);
        }


        private void Timer_Initialized(object sender, EventArgs e)
        {
            if (App.Configuration.KeyTimesVisible) 
            {
               
                IsTimeItemChecked = true;
                return;
            }
            
            IsTimeItemChecked = false;
        }

        private void Timer_LongBreakEnded(object sender, EventArgs e)
        {
            ManualBreakEnabled = true;
        }

        private void Timer_LongBreakStarted(object sender, EventArgs e)
        {
            ManualBreakEnabled = false;
        }

        private void Timer_ShortBreakEnded(object sender, EventArgs e)
        {
            ManualBreakEnabled = true;
        }

        private void Timer_ShortBreakStarted(object sender, EventArgs e)
        {
            ManualBreakEnabled = false;
        }

        private async System.Threading.Tasks.Task ExitAppAsync()
        {
            await Utils.GetMainWindow().HideUsingLinearAnimationAsync();
            App.Current.Shutdown();
        }
    }
}
