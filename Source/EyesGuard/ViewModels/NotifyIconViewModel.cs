using EyesGuard.AppManagers;
using EyesGuard.Logic;
using EyesGuard.MEF;
using EyesGuard.ViewModels.Interfaces;
using EyesGuard.Views.Windows;
using FormatWith;
using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace EyesGuard.ViewModels
{
    [Export(typeof(INotifyIconViewModel))]
    public class NotifyIconViewModel : ViewModelBase, INotifyIconViewModel,IPartImportsSatisfiedNotification
    {

        private string _nextShortBreak;
        private string _nextLongBreak;
        private string _pauseRemaining;

        public NotifyIconViewModel()
        {
            Title = string.Empty;
            DarkBrush = Brushes.Green;
            LowBrush = Brushes.Green;
            PausedVisibility = Visibility.Collapsed;
            NextLongBreak = string.Empty;
            NextShortBreak = string.Empty;
            PauseRemaining = string.Empty;
            Pause = new RelayCommand<int>(time => App.PauseProtection(TimeSpan.FromMinutes(time)));
            Exit = new RelayCommand(() => Exit_Click());
            CustomPause = new RelayCommand(() => CustomPause_Click());
            StartProtect = new RelayCommand(() => StartProtect_Click());
            StopProtect = new RelayCommand(() => StopProtect_Click());
            Settings = new RelayCommand(() => Settings_Click());
        }

        public ICommand Pause { get; }
        public ICommand CustomPause { get; }
        public ICommand Exit { get; }
        public ICommand StartProtect { get; }
        public ICommand StopProtect { get; }
        public ICommand DoubleClick { get; }
        public ICommand Settings { get; }



        [Import]
        private ITimerService Timer { get; set; }

        public void OnImportsSatisfied()
        {
            Timer.ShortBreakEnded += Timer_ShortBreakEnded;
            Timer.ShortBreakStarted += Timer_ShortBreakStarted;
            Timer.LongBreakEnded += Timer_LongBreakEnded;
            Timer.LongBreakStarted += Timer_LongBreakStarted;
            Timer.LongBreakTick += Timer_LongBreakTick;
            Timer.ShortBreakTick += Timer_ShortBreakTick;
        }

        private void StartProtect_Click()
        {
            if (App.CheckIfResting()) return;

            if (App.Configuration.ProtectionState == App.GuardStates.PausedProtecting)
                App.ResumeProtection();
            else
                App.CurrentMainPage.ProtectionState = App.GuardStates.Protecting;
        }

        private void StopProtect_Click()
        {
            if (App.CheckIfResting()) return;

            if (App.Configuration.SaveStats) App.UpdateIntruptOfStats(App.GuardStates.NotProtecting);
            App.CurrentMainPage.ProtectionState = App.GuardStates.NotProtecting;
        }

        private void Settings_Click()
        {
            Utils.GetMainWindow().MainFrame.Content = GlobalMEFContainer.Instance.GetView(MetadataConstants.SettingsPage);

            if (!Utils.GetMainWindow().IsVisible)
                ChromeManager.Show();
        }

        private void CustomPause_Click()
        {
            Utils.GetMainWindow().MainFrame.Content = GlobalMEFContainer.Instance.GetView(MetadataConstants.CustomPause);

            if (!Utils.GetMainWindow().IsVisible)
                ChromeManager.Show();
        }

        private void Exit_Click()
        {
            App.Current.Shutdown();
        }
        private void Timer_ShortBreakTick(object sender, EventArgs e)
        {
            NextShortBreak = $"{Timer.NextShortBreak.Hours}:{Timer.NextShortBreak.Minutes}:{Timer.NextShortBreak.Seconds}";
        }

        private void Timer_LongBreakTick(object sender, EventArgs e)
        {
            NextLongBreak = $"{Timer.NextLongBreak.Hours}:{Timer.NextLongBreak.Minutes}:{Timer.NextLongBreak.Seconds}";
        }

        private void Timer_LongBreakStarted(object sender, System.EventArgs e)
        {
            NextShortBreak = App.LocalizedEnvironment.Translation.EyesGuard.Resting;
        }

        private void Timer_LongBreakEnded(object sender, System.EventArgs e)
        {
            NextShortBreak = App.LocalizedEnvironment.Translation.EyesGuard.Waiting;
        }

        private void Timer_ShortBreakStarted(object sender, System.EventArgs e)
        {
            NextShortBreak = App.LocalizedEnvironment.Translation.EyesGuard.Resting;
        }

        private void Timer_ShortBreakEnded(object sender, System.EventArgs e)
        {
            NextShortBreak = App.LocalizedEnvironment.Translation.EyesGuard.Waiting;
        }

        public SolidColorBrush DarkBrush {
            get { return GetValue(() => DarkBrush); }
            set { SetValue(() => DarkBrush, value); }
        }

        public SolidColorBrush LowBrush
        {
            get { return GetValue(() => LowBrush); }
            set { SetValue(() => LowBrush, value); }
        }

        public ImageSource Source
        {
            get { return GetValue(() => Source); }
            set { SetValue(() => Source, value); }
        }


        public Visibility StartProtectVisibility
        {
           get { return GetValue(() => StartProtectVisibility); }
           set { SetValue(() => StartProtectVisibility, value); }
        }

        public Visibility StopProtectVisibility
        {
            get { return GetValue(() => StopProtectVisibility); }
            set { SetValue(() => StopProtectVisibility, value); }
        }

        public string Title
        {
            get { return GetValue(() => Title); }
            set { SetValue(() => Title, value); }
        }

        public string NextShortBreak
        {
            get => _nextShortBreak;
            set
            {
                SetField(ref _nextShortBreak, value);
                OnPropertyChanged(nameof(NextShortBreakFullText));
            }
        }

        public string NextLongBreak
        {
            get => _nextLongBreak;
            set
            {
                SetField(ref _nextLongBreak, value);
                OnPropertyChanged(nameof(NextLongBreakFullText));
            }
        }

        public Visibility PausedVisibility
        {
            get { return GetValue(() => PausedVisibility); }
            set { SetValue(() => PausedVisibility, value); }
        }

        public string PauseRemaining
        {
            get => _pauseRemaining;
            set
            {
                SetField(ref _pauseRemaining, value);
                OnPropertyChanged(nameof(PauseRemainingFullText));
            }
        }

        public string PauseRemainingFullText =>
            App.LocalizedEnvironment.Translation.ShellExtensions.TaskbarIcon.Menu.PausedFor.FormatWith(new
            {
                PauseRemaining
            });

        public string NextShortBreakFullText =>
            App.LocalizedEnvironment.Translation.ShellExtensions.TaskbarIcon.Menu.NextShortBreak.FormatWith(new
            {
                NextShortBreak
            });

        public string NextLongBreakFullText =>
            App.LocalizedEnvironment.Translation.ShellExtensions.TaskbarIcon.Menu.NextLongBreak.FormatWith(new
            {
                NextLongBreak
            });

        public string TooltipTitle => App.LocalizedEnvironment.Translation.Application.HeaderTitle;

 
    }
}
