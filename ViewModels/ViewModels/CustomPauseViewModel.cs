using EyesGuard.Logic;
using EyesGuard.MEF;
using EyesGuard.Views;
using EyesGuard.Views.Pages;
using EyesGuard.Views.Windows;
using System;
using System.ComponentModel.Composition;
using System.Text;
using System.Windows.Input;

namespace EyesGuard.ViewModels
{
    [Export]
    public class CustomPauseViewModel : ViewModelBase
    {
        ITimerService timer;
        private  string hoursError = string.Empty; 
        private  string minutsError = string.Empty;
        private  string secondsError = string.Empty;
        private  string chooseLargerTime = string.Empty;

        [ImportingConstructor]
        public CustomPauseViewModel(ITimerService t)
        {
 
            SecondsCustomPause = "0";
            MinutesCustomPause = "15";
            HoursCustomPause = "1";
            PauseProtection = new RelayCommand(() => DoCustomPause());
            InitTexts();
            timer = t;
            timer.ShortBreakStarted += Timer_ShortBreakStarted;
        }

        private void Timer_ShortBreakStarted(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public ICommand PauseProtection { get; }

        public string MinutesCustomPause
        {
            get { return GetValue(() => MinutesCustomPause); }
            set { SetValue(() => MinutesCustomPause, value); }
        }

        public string SecondsCustomPause
        {
            get { return GetValue(() => SecondsCustomPause); }
            set { SetValue(() => SecondsCustomPause, value); }
        }

        public string HoursCustomPause
        {
            get { return GetValue(() => HoursCustomPause); }
            set { SetValue(() => HoursCustomPause, value); }
        }


        private void DoCustomPause()
        {

             if (ParseInput(out int hours, out int minutes, out int seconds) == false)
                {
                    App.ShowWarning(
                    $"{App.LocalizedEnvironment.Translation.EyesGuard.OperationFailed}.\n{App.LocalizedEnvironment.Translation.EyesGuard.CheckInput}.");
                    return;
                }

             string warning = ValidateInput(hours, minutes, seconds);

             if (warning?.Length > 0)
                {
                    App.ShowWarning(warning);
                    return;                
                }

             App.PauseProtection(new TimeSpan(hours, minutes, seconds));

            var vc = GlobalMEFContainer.Instance.ViewContentLoader;

            Utils.GetMainWindow().MainFrame.Content = vc.GetView(MetadataConstants.MainPage);
            return;

        }

        private bool ParseInput(out int hours, out int minutes, out int seconds)
        {
            bool ret1 = int.TryParse(HoursCustomPause, out hours);
            bool ret2 = int.TryParse(MinutesCustomPause, out minutes);
            bool ret3 = int.TryParse(SecondsCustomPause, out seconds);

            return ret1 && ret2 && ret3;
        }

        
        private string ValidateInput(int hours, int minutes, int seconds)
        {
            StringBuilder warning = new StringBuilder(string.Empty);
           

            if (hours > 11)
            {
                if (warning.Length > 0) warning.Append(Environment.NewLine);
                warning.Append(hoursError);
            }

            if (minutes > 59)
            {
                if (warning.Length > 0) warning.Append(Environment.NewLine);
                warning.Append(minutsError);

            }

            if (seconds > 59)
            {
                if (warning.Length > 0) warning.Append(Environment.NewLine);
                warning.Append(secondsError);
            }

            if (new TimeSpan(hours, minutes, seconds).TotalSeconds < 5)
            {
                if (warning.Length > 0) warning.Append(Environment.NewLine);
                warning.Append(chooseLargerTime);
            }

            return warning.ToString();
        }

        /// <summary>
        /// This is needed because the LocalizedEnvironment is loaded at run time, it's not static like resx
        /// </summary>
        private void InitTexts()
        {
            hoursError = string.Format("» " + App.LocalizedEnvironment.Translation.EyesGuard.TimeManipulation.HoursLimit, 11);
            minutsError = string.Format("» " + App.LocalizedEnvironment.Translation.EyesGuard.TimeManipulation.MinutesLimit, 59);
            secondsError = string.Format("» " + App.LocalizedEnvironment.Translation.EyesGuard.TimeManipulation.SecondsLimit, 59);
            chooseLargerTime = string.Format("» " + App.LocalizedEnvironment.Translation.EyesGuard.TimeManipulation.ChooseLargerTime);

        }
    }
}
