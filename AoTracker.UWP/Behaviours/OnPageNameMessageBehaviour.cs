using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using AoTracker.Domain.Messaging;
using AoTracker.UWP.Models;
using AoTracker.UWP.Utils;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Xaml.Interactivity;

namespace AoTracker.UWP.Behaviours
{
    public class OnPageNameMessageBehaviour : Behavior<TextBlock>
    {
        public NavigationStack NavigationStack { get; set; }

        protected override void OnAttached()
        {
            Messenger.Default.Register<PageTitleMessage>(this, OnNewMessage);       
        }

        protected override void OnDetaching()
        {
            Messenger.Default.Unregister<PageTitleMessage>(this, OnNewMessage);
        }

        private void OnNewMessage(PageTitleMessage message)
        {
            if (message.Page.GetRelevantStack() == NavigationStack)
                AssociatedObject.Text = message.NewTitle ?? string.Empty;
        }
    }
}
