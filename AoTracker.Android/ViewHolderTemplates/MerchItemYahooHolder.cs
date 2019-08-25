using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using Android.Widget;
using AoTracker.Android.Activities;
using AoTracker.Android.Utils;
using AoTracker.Crawlers.Sites.Yahoo;
using AoTracker.Infrastructure.Util;
using AoTracker.Infrastructure.ViewModels.Item;
using AoTracker.Interfaces;
using Java.Lang;

namespace AoTracker.Android.ViewHolders
{
    class MerchItemYahooHolderTemplate
    {
        public static void DataTemplate(IMerchItem item, IMerchItemHolderYahoo holder, int position)
        {
            var yahooItem = (YahooItem)item.Item;

            MerchItemHolderTemplate.CommonFeedItemTemplate(item, holder);

            holder.Title.Text = item.Item.Name;
            holder.DetailBids.SetText(GetYahooItemLabel("Bids:", yahooItem.BidsCount.ToString()),
                TextView.BufferType.Spannable);
            holder.DetailEndsIn.SetText(GetYahooItemLabel("Ends in:", SharedUtil.TimeDiffToString((DateTime.UtcNow - yahooItem.EndTime).Duration())),
                TextView.BufferType.Spannable);
            holder.DetailCondition.SetText(GetYahooItemLabel("Condition:", yahooItem.Condition.ToString()),
                TextView.BufferType.Spannable);

            //if (yahooItem.BuyoutPrice != 0)
            //{
            //    holder.PriceSubtitle.Text = $"{yahooItem.BuyoutPrice}¥";
            //}

            if (yahooItem.Tax == 0)
            {
                holder.DetailsTax.Visibility = ViewStates.Gone;
            }
            else
            {
                holder.DetailsTax.Visibility = ViewStates.Visible;
                holder.DetailsTax.SetText(GetYahooItemLabel("Tax:", $"+{yahooItem.Tax}%"),
                    TextView.BufferType.Spannable);
            }

            holder.DetailShipping.Visibility = BindingConverters.BoolToVisibility(yahooItem.IsShippingFree);
        }

        private static ICharSequence GetYahooItemLabel(string note, string value)
        {
            var spannable = new SpannableString($"{note} {value}");

            spannable.SetSpan(
                new TypefaceSpan(
                    MainActivity.Instance.Resources.GetString(Resource.String.font_family_light)),
                0,
                note.Length,
                SpanTypes.ExclusiveInclusive);
            spannable.SetSpan(
                new TypefaceSpan(
                    MainActivity.Instance.Resources.GetString(Resource.String.font_family_medium)),
                note.Length,
                note.Length + value.Length + 1,
                SpanTypes.ExclusiveInclusive);

            return spannable;
        }
    }

    
}