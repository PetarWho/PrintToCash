using System;
using System.Globalization;
using System.Windows.Data;

namespace PrintToCash.Converters
{
    public class DateConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime date)
            {

                string formattedDate = date.ToString("dd MMM yyyy", CultureInfo.InvariantCulture);
                return formattedDate;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
