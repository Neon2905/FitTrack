using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FitTrack.Converters
{
    /// <summary>
    /// A value converter that converts between a <see cref="Boolean"/> value and a <see cref="Visibility"/> value.
    /// </summary>
    /// <remarks>
    /// Implements the <see cref="IValueConverter"/> interface to provide a way to convert a boolean value to a visibility value
    /// for use in data binding scenarios in WPF, and vice versa.
    /// </remarks>
    public class BooleanToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts a <see cref="Boolean"/> value to a <see cref="Visibility"/> value.
        /// </summary>
        /// <param name="value">The value to convert, which should be a boolean.</param>
        /// <param name="targetType">The type to convert to. This is expected to be <see cref="Visibility"/>.</param>
        /// <param name="parameter">An optional parameter for the conversion.</param>
        /// <param name="culture">The culture to use for the conversion.</param>
        /// <returns>A <see cref="Visibility"/> value representing the converted boolean value. Returns <see cref="Visibility.Collapsed"/> if the input is not a boolean.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
                return boolValue ? Visibility.Visible : Visibility.Collapsed;
            else
                return Visibility.Collapsed;
        }

        /// <summary>
        /// Converts a <see cref="Visibility"/> value back to a <see cref="Boolean"/> value.
        /// </summary>
        /// <param name="value">The value to convert, which should be of type <see cref="Visibility"/>.</param>
        /// <param name="targetType">The type to convert to. This is expected to be <see cref="Boolean"/>.</param>
        /// <param name="parameter">An optional parameter for the conversion.</param>
        /// <param name="culture">The culture to use for the conversion.</param>
        /// <returns>A <see cref="Boolean"/> value representing the converted visibility value. Returns <c>false</c> if the input is not a <see cref="Visibility"/>.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
                return visibility == Visibility.Visible;
            else
                return false;
        }
    }
}
