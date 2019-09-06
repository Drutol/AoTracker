using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoTracker.UWP.Models.Messages
{
    public class NavigationStackUpdateMessage
    {
        public NavigationStack NavigationStack { get; set; }
        public bool CanGoBack { get; set; }
        public bool NavigatedBackToEmpty { get; set; }
    }
}
