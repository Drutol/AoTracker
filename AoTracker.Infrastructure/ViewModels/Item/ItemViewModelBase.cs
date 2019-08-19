using System;
using System.Collections.Generic;
using System.Text;

namespace AoTracker.Infrastructure.ViewModels.Item
{
    public abstract class ItemViewModelBase<T> : ViewModelBase where T : class
    {
        private T _backingModel;

        public T BackingModel
        {
            get => _backingModel;
            set => Set(ref _backingModel, value);
        }

        public ItemViewModelBase(T item)
        {
            BackingModel = item;
        }

        public override void UpdatePageTitle()
        {
            //don't update on child view models
        }
    }
}
