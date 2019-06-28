using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Infrastructure.ViewModels;
using Xamarin.Forms;

namespace AoTracker.Util
{
    public abstract class ContentPage<T> : ContentPage where T : ViewModelBase
    {
        protected T ViewModel { get; }

        public ContentPage()
        {
            ViewModel = DependencyService.Resolve<T>();
        }
    }
}
