using System.ComponentModel;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Views;
using AoTracker.Controls;
using AoTracker.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Android.Graphics.Color;

[assembly: ExportRenderer(typeof(CustomViewCell), typeof(CustomViewCellRenderer))]
namespace AoTracker.Droid.Renderers
{
    public class CustomViewCellRenderer : ViewCellRenderer
    {
        private Android.Views.View _cell;
        private bool _isSelected;
        private Drawable _unselectedBackground;

        protected override Android.Views.View GetCellCore(Cell item,
            Android.Views.View convertView,
            ViewGroup parent,
            Context context)
        {
            _cell = base.GetCellCore(item, convertView, parent, context);

            _isSelected = false;
            _unselectedBackground = _cell.Background;

            return _cell;
        }

        protected override void OnCellPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            base.OnCellPropertyChanged(sender, args);

            if (args.PropertyName == "IsSelected")
            {
                var viewCell = sender as CustomViewCell;
                _isSelected = !_isSelected;

                if (_isSelected)
                {
                    _cell.SetBackgroundColor(viewCell.SelectedBackgroundColor.ToAndroid());
                }
                else
                {
                    _cell.SetBackgroundColor(Color.Transparent); // change this line
                }
            }
        }
    }

}