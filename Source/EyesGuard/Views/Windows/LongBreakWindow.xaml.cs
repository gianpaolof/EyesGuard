﻿using EyesGuard.MEF;
using EyesGuard.ViewModels.Interfaces;
using EyesGuard.Views.Animations;
using EyesGuard.Views.Windows.Interfaces;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Input;
using static EyesGuard.App;

namespace EyesGuard.Views.Windows
{
    /// <summary>
    /// Interaction logic for LongBreakWindow.xaml
    /// </summary>
    [Export(typeof(IContent))]
    [ExtensionMetadata(MetadataConstants.LongBreakWindow)]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class LongBreakWindow : BreakWindow, IBreakShellView, IContent
    {
        public LongBreakWindow()
        {
            InitializeComponent();
        }
        
        public bool LetItClose { get; set; } = false;

        private  void CloseLongBreak_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (App.Configuration.SaveStats)
                {
                    App.Configuration.LongBreaksFailed++;
                    App.Configuration.SaveSettingsToFile();
                }
                HideAnimation();
                App.ShortBreakShownOnce = false;
                if (App.Configuration.ProtectionState == GuardStates.Protecting)
                {
                    App.ShortBreakHandler.Start();
                    App.LongBreakHandler.Start();
                }
                LongDurationCounter.Stop();

                IHeaderMenuViewModel h = GlobalMEFContainer.Instance.GetExport<IHeaderMenuViewModel>();
                h.ManualBreakEnabled = true;
            }
            catch { }
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!LetItClose)
                e.Cancel = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.Configuration.ForceUserToBreak)
                base.Cursor = Cursors.None;
        }

        public  void ShowAnimation()
        {
            base.ShowWindow();
        }

        public void  HideAnimation()
        {
            LetItClose = true;
            base.HideWindow();
        }

        public void OnNavigatedFrom()
        {
            throw new System.NotImplementedException();
        }

        public void OnNavigatedTo()
        {
            throw new System.NotImplementedException();
        }
    }
}
