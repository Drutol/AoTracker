using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Infrastructure.ViewModels;
using Xamarin.Forms;

namespace AoTracker.Util
{
    public abstract class ViewModelContentPage<T> : ContentPage where T : ViewModelBase
    {
        protected T ViewModel { get; }

        public ViewModelContentPage()
        {
            ViewModel = DependencyService.Resolve<T>();
        }
    }
}
