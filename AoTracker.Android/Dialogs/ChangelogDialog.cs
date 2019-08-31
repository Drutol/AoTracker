using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using AoLibs.Dialogs.Android;
using AoTracker.Android.Utils;
using AoTracker.Infrastructure.Models.DialogParameters;
using AoTracker.Infrastructure.ViewModels.Dialogs;
using AoTracker.Resources;
using GalaSoft.MvvmLight.Helpers;
using Xamarin.Essentials;

namespace AoTracker.Android.Dialogs
{
    public class ChangelogDialog : CustomArgumentViewModelDialogBase<ChangelogDialogViewModel, ChangelogDialogParameter>
    {
        protected override int LayoutResourceId { get; } = Resource.Layout.dialog_changelog;

        protected override void InitBindings()
        {
            Bindings.Add(this.SetBinding(() => ViewModel.Parameter).WhenSourceChanges(() =>
            {
                Header.Text =
                    $"{AppResources.Dialog_Changelog_Changelog} v{VersionTracking.CurrentVersion}.{VersionTracking.CurrentBuild} {ViewModel.Parameter.Date}";
                Changelog.SetAdapter(new RecyclerViewAdapterBuilder<string, ChangelogItemHolder>()
                    .WithItems(ViewModel.Parameter.Changelog)
                    .WithContentStretching()
                    .WithResourceId(LayoutInflater, Resource.Layout.item_changelog)
                    .WithDataTemplate(ChangelogDataTemplate)
                    .Build());
                if (string.IsNullOrEmpty(ViewModel.Parameter.Note))
                {
                    Note.Visibility = ViewStates.Gone;
                }
                else
                {
                    Note.Text = ViewModel.Parameter.Note;
                }
            }));

            Changelog.SetLayoutManager(new LinearLayoutManager(Activity));
            Dialog.Window.SetBackgroundDrawableResource(global::Android.Resource.Color.Transparent);
        }

        private void ChangelogDataTemplate(string item, ChangelogItemHolder holder, int position)
        {
            holder.Text.Text = item;
        }

        #region Views

        private TextView _header;
        private TextView _note;
        private RecyclerView _changelog;

        public TextView Header => _header ?? (_header = FindViewById<TextView>(Resource.Id.Header));
        public TextView Note => _note ?? (_note = FindViewById<TextView>(Resource.Id.Note));
        public RecyclerView Changelog => _changelog ?? (_changelog = FindViewById<RecyclerView>(Resource.Id.Changelog));

        #endregion

        class ChangelogItemHolder : RecyclerView.ViewHolder
        {
            private readonly View _view;

            public ChangelogItemHolder(View view) : base(view)
            {
                _view = view;
            }

            private TextView _text;

            public TextView Text => _text ?? (_text = _view.FindViewById<TextView>(Resource.Id.Text));
        }

    }
}