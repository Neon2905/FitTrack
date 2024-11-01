using FitTrack.Model;
using System.Diagnostics;
using StoredData = FitTrack.Properties.Settings;

namespace FitTrack.Core
{
    /// <summary>
    /// Provides access to application settings stored in <see cref="StoredData"/> across the project.
    /// </summary>
    static class LocalStorage
    {
        /// <summary>
        /// Gets or sets a value indicating whether energy is displayed in kilocalories.
        /// </summary>
        public static bool EnergyInKCalories
        {
            get => StoredData.Default.IsKCalories;
            set { StoredData.Default.IsKCalories = value; StoredData.Default.Save(); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether weight is displayed in kilograms.
        /// </summary>
        public static bool WeightInKg
        {
            get => StoredData.Default.IsMetric;
            set { StoredData.Default.IsMetric = value; StoredData.Default.Save(); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether duration is displayed in hours.
        /// </summary>
        public static bool DurationInHour
        {
            get => StoredData.Default.DurationInHour;
            set { StoredData.Default.DurationInHour = value; StoredData.Default.Save(); }
        }

        /// <summary>
        /// Gets or sets the login token used for login authentication.
        /// </summary>
        public static string LoginToken
        {
            get => StoredData.Default?.LoginToken;
            set { StoredData.Default.LoginToken = value; StoredData.Default.Save(); }
        }

        public static string DeviceId
        {
            get => StoredData.Default?.DeviceID;
            set { StoredData.Default.DeviceID = value; StoredData.Default.Save();}
        }

        /// <summary>
        /// Gets the registered SessoinDevice instance for this system.
        /// </summary>
        public static SessionDevice SessionDevice
        {
            get => new SessionDevice(DeviceId);
            set => DeviceId = value.Id.ToString();
        }

        /// <summary>
        /// Reset all settings and local data to default
        /// </summary>
        /// <remarks>
        /// <br>WARNING: </br>Calling this method will reset all locally saved settings and data (login cookies) to default values. Use only when necessary!
        /// </remarks>
        public static void Reset()
        {
            EnergyInKCalories = WeightInKg = DurationInHour = false;
            LoginToken = DeviceId = null;

            Trace.WriteLine("ALERT: Local Storage data reset to default!");
        }
    }
}
