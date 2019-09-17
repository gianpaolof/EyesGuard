using EyesGuard.Views.Windows.Interfaces;
using System.ComponentModel.Composition;
using System.Windows;

namespace EyesGuard.Views.Windows
{
    /// <summary>
    /// Interaction logic for ShortBreakWindow.xaml
    /// </summary>
    [Export(typeof(IShortBreakShellView)), PartCreationPolicy(CreationPolicy.Shared)]
    public partial class ShortBreakWindow : Window, IShortBreakShellView
    {
        public ShortBreakWindow()
        {
            InitializeComponent();


        }

        public bool LetItClose { get; set; } = false;

        public Window GetWindow()
        {
            return this;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!LetItClose)
                e.Cancel = true;
        }
    }
}
