namespace FitTrack.Database
{
    /// <summary>
    /// Contains enums representing the database tables and their columns.
    /// </summary>
    public class Entities
    {
        /// <summary>
        /// Represents the database tables.
        /// </summary>
        public enum Table
        {
            /// <summary>
            /// The Account table.
            /// </summary>
            Account,
            /// <summary>
            /// The Exercise table.
            /// </summary>
            Exercise,
            /// <summary>
            /// The Activity table.
            /// </summary>
            Activity,
            /// <summary>
            /// The Challenge table.
            /// </summary>
            Challenge,
            /// <summary>
            /// The SessionDevice table.
            /// </summary>
            SessionDevice,
            /// <summary>
            /// The Session table.
            /// </summary>
            Session
        }

        /// <summary>
        /// Represents the columns in the Account table.
        /// </summary>
        public enum Account
        {
            /// <summary>
            /// The Id column.
            /// </summary>
            Id,
            /// <summary>
            /// The Username column.
            /// </summary>
            Username,
            /// <summary>
            /// The PasswordHash column.
            /// </summary>
            PasswordHash,
            /// <summary>
            /// The LoginTokenHash column.
            /// </summary>
            LoginTokenHash,
            /// <summary>
            /// The Email column.
            /// </summary>
            Email,
            /// <summary>
            /// The FirstName column.
            /// </summary>
            FirstName,
            /// <summary>
            /// The LastName column.
            /// </summary>
            LastName,
            /// <summary>
            /// The DateOfBirth column.
            /// </summary>
            DateOfBirth,
            /// <summary>
            /// The Gender column.
            /// </summary>
            Gender,
            /// <summary>
            /// The Weight column.
            /// </summary>
            Weight,
            /// <summary>
            /// The Active_Challenge_Id column.
            /// </summary>
            Active_Challenge_Id,
            /// <summary>
            /// The Created_at column.
            /// </summary>
            Created_at,
            /// <summary>
            /// The Updated_at column.
            /// </summary>
            Updated_at
        }

        /// <summary>
        /// Represents the columns in the Exercise table.
        /// </summary>
        public enum Exercise
        {
            /// <summary>
            /// The Id column.
            /// </summary>
            Id,
            /// <summary>
            /// The Name column.
            /// </summary>
            Name,
            /// <summary>
            /// The LOW_MET column.
            /// </summary>
            LOW_MET,
            /// <summary>
            /// The LOW_MET_EXPRESSION column.
            /// </summary>
            LOW_MET_EXPRESSION,
            /// <summary>
            /// The MEDIUM_MET column.
            /// </summary>
            MEDIUM_MET,
            /// <summary>
            /// The MEDIUM_MET_EXPRESSION column.
            /// </summary>
            MEDIUM_MET_EXPRESSION,
            /// <summary>
            /// The EXTREME_MET column.
            /// </summary>
            EXTREME_MET,
            /// <summary>
            /// The EXTREME_MET_EXPRESSION column.
            /// </summary>=
            EXTREME_MET_EXPRESSION,
            /// <summary>
            /// The Description column.
            /// </summary>
            Description,
            /// <summary>
            /// The Created_at column.
            /// </summary>
            Created_at,
            /// <summary>
            /// The Updated_at column.
            /// </summary>
            Updated_at
        }

        /// <summary>
        /// Represents the columns in the Activity table.
        /// </summary>
        public enum Activity
        {
            /// <summary>
            /// The Id column.
            /// </summary>
            Id,
            /// <summary>
            /// The ChallengeId column.
            /// </summary>
            ChallengeId,
            /// <summary>
            /// The ExerciseId column.
            /// </summary>
            ExerciseId,
            /// <summary>
            /// The Weight column.
            /// </summary>
            Weight,
            /// <summary>
            /// The Duration column.
            /// </summary>
            Duration,
            /// <summary>
            /// The Burned_Calories column.
            /// </summary>
            Burned_Calories,
            /// <summary>
            /// The Created_at column.
            /// </summary>
            Created_at,
            /// <summary>
            /// The Updated_at column.
            /// </summary>
            Updated_at
        }

        /// <summary>
        /// Represents the columns in the Challenge table.
        /// </summary>
        public enum Challenge
        {
            /// <summary>
            /// The Id column.
            /// </summary>
            Id,
            /// <summary>
            /// The UserId column.
            /// </summary>
            UserId,
            /// <summary>
            /// The Name column.
            /// </summary>
            Name,
            /// <summary>
            /// The Calories_Goal column.
            /// </summary>
            Calories_Goal,
            /// <summary>
            /// The Progressed_Calories column.
            /// </summary>
            Progressed_Calories,
            /// <summary>
            /// The To_Reach_at column.
            /// </summary>
            To_Reach_at,
            /// <summary>
            /// The Finished_at column.
            /// </summary>
            Finished_at,
            /// <summary>
            /// The Created_at column.
            /// </summary>
            Created_at,
            /// <summary>
            /// The Updated_at column.
            /// </summary>
            Updated_at
        }

        /// <summary>
        /// Represents the columns in the SessionDevice table.
        /// </summary>
        public enum SessionDevice
        {
            /// <summary>
            /// The Id column.
            /// </summary>
            Id,
            /// <summary>
            /// The DeviceName column.
            /// </summary>
            DeviceName,
            /// <summary>
            /// The OSInformation column.
            /// </summary>
            OSInformation,
            /// <summary>
            /// The SignInCoolDown column.
            /// </summary>
            SignInCoolDown,
            /// <summary>
            /// The Created_at column.
            /// </summary>
            Created_at,
            /// <summary>
            /// The Updated_at column.
            /// </summary>
            Updated_at
        }

        /// <summary>
        /// Represents the columns in the Session table.
        /// </summary>
        public enum Session
        {
            /// <summary>
            /// The Id column.
            /// </summary>
            Id,
            /// <summary>
            /// The DeviceId column.
            /// </summary>
            DeviceId,
            /// <summary>
            /// The AccountId column.
            /// </summary>
            AccountId,
            /// <summary>
            /// The SessionTime column.
            /// </summary>
            SessionTime,
            /// <summary>
            /// The SessionType column.
            /// </summary>
            SessionType
        }
    }
}
