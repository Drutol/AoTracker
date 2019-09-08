using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoTracker.UWP.Behaviours.Interfaces
{
    public interface IElementWithNestedBindings : INotifyPropertyChanged
    {
        string NestedPropertyName { get; }
        void UpdateBindings();
    }
}
