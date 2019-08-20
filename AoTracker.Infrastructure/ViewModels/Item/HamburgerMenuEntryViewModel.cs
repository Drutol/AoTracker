using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Domain.Enums;
using AoTracker.Infrastructure.ViewModels;

namespace AoTracker.Infrastructure.Models
{
    public class HamburgerMenuEntryViewModel : ViewModelBase
    {
        private bool _isSelected;

        public string Title { get; set; }
        public PageIndex Page { get; set; }

        public bool IsSelected
        {
            get => _isSelected;
            set => Set(ref _isSelected, value);
        }
    }
}
