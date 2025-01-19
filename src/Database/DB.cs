using FitTrack.Core;
using FitTrack.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;

namespace FitTrack.Database
{
    /// <summary>
    /// Represents the database operations and management for the application.
    /// </summary>
    class DB : ServerBase
    {
        private class ExerciseMET
        {
            public double LOW { get; set; }
            public double MED { get; set; }
            public double EXTREME { get; set; }
            public string LOW_EXPRESSION { get; set; }
            public string MED_EXPRESSION { get; set; }
            public string EXTREME_EXPRESSION { get; set; }
        }
        private static readonly Dictionary<string, ExerciseMET> ExerciseDictionary = new Dictionary<string, ExerciseMET>
        {
            { "Cycling", new ExerciseMET { LOW = 4, MED = 8, EXTREME = 10, LOW_EXPRESSION="Leisurely (<10 mph)", MED_EXPRESSION="10-12 mph", EXTREME_EXPRESSION="14-16 mph" } },
            { "Swimming", new ExerciseMET { LOW = 5, MED = 7, EXTREME = 9, LOW_EXPRESSION="Leisurely", MED_EXPRESSION="General (Moderate Effort)", EXTREME_EXPRESSION="Fast (Vigorous Effort)" } },
            { "Walking", new ExerciseMET { LOW = 2.8, MED = 3.3, EXTREME = 5, LOW_EXPRESSION="2.0 mph", MED_EXPRESSION="3.0 mph", EXTREME_EXPRESSION="4.0 mph" } },
            { "Dancing", new ExerciseMET { LOW = 3, MED = 4.5, EXTREME = 6.5, LOW_EXPRESSION="Ballroom, Slow", MED_EXPRESSION="Ballroom, Fast", EXTREME_EXPRESSION="Aerobic, Ballet or Modern, Twist" } },
            { "ClimbingStairs", new ExerciseMET { LOW = 4, MED = 8, EXTREME = 15, LOW_EXPRESSION="Slow Pace", MED_EXPRESSION="Normal Pace", EXTREME_EXPRESSION="Fast Pace" } },
            { "Weightlifting", new ExerciseMET { LOW = 3, MED = 6, EXTREME = 8, LOW_EXPRESSION="General (Light Workout)", MED_EXPRESSION="Vigorous effort (Free weights)", EXTREME_EXPRESSION="Power lifting (Vigorous Effort)" } },
            { "Running", new ExerciseMET { LOW = 8.3, MED = 9.8, EXTREME = 11.5, LOW_EXPRESSION="Light Running (Jogging, 5 mph)", MED_EXPRESSION="Moderate Running (6 mph)", EXTREME_EXPRESSION="Vigorous Running (7.5 mph" } }
        };

        /// <summary>
        /// Inserts exercise data into the database.
        /// </summary>
        protected static void SeedExercises()
        {
            foreach (String key in ExerciseDictionary.Keys)
            {
                ExecuteNonQuery($"INSERT INTO Exercise(Name,LOW_MET,MEDIUM_MET,EXTREME_MET,LOW_MET_EXPRESSION,MEDIUM_MET_EXPRESSION,EXTREME_MET_EXPRESSION) " +
                    $"VALUES ('{key}','{ExerciseDictionary[key].LOW}','{ExerciseDictionary[key].MED}','{ExerciseDictionary[key].EXTREME}'," +
                    $"'{ExerciseDictionary[key].LOW_EXPRESSION}','{ExerciseDictionary[key].MED_EXPRESSION}','{ExerciseDictionary[key].EXTREME_EXPRESSION}');");
            }
            Trace.WriteLine("Seeding exercises successful.");
        }


        /// <summary>
        /// Seeds the database with initial account profiles.
        /// </summary>
        protected static void SeedProfile()
        {
            List<AccountData> AccountDatas = new List<AccountData>()
            {
                new AccountData(){ Id=GenerateNewAccountId() , Username="admin", Password="admin", FirstName="Application", LastName="Administrator", Email="fittrack@administration.com" },
                new AccountData(){ Id=GenerateNewAccountId() , Username="user", Password="Password", LastName="User", Email="user@email.com" }
            };

            foreach (var account in AccountDatas)
            {
                Account LoginAccount = Account.Create(account.Username, account.Password);
                LoginAccount.Email = account.Email;
                LoginAccount.FirstName = account.FirstName;
                LoginAccount.LastName = account.LastName;
            }

            Trace.WriteLine("Seeding profiles successful.");

            //Delete login data used for seeding
            LocalStorage.DeviceId = LocalStorage.LoginToken = null;
        }

        /// <summary>
        /// Initializes the database by creating necessary tables.
        /// </summary>
        /// <exception cref="Exceptions.DatabaseInitiationFailedException">Thrown when the database initiation fails.</exception>
        protected static void InitiateDatabase()
        {
            Trace.WriteLine("Initiating Database Entities");

            try
            {
                ExecuteNonQuery("CREATE TABLE Account(" +
                                    "Id NVARCHAR(8) PRIMARY KEY, " +
                                    "Username NVARCHAR(50) NOT NULL UNIQUE, " +
                                    "PasswordHash NVARCHAR(64) NOT NULL, " +
                                    "LoginTokenHash NVARCHAR(64) UNIQUE, " +
                                    "Email NVARCHAR(100), " +
                                    "FirstName NVARCHAR(20), " +
                                    "LastName NVARCHAR(20), " +
                                    "DateOfBirth DATE, " +
                                    "Gender CHAR(1) DEFAULT 'N', " +
                                    "Weight FLOAT DEFAULT 0," +
                                    "Active_Challenge_Id BIGINT DEFAULT 0, " +
                                    "Created_at DATETIME DEFAULT GETUTCDATE(), " +
                                    "Updated_at DATETIME DEFAULT GETUTCDATE()); ");

                ExecuteNonQuery("CREATE TABLE Exercise ( " +
                                    "Id INT PRIMARY KEY IDENTITY(1,1), " +
                                    "Name NVARCHAR(50) NOT NULL, " +
                                    "LOW_MET FLOAT, " +
                                    "LOW_MET_Expression NVARCHAR(100), " +
                                    "MEDIUM_MET FLOAT, " +
                                    "MEDIUM_MET_Expression NVARCHAR(100), " +
                                    "EXTREME_MET FLOAT, " +
                                    "EXTREME_MET_EXPRESSION NVARCHAR(100), " +
                                    "Created_at DATETIME DEFAULT GETUTCDATE(), " +
                                    "Updated_at DATETIME DEFAULT GETUTCDATE(), " +
                                    "Description NVARCHAR(255)); ");

                ExecuteNonQuery("CREATE TABLE Challenge ( " +
                                    "Id BIGINT PRIMARY KEY, " +
                                    "UserId NVARCHAR(8) NOT NULL, " +
                                    "Name NVARCHAR(30), " +
                                    "Calories_Goal FLOAT, " +
                                    "Progressed_Calories FLOAT, " +
                                    "To_reach_at DATETIME, " +
                                    "Finished_at DATETIME, " +
                                    "Created_at DATETIME DEFAULT GETUTCDATE(), " +
                                    "Updated_at DATETIME DEFAULT GETUTCDATE(), " +
                                    "FOREIGN KEY (UserId) REFERENCES Account(Id)) ");

                ExecuteNonQuery("CREATE TABLE Activity ( " +
                                    "Id BIGINT PRIMARY KEY, " +
                                    "ChallengeId BIGINT NOT NULL, " +
                                    "ExerciseId INT NOT NULL, " +
                                    "Weight FLOAT DEFAULT 0, " +
                                    "Duration FLOAT, " +
                                    "Burned_Calories FLOAT, " +
                                    "Created_at DATETIME DEFAULT GETUTCDATE(), " +
                                    "Updated_at DATETIME DEFAULT GETUTCDATE(), " +
                                    "FOREIGN KEY (ChallengeId) REFERENCES Challenge(Id), " +
                                    "FOREIGN KEY (ExerciseId) REFERENCES Exercise(Id));");

                ExecuteNonQuery("CREATE TABLE SessionDevice ( " +
                                    "Id NVARCHAR(16) PRIMARY KEY, " +
                                    "DeviceName NVARCHAR(MAX) NOT NULL," +
                                    "OSInformation NVARCHAR(MAX) NOT NULL," +
                                    "SignInCoolDown DATETIME," +
                                    "Created_at DATETIME DEFAULT GETUTCDATE()," +
                                    "Updated_at DATETIME DEFAULT GETUTCDATE());");

                ExecuteNonQuery("CREATE TABLE Session ( " +
                                    "Id BIGINT PRIMARY KEY IDENTITY(1,1)," +
                                    "DeviceId NVARCHAR(16) NOT NULL, " +
                                    "AccountId NVARCHAR(8), " +
                                    "SessionType NVARCHAR(50) NOT NULL, " +
                                    "SessionTime DATETIME DEFAULT GETUTCDATE()," +
                                    "FOREIGN KEY (DeviceId) REFERENCES SessionDevice(Id)," +
                                    "FOREIGN KEY (AccountId) REFERENCES Account(Id));");
            }
            catch (SqlException e) 
            { 
                throw new Exceptions.DatabaseInitiationFailedException(e.Message, e.InnerException); 
            }

            Trace.WriteLine("Database Entities Initiation Successful.");

            if (LocalStorage.DeviceId is null || !SessionDevice.Find(LocalStorage.DeviceId))
                //Assign new registered SessionDevice              
                LocalStorage.SessionDevice = SessionDevice.Register();
        }

        /// <summary>
        /// Resets the database by dropping all tables and reinitializing them.
        /// </summary>
        protected static void ResetDatabase()
        {
            Trace.WriteLine("Initiated Database Reset Procedures.");
            try { DeleteDatabase(); }catch(SqlException e) { throw new Exceptions.DatabaseFailedToDropEntityException(e.Message,e.InnerException); }
            InitiateDatabase();
            SeedProfile();
            SeedExercises();
            Trace.WriteLine("Database Reset Process Successful.");
        }

        private static void DeleteDatabase()
        {
            if (DoesTableExist(Entities.Table.Session))
                ExecuteNonQuery("DROP TABLE Session;");

            if (DoesTableExist(Entities.Table.SessionDevice))
                ExecuteNonQuery("DROP TABLE SessionDevice;");

            if (DoesTableExist(Entities.Table.Activity))
                ExecuteNonQuery("DROP TABLE Activity;");

            if (DoesTableExist(Entities.Table.Challenge))
                ExecuteNonQuery("DROP TABLE Challenge;");

            if (DoesTableExist(Entities.Table.Exercise))
                ExecuteNonQuery("DROP TABLE Exercise;");

            if (DoesTableExist(Entities.Table.Account))
                ExecuteNonQuery("DROP TABLE Account;");
        }

        private class AccountData
        {
            public string Id;
            public string Username;
            public string Password;
            public string FirstName;
            public string LastName;
            public string Email;
        }
    }
}