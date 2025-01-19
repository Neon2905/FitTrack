using FitTrack.Core;

namespace FitTrack.Utilities
{
    /// <summary>
    /// Provides methods to calculate calories burned based on MET (Metabolic Equivalent of Task), weight, and duration.
    /// </summary>
    public class Calculator
    {
        /// <summary>
        /// Calculates the calories burned per minute based on MET and weight.
        /// </summary>
        /// <param name="MET">The Metabolic Equivalent of Task for the exercise.</param>
        /// <param name="Weight">The weight of the individual in kilograms or pounds, depending on the setting.</param>
        /// <returns>The amount of calories burned per minute.</returns>
        private static double CalculateCaloriesBurnedPerMinute(double MET, double Weight)
        {
            return MET * 3.5 * (LocalStorage.WeightInKg ? Weight : Convert.LbToKg(Weight)) / 200;
        }

        /// <summary>
        /// Calculates the total calories burned based on MET, weight, and duration.
        /// </summary>
        /// <param name="MET">The Metabolic Equivalent of Task for the exercise.</param>
        /// <param name="Weight">The weight of the individual in kilograms or pounds, depending on the setting.</param>
        /// <param name="Duration">The duration of the activity in minutes or hours, depending on the setting.</param>
        /// <returns>The total amount of calories burned.</returns>
        public static double CalculateTotalCaloriesBurned(double MET, double Weight, double Duration)
        {
            double result =  CalculateCaloriesBurnedPerMinute(MET, Weight) * (LocalStorage.DurationInHour ? Convert.HourToMinute(Duration) : Duration);
            return LocalStorage.EnergyInKCalories ? Convert.CalToKcal(result) : result;
        }
    }
}
