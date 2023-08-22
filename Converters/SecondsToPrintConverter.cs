using System;
using System.Globalization;
using System.Windows.Data;

namespace PrintToCash.Converters
{
    public class SecondsToPrintConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int secondsToPrint)
            {
                TimeSpan timeSpan = TimeSpan.FromSeconds(secondsToPrint);
                string formattedTime = string.Format("{0}h {1:D2}m", (int)timeSpan.TotalHours, timeSpan.Minutes);
                return formattedTime;
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
