using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace ToastrForge.Converters;

public class ColorBrushConverter : IValueConverter
{
    public static readonly ColorBrushConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Color color)
        {
            return new SolidColorBrush(color);
        }
        return null; // Or a default brush
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is SolidColorBrush brush)
        {
            return brush.Color;
        }
        return null;
    }
}
