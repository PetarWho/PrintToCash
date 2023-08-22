using System;
using System.Globalization;
using System.Windows.Data;

namespace PrintToCash.Converters
{
    public class GramsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is float weight)
            {
                string formattedWeight = string.Format("{0}g", weight);
                return formattedWeight;
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
