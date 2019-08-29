using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Input;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using ModernHttpClient;

namespace AoTracker.Android.Utils
{
    [Preserve(AllMembers = true)]
    static class LinkerIncludes
    {
        private static void Include()
        {
            var i1 = new FitWindowsFrameLayout(null,null);
            var i2 = new FitWindowsLinearLayout(null,null);
        }

        private static void Include(Button button)
        {
            button.Click += (s, e) => button.Text = button.Text + "";
        }

        private static void Include(CheckBox checkBox)
        {
            checkBox.CheckedChange += (sender, args) => checkBox.Checked = !checkBox.Checked;
        }

        private static void Include(View view)
        {
            view.Click += (s, e) => view.ContentDescription = view.ContentDescription + "";
        }

        private static void Include(TextView text)
        {
            text.TextChanged += (sender, args) => text.Text = "" + text.Text;
            text.Hint = "" + text.Hint;
        }

        private static void Include(CompoundButton cb)
        {
            cb.CheckedChange += (sender, args) => cb.Checked = !cb.Checked;
        }

        private static void Include(SeekBar sb)
        {
            sb.ProgressChanged += (sender, args) => sb.Progress = sb.Progress + 1;
        }

        private static void Include(ProgressBar sb)
        {
            var a = sb.Max;
            sb.Max = 10;
        }

        private static void Include(UmAlQuraCalendar sb)
        {
            sb = new System.Globalization.UmAlQuraCalendar();
        }

        private static void Include(ThaiBuddhistCalendar sb)
        {
            sb = new System.Globalization.ThaiBuddhistCalendar();
        }

        private static void Include(Switch sw)
        {
            sw.CheckedChange += (sender, args) =>
            {

            };
        }

        private static void Include(CollapsingToolbarLayout sw)
        {
            sw = new CollapsingToolbarLayout(null);
            sw = new CollapsingToolbarLayout(null, null);
            sw = new CollapsingToolbarLayout(null, null, 0);
        }

        private static void Include(EditText et)
        {
            et.Text = "fdsf";
            et.SetText("aaa", TextView.BufferType.Normal);
            et.TextChanged += (sender, args) =>
            {

            };
            et.AfterTextChanged += (sender, args) =>
            {

            };
        }

        private static void Include(INotifyCollectionChanged changed)
        {
            changed.CollectionChanged += (s, e) => { var test = string.Format("{0}{1}{2}{3}{4}", e.Action, e.NewItems, e.NewStartingIndex, e.OldItems, e.OldStartingIndex); };
        }

        private static void Include(ICommand command)
        {
            command.CanExecuteChanged += (s, e) => { if (command.CanExecute(null)) command.Execute(null); };
        }

        private static void Include(AesCryptoServiceProvider a)
        {
            System.Security.Cryptography.AesCryptoServiceProvider b = new System.Security.Cryptography.AesCryptoServiceProvider();
        }
    }
}