using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace AoTracker.UWP.Converters
{
    public class ItemIndexConverter : DependencyObject, IValueConverter
    {
        public static readonly DependencyProperty CollectionProperty = DependencyProperty.Register(
            "Collection", typeof(IList), typeof(ItemIndexConverter), new PropertyMetadata(default(IList)));

        public IList Collection
        {
            get => (IList) GetValue(CollectionProperty);
            set => SetValue(CollectionProperty, value);
        }
        
        public virtual object Convert(object value, Type targetType, object parameter, string language)
        {
            return Collection.IndexOf(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
