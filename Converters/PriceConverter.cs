using System;
using System.Globalization;
using System.Windows.Data;

namespace PrintToCash.Converters
{
    public class PriceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal price)
            {
                string formattedPrice = string.Format("BGN {0}", price.ToString("N2"));
                return formattedPrice;
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
