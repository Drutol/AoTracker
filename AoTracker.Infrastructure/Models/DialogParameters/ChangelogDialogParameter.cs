using System;
using System.Collections.Generic;
using System.Text;

namespace AoTracker.Infrastructure.Models.DialogParameters
{
    public class ChangelogDialogParameter
    {
        public string Date { get; set; }
        public string Note { get; set; }
        public List<string> Changelog { get; set; }
    }
}
