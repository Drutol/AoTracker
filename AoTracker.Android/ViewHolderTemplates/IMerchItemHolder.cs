using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;

namespace AoTracker.Android.ViewHolders
{
    public interface IMerchItemHolder
    {
        TextView Title { get; }
        ImageView PriceTrendIcon { get; }
        ImageView ImageLeft { get; }
        FloatingActionButton NewAlertSection { get; }
        TextView Price { get; }
        TextView PriceSubtitle { get; }
        LinearLayout ClickSurface { get; }
        LinearLayout PriceSection { get; }
    }

    public interface IMerchItemHolderGeneral : IMerchItemHolder
    {

        ImageView StoreIcon { get; }
        TextView Detail { get; }
        FrameLayout DetailSection { get; }
        TextView Subtitle { get; }
    }

    public interface IMerchItemHolderYahoo : IMerchItemHolder
    {
        ImageView StoreIcon { get; }
        TextView DetailBids { get; }
        TextView DetailEndsIn { get; }
        TextView DetailCondition { get; }
        LinearLayout DetailSection { get; }
        TextView DetailShipping { get; }
        TextView DetailsTax { get; }
    }
}