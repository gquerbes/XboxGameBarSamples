using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace WidgetSampleCS
{
    public class VisibilityInverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if(Visibility.Collapsed == (Visibility)value)
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
