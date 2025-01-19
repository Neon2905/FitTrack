using FitTrack.Utilities;
using System;
using System.Collections.Generic;
using FitTrack.Database;
using Attribute = FitTrack.Database.Entities.Exercise;
using FitTrack.Exceptions;

namespace FitTrack.Model
{
    /// <summary>
    /// Represents an exercise with various attributes and methods to retrieve exercise data and metrics.
    /// </summary>
    class Exercise : ServerBase
    {
        #region Properties

        private readonly int id;

        /// <summary>
        /// Gets the unique identifier of the exercise.
        /// </summary>
        public int Id => id;

        /// <summary>
        /// Gets the name of the exercise.
        /// </summary>
        public string Name => Get(Attribute.Name);

        /// <summary>
        /// Gets the type of the exercise based on its name.
        /// </summary>
        public ExerciseType Type 
        {
            get
            {
                switch (Name.ToLower())
                {
                    case ("dancing"):
                        return ExerciseType.Dancing;
                    case ("cycling"):
                        return ExerciseType.Cycling;
                    case ("climbingstairs"):
                        return ExerciseType.ClimbingStairs;
                    case ("walking"):
                        return ExerciseType.Walking;
                    case ("running"):
                        return ExerciseType.Running;
                    case ("swimming"):
                        return ExerciseType.Swimming;
                    case ("weightlifting"):
                        return ExerciseType.WeightLifting;
                    default:
                        return ExerciseType.Other;
                }
            }
        }

        /// <summary>
        /// Gets the MET (Metabolic Equivalent of Task) value for low intensity of the exercise.
        /// </summary>
        public double LOW_MET => double.Parse(Get(Attribute.LOW_MET));

        /// <summary>
        /// Gets the MET value for medium intensity of the exercise.
        /// </summary>
        public double MEDIUM_MET => double.Parse(Get(Attribute.MEDIUM_MET));

        /// <summary>
        /// Gets the MET value for extreme intensity of the exercise.
        /// </summary>
        public double EXTREME_MET => double.Parse(Get(Attribute.EXTREME_MET));

        /// <summary>
        /// Gets the expression for low MET intensity.
        /// </summary>
        public string LOW_MET_EXPRESSION => Get(Attribute.LOW_MET_EXPRESSION);

        /// <summary>
        /// Gets the expression for medium MET intensity.
        /// </summary>
        public string MEDIUM_MET_EXPRESSION => Get(Attribute.MEDIUM_MET_EXPRESSION);

        /// <summary>
        /// Gets the expression for extreme MET intensity.
        /// </summary>
        public string EXTREME_MET_EXPRESSION => Get(Attribute.EXTREME_MET_EXPRESSION);

        /// <summary>
        /// Gets the date and time when the exercise was last updated, converted to local time.
        /// </summary>
        public DateTime UpdatedDateTime => Updated_at.ToLocalTime();

        /// <summary>
        /// Gets the date and time when the exercise was created, converted to local time.
        /// </summary>
        public DateTime CreatedDateTime => Created_at.ToLocalTime();

        /// <summary>
        /// Gets the date and time when the exercise was last updated.
        /// </summary>
        public DateTime Updated_at => DateTime.Parse(Get(Attribute.Updated_at));

        /// <summary>
        /// Gets the date and time when the exercise was created.
        /// </summary>
        public DateTime Created_at => DateTime.Parse(Get(Attribute.Created_at));

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Exercise"/> class with the specified identifier.
        /// </summary>
        /// <param name="Id">The unique identifier of the exercise.</param>
        /// <exception cref="ObjectNotFoundInDatabaseException">Thrown when the exercise with the specified identifier does not exist in the database.</exception>
        public Exercise(int Id)
        {
            if (Exists(Attribute.Id, Id))
                this.id = Id;
            else
                throw new ObjectNotFoundInDatabaseException($"Exercise with id:{id} does not exists", null, id, Entities.Table.Exercise);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Exercise"/> class based on the <see cref="ExerciseType"/>.
        /// </summary>
        /// <param name="Type">The type of the exercise.</param>
        public Exercise(ExerciseType Type)
        {
            //Find id of given ExerciseType
            int id = int.Parse(Get(Attribute.Id,Attribute.Name,NameOf(Type)));
            this.id = id;
        }

        #endregion

        /// <summary>
        /// Gets the MET value for a given <see cref="ExerciseIntensity"/>.
        /// </summary>
        /// <param name="ExerciseIntensity">The intensity of the exercise.</param>
        /// <returns>The MET value corresponding to the specified <see cref="ExerciseIntensity"/>.</returns>
        public double METof(ExerciseIntensity ExerciseIntensity)
        {
            switch (ExerciseIntensity)
            {
                case ExerciseIntensity.Low:
                    return LOW_MET;
                case ExerciseIntensity.Medium:
                    return MEDIUM_MET;
                case ExerciseIntensity.Extreme:
                    return EXTREME_MET;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Gets the low MET value for a specific <see cref="ExerciseType"/>.
        /// </summary>
        /// <param name="Type">The type of exercise.</param>
        /// <returns>The low MET value for the specified <see cref="ExerciseType"/>.</returns>
        public static double Low_MET(ExerciseType Type) =>
            double.Parse(Get(Entities.Exercise.LOW_MET, Entities.Exercise.Name, NameOf(Type)));

        /// <summary>
        /// Gets the medium MET value for a specific <see cref="ExerciseType"/>.
        /// </summary>
        /// <param name="Type">The type of exercise.</param>
        /// <returns>The medium MET value for the specified <see cref="ExerciseType"/>.</returns>
        public static double Medium_MET(ExerciseType Type) =>
            double.Parse(Get(Entities.Exercise.MEDIUM_MET, Entities.Exercise.Name, NameOf(Type)));

        /// <summary>
        /// Gets the extreme MET value for a specific <see cref="ExerciseType"/>.
        /// </summary>
        /// <param name="Type">The type of exercise.</param>
        /// <returns>The extreme MET value for the specified <see cref="ExerciseType"/>.</returns>
        public static double Extreme_MET(ExerciseType Type) =>
            double.Parse(Get(Entities.Exercise.EXTREME_MET, Entities.Exercise.Name, NameOf(Type)));

        #region Equality and Operator Overloads

        /// <summary>
        /// Compares two <see cref="Exercise"/> instances for equality.
        /// </summary>
        /// <param name="a">The first exercise.</param>
        /// <param name="b">The second exercise.</param>
        /// <returns><c>true</c> if the exercises are equal; otherwise, <c>false</c>.</returns>
        public static bool operator == (Exercise a, Exercise b)
        {
            if (a is null & b is null)
                return true;
            //only one value will be null in this state
            else if (a is null || b is null)
                return false;
            return a.Id == b.Id;
        }

        /// <summary>
        /// Compares two <see cref="Exercise"/> instances for inequality.
        /// </summary>
        /// <param name="a">The first exercise.</param>
        /// <param name="b">The second exercise.</param>
        /// <returns><c>true</c> if the exercises are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator != (Exercise a, Exercise b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current <see cref="Exercise"/> instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><c>true</c> if the specified object is an <see cref="Exercise"/> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(Object obj)
        {
            if (obj is Exercise Exercise)
                return this == Exercise;
            else
                return false;
        }

        /// <summary>
        /// Returns a hash code for the current <see cref="Exercise"/> instance.
        /// </summary>
        /// <returns>A hash code for the current <see cref="Exercise"/> instance.</returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        #endregion

        #region Read

        /// <summary>
        /// Retrieves the value of a specified attribute for the current <see cref="Exercise"/> instance.
        /// </summary>
        /// <param name="Wanted_attribute">The attribute to retrieve.</param>
        /// <returns>The value of the specified attribute.</returns>
        private string Get(Attribute Wanted_attribute) => 
            GetById(Wanted_attribute, this.Id);

        /// <summary>
        /// Retrieves a list of all exercises.
        /// </summary>
        /// <returns>A list of <see cref="Exercise"/> instances representing all exercises.</returns>
        public static List<Exercise> GetAll()
        {
            List<Exercise> Exercises = new List<Exercise>();

            foreach (var id in GetAll(Entities.Exercise.Id))
                Exercises.Add(new Exercise(int.Parse(id)));

            return Exercises;
        }

        #endregion
    }
}