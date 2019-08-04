using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Domain.Messaging;

namespace AoTracker.Infrastructure.ViewModels
{
    public abstract class ViewModelBase : GalaSoft.MvvmLight.ViewModelBase
    {
        private string _title;

        public string Title
        {
            get => _title;
            set
            {
                Set(ref _title, value);
                MessengerInstance.Send(new PageTitleMessage(value));
            }
        }

        public void UpdatePageTitle()
        {
            MessengerInstance.Send(new PageTitleMessage(Title));
        }

        protected bool Set<T>(ref T backingStore, T value, Action<T> onChanged)
        {
            var result = Set(ref backingStore, value);
            if(result)
                onChanged?.Invoke(value);
            return result;
        }
    }
}
