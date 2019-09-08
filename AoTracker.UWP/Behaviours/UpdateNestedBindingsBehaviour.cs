using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoTracker.UWP.Behaviours.Interfaces;
using Microsoft.Xaml.Interactivity;

namespace AoTracker.UWP.Behaviours
{
    class UpdateNestedBindingsBehaviour : Behavior
    {
        private IElementWithNestedBindings _element;

        protected override void OnAttached()
        {
            if (AssociatedObject is IElementWithNestedBindings element)
            {
                _element = element;
                _element.PropertyChanged += DataContextOnPropertyChanged;
            }
        }

        protected override void OnDetaching()
        {
            if (_element != null)
            {
                _element.PropertyChanged -= DataContextOnPropertyChanged;
            }
        }

        private void DataContextOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == _element.NestedPropertyName)
            {
                _element.UpdateBindings();
            }
        }
    }
}
