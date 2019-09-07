using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using AoTracker.Domain.Messaging;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Xaml.Interactivity;

namespace AoTracker.UWP.Behaviours
{
    public class ToolbarRequestMessageBehaviour : Behavior<AppBarButton>
    {
        public ToolbarRequestMessage AcceptedToolbarRequestMessage { get; set; }
        public ToolbarActionMessage InvokedToolbarActionMessage { get; set; }

        protected override void OnAttached()
        {
            Messenger.Default.Register<ToolbarRequestMessage>(this, OnRequestMessage);
            AssociatedObject.Visibility = Visibility.Collapsed;
            AssociatedObject.Click += AssociatedObjectOnClick;
        }

        private void AssociatedObjectOnClick(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send(InvokedToolbarActionMessage);
        }

        protected override void OnDetaching()
        {
            Messenger.Default.Unregister<ToolbarRequestMessage>(this, OnRequestMessage);
        }

        private void OnRequestMessage(ToolbarRequestMessage request)
        {
            if(request == AcceptedToolbarRequestMessage)
            {
                AssociatedObject.Visibility = Visibility.Visible;
            }
            else if (request == ToolbarRequestMessage.ResetToolbar)
            {
                AssociatedObject.Visibility = Visibility.Visible;
            }
        }
    }
}
