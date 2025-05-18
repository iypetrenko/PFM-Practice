
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PersonalFinanceManager.Converters
{
    public class StringToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str)
            {
                if (parameter?.ToString() == "NonEmpty")
                {
                    return string.IsNullOrWhiteSpace(str) ? Visibility.Visible : Visibility.Collapsed;
                }
                return string.IsNullOrWhiteSpace(str) ? Visibility.Collapsed : Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}