using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoLibs.Navigation.UWP.Pages;
using AoTracker.Infrastructure.ViewModels;

namespace AoTracker.UWP.Utils
{
    public class CustomPageBase<T> : PageBase<T> where T : ViewModelBase
    {
        public override void NavigatedTo()
        {
            base.NavigatedTo();
            ViewModel.UpdatePageTitle();
        }

        public override void NavigatedBack()
        {
            base.NavigatedBack();
            ViewModel.UpdatePageTitle();
        }
    }
}
