using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using Android.Widget;
using AoLibs.Utilities.Android;
using AoLibs.Utilities.Android.Listeners;
using AoTracker.Android.Activities;
using AoTracker.Android.Fragments.Feed;
using AoTracker.Android.Themes;
using AoTracker.Android.Utils;
using AoTracker.Crawlers.Mandarake;
using AoTracker.Crawlers.Sites.Lashinbang;
using AoTracker.Crawlers.Sites.Mercari;
using AoTracker.Crawlers.Sites.Yahoo;
using AoTracker.Crawlers.Surugaya;
using AoTracker.Domain.Enums;
using AoTracker.Infrastructure.ViewModels.Item;
using AoTracker.Interfaces;
using FFImageLoading;
using Java.Lang;
using String = System.String;

namespace AoTracker.Android.ViewHolders
{
    public class MerchItemHolderTemplate
    {
        public static void DataTemplate(IMerchItem item, IMerchItemHolderGeneral holder, int position)
        {
            CommonFeedItemTemplate(item, holder);

            if (item.Item is SurugayaItem surugayaItem)
            {
                holder.Title.Text = surugayaItem.Category;
                holder.Detail.Text = surugayaItem.Name;
                holder.Detail.Visibility = ViewStates.Visible;
                holder.Subtitle.Text = surugayaItem.Brand;
                holder.StoreIcon.SetImageResource(Resource.Drawable.surugaya);
            }
            else if (item.Item is MandarakeItem mandarakeItem)
            {
                holder.Title.Text = mandarakeItem.Name;
                holder.Detail.Visibility = ViewStates.Gone;
                holder.Subtitle.Text = mandarakeItem.Shop;
                holder.StoreIcon.SetImageResource(Resource.Drawable.mandarake);
            }
            else if (item.Item is MercariItem mercariItem)
            {
                holder.Title.Text = mercariItem.Name;
                holder.Detail.Visibility = ViewStates.Gone;
                holder.Subtitle.Text = String.Empty;
                holder.StoreIcon.SetImageResource(Resource.Drawable.mercari);
            }
            else if (item.Item is LashinbangItem lashinbangItem)
            {
                holder.Title.Text = lashinbangItem.Name;
                holder.Detail.Visibility = ViewStates.Gone;
                holder.Subtitle.Text = String.Empty;
                holder.StoreIcon.SetImageResource(Resource.Drawable.lashinbang);
            }
        }

        internal static void CommonFeedItemTemplate(IMerchItem item, IMerchItemHolder holder)
        {
            holder.Price.Text = item.Item.Price + "¥";
            ImageService.Instance.LoadUrl(item.Item.ImageUrl).Retry(2, 1000).Into(holder.ImageLeft);

            if (item is FeedItemViewModel feedViewModel)
            {
                holder.NewAlertSection.Visibility = BindingConverters.BoolToVisibility(feedViewModel.IsNew);
                switch (feedViewModel.PriceChange)
                {
                    case PriceChange.Stale:
                        holder.PriceTrendIcon.Visibility = ViewStates.Gone;
                        holder.PriceSubtitle.Visibility = ViewStates.Gone;
                        break;
                    case PriceChange.Decrease:
                        holder.PriceSubtitle.Text = $"({feedViewModel.PriceDifference:N0}¥)";
                        holder.PriceTrendIcon.Visibility = ViewStates.Visible;
                        holder.PriceSubtitle.Visibility = ViewStates.Visible;
                        holder.PriceTrendIcon.SetImageResource(Resource.Drawable.icon_chevron_triple_down);
                        holder.PriceTrendIcon.ImageTintList = ColorStateList.ValueOf(ThemeManager.LimeColour);
                        break;
                    case PriceChange.Increase:
                        holder.PriceSubtitle.Text = $"(+{feedViewModel.PriceDifference:N0}¥)";
                        holder.PriceTrendIcon.Visibility = ViewStates.Visible;
                        holder.PriceSubtitle.Visibility = ViewStates.Visible;
                        holder.PriceTrendIcon.SetImageResource(Resource.Drawable.icon_chevron_triple_up);
                        holder.PriceTrendIcon.ImageTintList = ColorStateList.ValueOf(ThemeManager.RedColour);
                        break;
                }
            }
            else
            {
                holder.NewAlertSection.Visibility = ViewStates.Gone;
                holder.PriceTrendIcon.Visibility = ViewStates.Gone;
                holder.PriceSubtitle.Visibility = ViewStates.Gone;
            }
        }
    }
}