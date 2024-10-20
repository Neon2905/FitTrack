using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FitTrack.Converters
{
    /// <summary>
    /// A value converter that inverts a <see cref="Boolean"/> value and converts it to a <see cref="Visibility"/> value.
    /// </summary>
    /// <remarks>
    /// Implements the <see cref="IValueConverter"/> interface to provide a way to invert a boolean value
    /// and map it to a <see cref="Visibility"/> enumeration for use in data binding scenarios in WPF.
    /// </remarks>
    public class InverseBooleanToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts a <see cref="Boolean"/> value to its inverse <see cref="Visibility"/> value.
        /// </summary>
        /// <param name="value">The boolean value to convert.</param>
        /// <param name="targetType">The type to convert to, expected to be <see cref="Visibility"/>.</param>
        /// <param name="parameter">An optional parameter for the conversion.</param>
        /// <param name="culture">The culture to use for the conversion.</param>
        /// <returns><see cref="Visibility.Collapsed"/> if the boolean value is <c>true</c>; otherwise, <see cref="Visibility.Visible"/>.
        /// Returns <see cref="Visibility.Visible"/> if the input is not a boolean.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
                return boolValue ? Visibility.Collapsed : Visibility.Visible;
            else
                return Visibility.Visible;
        }

        /// <summary>
        /// Converts a <see cref="Visibility"/> value back to its inverse boolean value.
        /// </summary>
        /// <param name="value">The <see cref="Visibility"/> value to convert.</param>
        /// <param name="targetType">The type to convert to, expected to be <see cref="Boolean"/>.</param>
        /// <param name="parameter">An optional parameter for the conversion.</param>
        /// <param name="culture">The culture to use for the conversion.</param>
        /// <returns><c>false</c> if the visibility value is <see cref="Visibility.Visible"/>; otherwise, <c>true</c>.
        /// Returns <c>false</c> if the input is not a <see cref="Visibility"/> value.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
            {
                return visibility != Visibility.Visible;
            }
            return false;
        }
    }
}
