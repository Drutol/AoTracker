using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AoLibs.Navigation.Android.Navigation;
using AoTracker.Infrastructure.ViewModels;

namespace AoTracker.Android.Fragments
{
    public abstract class CustomFragmentBase<T> : FragmentBase<T> where T : ViewModelBase
    {
        public override void NavigatedTo()
        {
            base.NavigatedTo();
            ViewModel.UpdatePageTitle();
        }
    }
}