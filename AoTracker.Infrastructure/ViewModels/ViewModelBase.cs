using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using AoTracker.Domain.Enums;
using AoTracker.Domain.Messaging;

namespace AoTracker.Infrastructure.ViewModels
{
    public abstract class ViewModelBase : GalaSoft.MvvmLight.ViewModelBase
    {
        private string _pageTitle;

        public abstract PageIndex  PageIdentifier { get; }

        public string PageTitle
        {
            get => _pageTitle;
            set
            {
                _pageTitle = value;
                MessengerInstance.Send(new PageTitleMessage(PageIdentifier, value));
            }
        }

        public virtual void UpdatePageTitle()
        {
            MessengerInstance.Send(new PageTitleMessage(PageIdentifier, PageTitle));
        }

        protected new bool Set<T>(ref T backingStore, T value, bool alwaysRaise = true, [CallerMemberName] string propertyName = null)
        {
            var result = base.Set(ref backingStore, value);
            if(alwaysRaise)
                RaisePropertyChanged(propertyName);
            return result;
        }
    }
}
