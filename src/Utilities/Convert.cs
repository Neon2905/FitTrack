using System;

namespace FitTrack.Utilities
{
    /// <summary>
    /// Provides utility methods for converting between different units of measurement.
    /// </summary>
    public class Convert
    {
        /// <summary>
        /// Converts weight from pounds to kilograms.
        /// </summary>
        /// <param name="input">The weight in pounds.</param>
        /// <returns>The weight in kilograms, rounded to two decimal places.</returns>
        public static double LbToKg(double input)
        {
            return Math.Round(input / 2.20462, 2);
        }

        /// <summary>
        /// Converts weight from kilograms to pounds.
        /// </summary>
        /// <param name="input">The weight in kilograms.</param>
        /// <returns>The weight in pounds, rounded to two decimal places.</returns>
        public static double KgToLb(double input)
        {
            return Math.Round(input * 2.20462, 2);
        }

        /// <summary>
        /// Converts calories to kilocalories.
        /// </summary>
        /// <param name="input">The number of calories.</param>
        /// <returns>The number of kilocalories, rounded to two decimal places.</returns>
        public static double CalToKcal(double input)
        {
            return Math.Round(input / 1000, 2);
        }

        /// <summary>
        /// Converts kilocalories to calories.
        /// </summary>
        /// <param name="input">The number of kilocalories.</param>
        /// <returns>The number of calories, rounded to two decimal places.</returns>
        public static double KcalToCal(double input)
        {
            return Math.Round(input * 1000, 2);
        }

        /// <summary>
        /// Converts hours to minutes.
        /// </summary>
        /// <param name="input">The time in hours.</param>
        /// <returns>The time in minutes, rounded to one decimal place.</returns>
        public static double HourToMinute(double input)
        {
            return Math.Round(input * 60, 1);
        }

        /// <summary>
        /// Converts minutes to hours.
        /// </summary>
        /// <param name="input">The time in minutes.</param>
        /// <returns>The time in hours, rounded to one decimal place.</returns>
        public static double MinuteToHour(double input)
        {
            return Math.Round(input / 60, 1);
        }
    }
}
