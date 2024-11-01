using FitTrack.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using FitTrack.Exceptions;
using FitTrack.Database;
using Attribute = FitTrack.Database.Entities.Challenge;
using FitTrack.Properties;

namespace FitTrack.Model
{
    /// <summary>
    /// Represents a challenge that includes activities and tracks progress towards a goal.
    /// </summary>
    class Challenge : ServerBase
    {
        #region Prpoerties
        private readonly int id;

        /// <summary>
        /// Gets the unique identifier of the challenge.
        /// </summary>
        public int Id => id;

        /// <summary>
        /// Gets or sets the name of the challenge.
        /// </summary>
        public string Name
        {
            get => Get(Attribute.Name);
            set => Update(Attribute.Name, value);
        }

        /// <summary>
        /// Gets or sets the calorie goal for the challenge.
        /// </summary>
        public double Calories_Goal
        {
            get 
            {
                string requested_item = Get(Attribute.Calories_Goal);
                double calories = string.IsNullOrEmpty(requested_item) ? 0 : Math.Round(double.Parse(requested_item), 2, MidpointRounding.AwayFromZero); //Set 0 if Value does not exist
                return Settings.Default.IsKCalories ? Utilities.Convert.CalToKcal(calories) : calories;
            }
            
            set => Update(Attribute.Calories_Goal, Settings.Default.IsKCalories ? Utilities.Convert.KcalToCal(value) : value);
        }

        /// <summary>
        /// Gets or sets the progressed calories towards the challenge goal.
        /// </summary>
        public double Progressed_Calories
        {
            get 
            {
                string requested_item = Get(Attribute.Progressed_Calories);
                double calories = string.IsNullOrEmpty(requested_item) ? 0 : Math.Round(double.Parse(requested_item), 2, MidpointRounding.AwayFromZero); //Set 0 if Value does not exist
                return Settings.Default.IsKCalories ? Utilities.Convert.CalToKcal(calories) : calories;
            }
            set => Update(Attribute.Progressed_Calories, Settings.Default.IsKCalories ? Utilities.Convert.KcalToCal(value) : value);
        }

        /// <summary>
        /// Gets the percentage of progress made towards the calorie goal.
        /// </summary>
        public double Progress_Percentage
        {
            get
            {
                double percent = Math.Round(Progressed_Calories / Calories_Goal * 100, 2);
                return percent < 100 ? percent : 100;
            }
        }

        /// <summary>
        /// Gets whether the challenge has contributed any progress.
        /// </summary>
        public bool HasContributed => Progressed_Calories > 0;

        /// <summary>
        /// Gets whether the challenge goal has already been reached.
        /// </summary>
        public bool GoalAlreadyReached => Progressed_Calories >= Calories_Goal;

        /// <summary>
        /// Gets the formatted local date and time by which the challenge is to be reached.
        /// </summary>
        public string ToReachAtDateTime
        {
            get
            {
                if (To_Reach_At is null)
                    return null;
                else
                    return DateTime.Parse(To_Reach_At.ToString()).ToLocalTime().ToString("dd MMMM, yyyy");
            }
        }

        /// <summary>
        /// Gets whether the challenge has expired based on the target date.
        /// </summary>
        public bool Expired => !(To_Reach_At is null) && DateTime.UtcNow > To_Reach_At;

        /// <summary>
        /// Gets the formatted local date and time when the challenge was finished.
        /// </summary>
        public string FinishedAtDateTime
        {
            get
            {
                if (Finished_At is null)
                    return null;
                else
                    return DateTime.Parse(Finished_At.ToString()).ToLocalTime().ToString("dddd, dd MMMM, yyyy");
            }
        }

        /// <summary>
        /// Gets or sets the date and time by which the challenge is to be reached.
        /// </summary>
        public DateTime? To_Reach_At
        {
            get => Get(Attribute.To_Reach_at).Length>0? DateTime.Parse( Get(Attribute.To_Reach_at) ) : new DateTime?(); //Set null if Value does not exist
            set => Update(Attribute.To_Reach_at, value);
        }

        /// <summary>
        /// Gets or sets the date and time when the challenge was finished.
        /// </summary>
        public DateTime? Finished_At
        {
            get => Get(Attribute.Finished_at).Length>0? DateTime.Parse( Get(Attribute.Finished_at) ) : new DateTime?(); //Set null if Value does not exist
            set => Update(Attribute.Finished_at, value);
        }

        /// <summary>
        /// Gets whether the challenge has a target date.
        /// </summary>
        public bool HasTargetDate => !(To_Reach_At is null);

        /// <summary>
        /// Gets the list of activities associated with the challenge.
        /// </summary>
        #pragma warning disable IDE1006 // Naming Styles
        private List<Activity> activities
        {
            get
            {
                List<Activity> activities = new List<Activity>();

                //Get all associated activities
                foreach (string ActivityId in GetAll(Entities.Activity.Id, Entities.Activity.ChallengeId, this.id))
                    activities.Add(new Activity(int.Parse(ActivityId)));
                return activities;
            }
            set => activities = value;
        }
        #pragma warning restore IDE1006 // Naming Styles

        /// <summary>
        /// Gets a read-only collection of activities associated with the challenge, ordered by creation date.
        /// </summary>
        public ReadOnlyCollection<Activity> Activities => new ReadOnlyCollection<Activity>(activities.OrderByDescending(x => x.Created_at).ToList());

        /// <summary>
        /// Gets the last formatted local date and time when a contribution was made to the challenge.
        /// </summary>
        public string LastContributedAt
        {
            get
            {
                if (HasContributed)
                    return UpdatedDateTime.ToLocalTime().ToString("dd MMMM, yyyy");
                else
                    return null;
            }
        }

        /// <summary>
        /// Gets the date and time when the challenge was last updated, converted to local time.
        /// </summary>
        public DateTime UpdatedDateTime => Updated_at.ToLocalTime();

        /// <summary>
        /// Gets the date and time when the challenge was created, converted to local time.
        /// </summary>
        public DateTime CreatedDateTime => Created_at.ToLocalTime();

        /// <summary>
        /// Gets the date and time when the challenge was last updated.
        /// </summary>
        public DateTime Updated_at => DateTime.Parse(Get(Attribute.Updated_at));

        /// <summary>
        /// Gets the date and time when the challenge was created.
        /// </summary>
        public DateTime Created_at => DateTime.Parse(Get(Attribute.Created_at));

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="Challenge"/> class with the specified identifier.
        /// </summary>
        /// <param name="Id">The unique identifier of the challenge.</param>
        /// <exception cref="ObjectNotFoundInDatabaseException">Thrown when the challenge with the specified identifier does not exist in the database.</exception>
        public Challenge(int Id)
        {
            if(Exists(Attribute.Id, Id.ToString()))
                this.id = Id;
            else
                throw new ObjectNotFoundInDatabaseException($"Challenge with id:{id} does not exists", null, id, Entities.Table.Challenge);
        }

        /// <summary>
        /// Calculates the total progressed calories for a specific exercise.
        /// </summary>
        /// <param name="ExerciseId">The identifier of the exercise.</param>
        /// <returns>The total progressed calories for the specified exercise.</returns>
        public double Toatl_Progressed_Calories(int ExerciseId)
        {
            double result = 0;
            foreach (Activity activity in Activities)
            {
                if (activity.ExerciseId == ExerciseId)
                    result += activity.Burned_Calories;
            }
            return result;
        }

        /// <summary>
        /// Calculates the total progressed calories for a specific exercise type.
        /// </summary>
        /// <param name="Type">The type of exercise.</param>
        /// <returns>The total progressed calories for the specified exercise type.</returns>
        public double Toatl_Progressed_Calories(ExerciseType Type) 
            => Toatl_Progressed_Calories(new Exercise(Type).Id);

        /// <summary>
        /// Determines whether a challenge with the specified identifier exists.
        /// </summary>
        /// <param name="Id">The identifier of the challenge.</param>
        /// <returns><c>true</c> if the challenge exists; otherwise, <c>false</c>.</returns>
        public static bool Find(object Id) =>
            Exists(Attribute.Id, Id);

        #region Equality and Operator Overloads

        /// <summary>
        /// Compares two <see cref="Challenge"/> instances for equality.
        /// </summary>
        /// <param name="a">The first challenge.</param>
        /// <param name="b">The second challenge.</param>
        /// <returns><c>true</c> if the challenges are equal; otherwise, <c>false</c>.</returns>
        public static bool operator == (Challenge a, Challenge b)
        {
            if(a is null & b is null)
                return true;
            //only one value will be null in this state
            else if(a is null || b is null) 
                return false;
            return a.Id == b.Id;
        }

        /// <summary>
        /// Compares two <see cref="Challenge"/> instances for inequality.
        /// </summary>
        /// <param name="a">The first challenge.</param>
        /// <param name="b">The second challenge.</param>
        /// <returns><c>true</c> if the challenges are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Challenge a, Challenge b) =>
            !(a==b);

        /// <summary>
        /// Determines whether the specified object is equal to the current <see cref="Challenge"/> instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><c>true</c> if the specified object is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(Object obj)
        {
            if (obj is Challenge challenge)
                return this == challenge;
            else
                return false;
        }

        /// <summary>
        /// Returns a hash code for the current <see cref="Challenge"/> instance.
        /// </summary>
        /// <returns>A hash code for the current instance.</returns>
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        #endregion

        #region Create

        /// <summary>
        /// Creates a new challenge with the specified parameters.
        /// </summary>
        /// <param name="UserId">The user identifier.</param>
        /// <param name="CaloriesGoal">The calorie goal for the challenge.</param>
        /// <param name="GoalDate">The optional goal date for the challenge.</param>
        /// <returns>The newly created <see cref="Challenge"/> instance.</returns>
        public static Challenge Create(string UserId, double CaloriesGoal, DateTime? GoalDate = null) => 
            CreateChallenge(UserId, CaloriesGoal, GoalDate);

        /// <summary>
        /// Creates a new activity for the challenge.
        /// </summary>
        /// <param name="ExerciseId">The identifier of the exercise.</param>
        /// <param name="Weight">The weight used during the activity.</param>
        /// <param name="Duration">The duration of the activity.</param>
        /// <param name="Burned_Calories">The number of calories burned during the activity.</param>
        public void CreateActivity(int ExerciseId, double Weight, double Duration, double Burned_Calories)
        {
            Activity activity = Activity.Create(this.Id, ExerciseId);
            activity.Weight = Weight;
            activity.Duration = Duration;
            activity.Burned_Calories = Burned_Calories;
            Progressed_Calories += Burned_Calories;
        }

        #endregion

        #region Read

        /// <summary>
        /// Retrieves the value of the specified attribute for the challenge.
        /// </summary>
        /// <param name="Wanted_attribute">The attribute to retrieve.</param>
        /// <returns>The value of the attribute.</returns>
        private string Get(Attribute Wanted_attribute) =>
            GetById(Wanted_attribute, this.Id);

        #endregion

        #region Update

        /// <summary>
        /// Updates the value of the specified attribute for the challenge.
        /// </summary>
        /// <param name="Attribute">The attribute to update.</param>
        /// <param name="Value">The new value of the attribute.</param>
        private void Update(Attribute Attribute, object Value) =>
            UpdateById(this.Id, Attribute, Value);

        #endregion

        #region Delete

        /// <summary>
        /// Deletes the challenge and all associated activities.
        /// </summary>
        public void Delete()
        {
            //Delete all associated activity
            foreach(var activity in this.Activities)
                activity.Delete();
            //Delete itself
            DeleteById(Entities.Table.Challenge, this.Id);
        }

        /// <summary>
        /// Removes a specific activity from the challenge.
        /// </summary>
        /// <param name="activity">The activity to remove.</param>
        /// <exception cref="InvalidEntityAccessException">Thrown when the activity does not belong to this challenge.</exception>
        public void RemoveActivity(Activity activity)
        {
            //Only allows when activity is under this challenge
            if (activities.Contains(activity))
                activity.Delete();
            else
                throw new InvalidEntityAccessException("Illegal Delete-Operation. Activity does not belong to this Challenge");
        }
        #endregion
    }
}
