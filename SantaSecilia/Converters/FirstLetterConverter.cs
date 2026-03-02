using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace SantaSecilia.Converters
{
    public class FirstLetterConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string text && !string.IsNullOrWhiteSpace(text))
            {
                return text.Substring(0, 1).ToUpper();
            }
            return "?"; 
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}