using System;
using System.Collections.Generic;
using System.Text;

namespace AoTracker.Interfaces.Adapters
{
    public interface ISnackbarProvider
    {
        void ShowToast(string text);
    }
}
