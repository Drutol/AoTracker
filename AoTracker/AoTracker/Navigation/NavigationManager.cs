using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AoTracker.Domain.Enums;
using AoTracker.Interfaces;
using AoTracker.Views;
using Xamarin.Forms;

namespace AoTracker.Navigation
{
    public class NavigationManager : INavigationManager
    {
        private readonly INavigation _navigation;

        public NavigationManager(INavigation navigation)
        {
            _navigation = navigation;
        }

        private readonly Dictionary<PageIndex, Func<object, Page>> _pages = new Dictionary<PageIndex, Func<object, Page>>
        {
            {PageIndex.Welcome, o => new WelcomePage()},
            {PageIndex.FeedPage, o => new FeedPage()},
            {PageIndex.SetsPage, o => new CrawlerSetsPage()},
        };

        public void NavigateRoot(PageIndex page, object parameter = null)
        {
            var newPage = _pages[page](parameter);
            _navigation.PushAsync(newPage);
            foreach (var p in _navigation.NavigationStack.ToList())
            {
                if(newPage != p)
                    _navigation.RemovePage(p);
            }
        }

        public void PushPage(PageIndex navigationPageIndex, object parameter = null)
        {
            throw new NotImplementedException();
        }
    }
}
