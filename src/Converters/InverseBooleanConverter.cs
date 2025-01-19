using System.Globalization;
using System;
using System.Windows.Data;

namespace FitTrack.Converters
{
    /// <summary>
    /// A value converter that inverts a <see cref="Boolean"/> value.
    /// </summary>
    /// <remarks>
    /// Implements the <see cref="IValueConverter"/> interface to provide a way to invert a boolean value
    /// for use in data binding scenarios in WPF.
    /// </remarks>
    public class InverseBooleanConverter : IValueConverter
    {
        /// <summary>
        /// Converts a <see cref="Boolean"/> value to its inverse.
        /// </summary>
        /// <param name="value">The boolean value to convert.</param>
        /// <param name="targetType">The type to convert to. This is expected to be <see cref="Boolean"/>.</param>
        /// <param name="parameter">An optional parameter for the conversion.</param>
        /// <param name="culture">The culture to use for the conversion.</param>
        /// <returns>The inverse of the input boolean value. Returns <c>false</c> if the input is not a boolean.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
                return !boolValue;
            else
                return false;
        }

        /// <summary>
        /// Converts a boolean value back to its inverse.
        /// </summary>
        /// <param name="value">The value to convert, which should be of type <see cref="Boolean"/>.</param>
        /// <param name="targetType">The type to convert to. This is expected to be <see cref="Boolean"/>.</param>
        /// <param name="parameter">An optional parameter for the conversion.</param>
        /// <param name="culture">The culture to use for the conversion.</param>
        /// <returns>The inverse of the input boolean value. Returns <c>false</c> if the input is not a boolean.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
                return !boolValue;
            else
                return false;
        }
    }
}
