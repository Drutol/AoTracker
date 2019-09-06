using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Domain.Enums;

namespace AoTracker.Infrastructure.ViewModels
{
    public class CrawlerResultViewModel : ViewModelBase
    {
        public override PageIndex PageIdentifier { get; } = PageIndex.CrawlerSets;
    }
}
