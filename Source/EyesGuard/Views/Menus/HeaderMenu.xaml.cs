using EyesGuard.AppManagers;
using EyesGuard.Logic;
using EyesGuard.MEF;
using EyesGuard.ViewModels.Interfaces;
using EyesGuard.Views.Animations;
using EyesGuard.Views.Pages;
using EyesGuard.Views.Windows;
using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;

namespace EyesGuard.Views.Menus
{
    [Export(typeof(IContent))]
    [ExtensionMetadata(MetadataConstants.xx)]
    public partial class HeaderMenu : UserControl, IContent
    {
        public HeaderMenu()
        {
            InitializeComponent();
            IHeaderMenuViewModel h = GlobalMEFContainer.Instance.GetExport<IHeaderMenuViewModel>();
            DataContext = h;
        }

        public void OnNavigatedFrom()
        {
            throw new NotImplementedException();
        }

        public void OnNavigatedTo()
        {
            throw new NotImplementedException();
        }
    }
}
