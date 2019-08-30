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
using AoLibs.Adapters.Android.Interfaces;
using AoTracker.Interfaces.Adapters;

namespace AoTracker.Android.Adapters
{
    public class SnackbarProvider : ISnackbarProvider
    {
        private readonly IContextProvider _contextProvider;

        public SnackbarProvider(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
        }

        public void ShowToast(string text)
        {
            Toast.MakeText(_contextProvider.CurrentContext, text, ToastLength.Short);
        }
    }
}