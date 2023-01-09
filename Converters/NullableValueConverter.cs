using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace ProgiTest.Converters
{
    public class NullableValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return ((double?)value).Value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return double.Parse((string)value);
        }
    }
}
