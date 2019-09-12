using System.Windows;

namespace EyesGuard.Views.Windows
{
    /// <summary>
    /// Interaction logic for ShortBreakWindow.xaml
    /// </summary>
    public partial class ShortBreakWindow : Window
    {
        public ShortBreakWindow()
        {
            App.CurrentShortBreakWindow = this;
            InitializeComponent();
        }

        public bool LetItClose { get; set; } = false;

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!LetItClose)
                e.Cancel = true;
        }
    }
}
