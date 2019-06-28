using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Domain.Enums;
using AoTracker.Interfaces;
using AoTracker.Views;
using Xamarin.Forms;

namespace AoTracker.Navigation
{
    public class OuterNavigationManager : IOuterNavigationManager
    {
        private Dictionary<OuterNavigationPage, Type> _pageMapping = new Dictionary<OuterNavigationPage, Type>
        {
            {
                OuterNavigationPage.Welcome,
                typeof(WelcomePage)
            },
            {
                OuterNavigationPage.Shell,
                typeof(AppShell)
            }
        };

        public void NavigateTo(OuterNavigationPage page)
        {
            App.Instance.MainPage = new NavigationPage((Page) Activator.CreateInstance(_pageMapping[page]));
        }
    }
}
