using System;
using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace PrintToCash.Converters
{
    public class ListCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return ((ICollection)value).Count.ToString();
            }
            catch (Exception)
            {
                return "Error!";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
