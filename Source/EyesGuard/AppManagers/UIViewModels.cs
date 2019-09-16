using EyesGuard.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyesGuard.AppManagers
{
    public class UIViewModels
    {
        public HeaderMenuViewModel HeaderMenu { get; set; } = new HeaderMenuViewModel();

        public NotifyIconViewModel NotifyIcon { get; set; } = new NotifyIconViewModel();
    }
}
