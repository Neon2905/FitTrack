using FitTrack.Exceptions;
using System;
using FitTrack.Database;
using FitTrack.Core;
using Attribute = FitTrack.Database.Entities.Activity;

namespace FitTrack.Model
{
    /// <summary>
    /// Represents an activity associated with a challenge and exercise.
    /// </summary>
    class Activity : ServerBase
    {
        #region Properties

        private readonly int id;

        /// <summary>
        /// Gets the unique identifier of the activity.
        /// </summary>
        public int Id => id;

        /// <summary>
        /// Gets the identifier of the challenge associated with the activity.
        /// </summary>
        public int ChallengeId => int.Parse(Get(Attribute.ChallengeId));

        /// <summary>
        /// Gets the identifier of the exercise associated with the activity.
        /// </summary>
        public int ExerciseId => int.Parse(Get(Attribute.ExerciseId));

        /// <summary>
        /// Gets the name of the exercise associated with the activity.
        /// </summary>
        public string ExerciseName => new Exercise(ExerciseId).Name;

        /// <summary>
        /// Gets or sets the weight used in the activity.
        /// </summary>
        public double Weight
        {
            get
            {
                var weight = double.Parse(Get(Attribute.Weight));
                return LocalStorage.WeightInKg ? weight : Utilities.Convert.KgToLb(weight);
            }
            set => Update(Attribute.Weight, LocalStorage.WeightInKg ? value : Utilities.Convert.LbToKg(value));
        }

        /// <summary>
        /// Gets or sets the duration of the activity.
        /// </summary>
        public double Duration
		{
			get
			{
				var duration = double.Parse(Get(Attribute.Duration));
				return LocalStorage.DurationInHour ? Utilities.Convert.MinuteToHour(duration) : duration;
            }
			set => Update(Attribute.Duration, LocalStorage.DurationInHour ? Utilities.Convert.HourToMinute(value) : value);
		}

        /// <summary>
        /// Gets or sets the number of calories burned during the activity.
        /// </summary>
        public double Burned_Calories
		{
			get
            {
				var calories = Math.Round(double.Parse(Get(Attribute.Burned_Calories)), 2, MidpointRounding.AwayFromZero); //Rounded to 2 decimal
                return LocalStorage.EnergyInKCalories ? Utilities.Convert.CalToKcal(calories) : calories;
            }
				
			set => Update(Attribute.Burned_Calories, LocalStorage.EnergyInKCalories ? Utilities.Convert.KcalToCal(value) : value);
		}

        /// <summary>
        /// Gets the date and time when the activity was last updated, converted to local time.
        /// </summary>
        public DateTime UpdatedDateTime => Updated_at.ToLocalTime();

        /// <summary>
        /// Gets the date and time when the activity was created, converted to local time.
        /// </summary>
        public DateTime CreatedDateTime => Created_at.ToLocalTime();

        /// <summary>
        /// Gets the date and time when the activity was last updated.
        /// </summary>
        public DateTime Updated_at => DateTime.Parse(Get(Attribute.Updated_at));

        /// <summary>
        /// Gets the date and time when the activity was created.
        /// </summary>
        public DateTime Created_at => DateTime.Parse(Get(Attribute.Created_at));

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="Activity"/> class with the specified identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the activity.</param>
        /// <exception cref="ObjectNotFoundInDatabaseException">Thrown when the activity with the specified identifier does not exist in the database.</exception>
        public Activity(int id)
        {
			if (Exists(Entities.Activity.Id, id.ToString()))
				this.id = id;
			else
				throw new ObjectNotFoundInDatabaseException($"Activity with id:{id} does not exists", null, id, Entities.Table.Activity);
        }

        #region Equality and Operator Overloads

        /// <summary>
        /// Compares two <see cref="Activity"/> instances for equality.
        /// </summary>
        /// <param name="a">The first activity.</param>
        /// <param name="b">The second activity.</param>
        /// <returns><c>true</c> if the activities are equal; otherwise, <c>false</c>.</returns>
        public static bool operator == (Activity a, Activity b)
        {
            if (a is null & b is null)
                return true;
            //only one value will be null in this state
            else if (a is null || b is null)
                return false;
            return a.Id == b.Id;
        }

        /// <summary>
        /// Compares two <see cref="Activity"/> instances for inequality.
        /// </summary>
        /// <param name="a">The first activity.</param>
        /// <param name="b">The second activity.</param>
        /// <returns><c>true</c> if the activities are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator != (Activity a, Activity b) =>
            !(a == b);

        /// <summary>
        /// Determines whether the specified object is equal to the current <see cref="Activity"/> instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><c>true</c> if the specified object is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(Object obj)
        {
            if (obj is Activity activity)
                return this == activity;
            else
                return false;
        }

        /// <summary>
        /// Gets the hash code for the current <see cref="Activity"/> instance.
        /// </summary>
        /// <returns>The hash code for the current instance.</returns>
        public override int GetHashCode() =>
            Id.GetHashCode();

        #endregion

        #region Create

        /// <summary>
        /// Creates a new <see cref="Activity"/> instance with the specified challenge and exercise identifiers.
        /// </summary>
        /// <param name="ChallengeId">The identifier of the associated challenge.</param>
        /// <param name="ExerciseId">The identifier of the associated exercise.</param>
        /// <returns>The newly created <see cref="Activity"/> instance.</returns>
        public static Activity Create(int ChallengeId, int ExerciseId) => 
            CreateActivity(ChallengeId, ExerciseId);

        #endregion

        #region Read

        /// <summary>
        /// Gets the value of the specified attribute for the activity.
        /// </summary>
        /// <param name="Wanted_Attribute">The attribute to retrieve.</param>
        /// <returns>The value of the specified attribute.</returns>
        private string Get(Attribute Wanted_Attribute) =>
            GetById(Wanted_Attribute, this.Id);

        #endregion

        #region Update

        /// <summary>
        /// Updates the value of the specified attribute for the activity.
        /// </summary>
        /// <param name="Attribute">The attribute to update.</param>
        /// <param name="Value">The new value for the attribute.</param>
        private void Update(Attribute Attribute, object Value) =>
            UpdateById(this.Id, Attribute, Value);

        #endregion

        #region Delete

        /// <summary>
        /// Deletes the activity from the database.
        /// </summary>
        public void Delete() =>
            DeleteById(Entities.Table.Activity, this.Id);

		#endregion
	}
}