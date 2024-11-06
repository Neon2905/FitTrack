using System;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using FitTrack.Model;
using System.Text;
using FitTrack.Core;
using FitTrack.Exceptions;

namespace FitTrack.Database
{
    /// <summary>
    /// Provides methods for database operations such as querying, inserting, updating, and deleting records.
    /// </summary>
    class ServerBase
    {
        /// <summary>
        /// The connection string for the database.
        /// </summary>
        private static readonly String ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + SystemInfo.ProjectDirectory + @"\Database\Database.mdf;Integrated Security=True";

        private static readonly char[] TokenAllowedCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
        private static readonly int AccountIdLengthLimit = 8;
        private static readonly int DeviceIdLengthLimit = 16;
        private static readonly int LoginTokenLengthLimit = 16;

        /// <summary>
        /// Executes a SQL query and returns the result as a DataTable.
        /// </summary>
        /// <param name="SQLQuery">The SQL query to execute.</param>
        /// <returns>A DataTable containing the query results.</returns>
        private static DataTable ExecuteQuery(string SQLQuery)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(SQLQuery, connection);
                new SqlDataAdapter(cmd).Fill(dataTable);
            }
            return dataTable;
        }

        /// <summary>
        /// Executes a SQL non-query command.
        /// </summary>
        /// <param name="SQLQuery">The SQL query to execute.</param>
        internal static void ExecuteNonQuery(string SQLQuery)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(SQLQuery, connection);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Generates a new unique account ID with length of <see cref="AccountIdLengthLimit"/>.
        /// </summary>
        /// <returns>A unique account ID.</returns>
        internal static string GenerateNewAccountId()
        {
            var AccountId = GenerateRandomString(AccountIdLengthLimit);

            //generate random string set till unique in Database
            while (Exists(Entities.Account.Id, AccountId))
                AccountId = GenerateRandomString(AccountIdLengthLimit);

            return AccountId;
        }

        /// <summary>
        /// Generates a new unique device ID with length of <see cref="DeviceIdLengthLimit"/>.
        /// </summary>
        /// <returns>A unique device ID.</returns>
        internal static string GenerateNewDeviceId()
        {
            var DeviceId = GenerateRandomString(DeviceIdLengthLimit);

            while (Exists(Entities.SessionDevice.Id, DeviceId))
                DeviceId = GenerateRandomString(DeviceIdLengthLimit);

            return DeviceId;
        }

        /// <summary>
        /// Generates a new unique login token with length of <see cref="LoginTokenLengthLimit"/>.
        /// </summary>
        /// <returns>A unique login token.</returns>
        internal static string GenerateNewLoginToken()
        {
            var LoginToken = GenerateRandomString(LoginTokenLengthLimit);

            while (Exists(Entities.Account.LoginTokenHash, GetHash(LoginToken)))
                LoginToken = GenerateRandomString(LoginTokenLengthLimit);

            return LoginToken;
        }

        /// <summary>
        /// Checks if a value exists in the specified column.
        /// </summary>
        /// <param name="column">Column to check.</param>
        /// <param name="value">Value to check for.</param>
        /// <returns>True if the value exists, otherwise false.</returns>
        internal static bool Exists(Enum column, object value)
        {
            //SELECT @column_name FROM @table_name WHERE @column_name = '@value'
            string Query = $"SELECT {NameOf(column)} FROM {EntityNameOf(column)} WHERE {NameOf(column)} = '{value}'";
            return ExecuteQuery(Query).Rows.Count > 0;
        }

        /// <summary>
        /// Checks if a table exists in the database.
        /// </summary>
        /// <param name="tableName">The name of the table to check.</param>
        /// <returns>True if the table exists, otherwise false.</returns>
        internal static bool DoesTableExist(Entities.Table tableName)
        {
            string Query = @"
                SELECT 1
                FROM INFORMATION_SCHEMA.TABLES 
                WHERE TABLE_SCHEMA = 'dbo' 
                AND TABLE_NAME = '" + NameOf(tableName) + "'";

            DataTable result = ExecuteQuery(Query);

            return result.Rows.Count > 0;
        }

        #region Create

        /// <summary>
        /// Creates a new Activity record in the database.
        /// </summary>
        /// <param name="ChallengeId">The parent challenge's ID associated with the activity.</param>
        /// <param name="ExerciseId">The exercise ID associated with the activity.</param>
        /// <returns>The newly created activity.</returns>
        internal static Activity CreateActivity(int ChallengeId, int ExerciseId)
        {
            int new_id = GetLastId(Entities.Table.Activity) + 1; // Create an available id

            ExecuteNonQuery($"INSERT INTO Activity(Id,ChallengeId,ExerciseId) VALUES ('{new_id}','{ChallengeId}','{ExerciseId}');");
            return new Activity(new_id);
        }

        /// <summary>
        /// Creates a new challenge record in the database.
        /// </summary>
        /// <param name="UserId">The user ID associated with the challenge.</param>
        /// <param name="Calories_goal">The calories goal for the challenge.</param>
        /// <param name="Goal_date">The optional goal date for the challenge.</param>
        /// <returns>The newly created challenge.</returns>
        internal static Challenge CreateChallenge(string UserId, double Calories_goal, DateTime? Goal_date)
        {
            int new_id = GetLastId(Entities.Table.Challenge) + 1; // Create an available id

            ExecuteNonQuery($"INSERT INTO Challenge(id,userId,calories_goal) VALUES ('{new_id}','{UserId}','{Calories_goal}');");
            Challenge challenge = new Challenge(new_id);
            if (Goal_date != null)
                challenge.To_Reach_At = Goal_date;
            return challenge;
        }
        #endregion

        #region Read

        /// <summary>
        /// Retrieves all records from a specified table.
        /// </summary>
        /// <param name="table">The table to retrieve records from.</param>
        /// <returns>A DataTable containing all records from the specified table.</returns>
        internal static DataTable GetAll(Entities.Table table)
        {
            string Query = $"SELECT * FROM {NameOf(table)}";
            DataTable result = ExecuteQuery(Query);
            return result.Rows.Count > 0 ? result : null;
        }

        /// <summary>
        /// Retrieves all records from a specified column that match a search value.
        /// </summary>
        /// <param name="WantedAttribute">The column to retrieve.</param>
        /// <param name="SearchAttribute">The column to search for matched value.</param>
        /// <param name="SearchValue">The value to search for.</param>
        /// <returns>A list of matching records.</returns>
        internal static List<string> GetAll(Enum WantedAttribute, Enum SearchAttribute, object SearchValue)
        {
            string Query = $"SELECT {NameOf(WantedAttribute)} FROM {EntityNameOf(WantedAttribute)} WHERE {NameOf(SearchAttribute)} = '{SearchValue}'";

            List<string> result = new List<string>();
            foreach (DataRow row in ExecuteQuery(Query).Rows)
            {
                result.Add(row[0].ToString());
            }
            return result;
        }

        /// <summary>
        /// Retrieves all records from a specified column.
        /// </summary>
        /// <param name="WantedAttribute">The column to retrieve.</param>
        /// <returns>A list of all records from the specified column.</returns>
        internal static List<string> GetAll(Enum WantedAttribute)
        {
            string Query = $"SELECT {NameOf(WantedAttribute)} FROM {EntityNameOf(WantedAttribute)}";
            List<string> result = new List<string>();
            foreach (DataRow row in ExecuteQuery(Query).Rows)
                result.Add(row[0].ToString());

            return result;
        }

        /// <summary>
        /// Retrieves a record by ID from a specified table.
        /// </summary>
        /// <param name="Table">The table to retrieve the record from.</param>
        /// <param name="Id">The ID of the record to retrieve.</param>
        /// <returns>The DataRow of the retrieved record.</returns>
        internal static DataRow Get(Entities.Table Table, object Id)
        {
            DataTable result = ExecuteQuery($"SELECT * FROM {NameOf(Table)} WHERE id = '{Id}'");
            return result.Rows.Count > 0 ? result.Rows[0] : null;
        }

        /// <summary>
        /// Retrieves a record by a specified search attribute and value.
        /// </summary>
        /// <param name="SearchAttribute">The column to search.</param>
        /// <param name="Value">The value to search for.</param>
        /// <returns>The DataRow of the retrieved record.</returns>
        internal static DataRow Get(Enum SearchAttribute, object Value)
        {
            DataTable result = ExecuteQuery($"SELECT * FROM {EntityNameOf(SearchAttribute)} WHERE {NameOf(SearchAttribute)} = '{Value}'");
            return result.Rows.Count > 0 ? result.Rows[0] : null;
        }

        /// <summary>
        /// Retrieves a value by ID from a specified column.
        /// </summary>  
        /// <param name="WantedAttribute">The column to retrieve the value from.</param>
        /// <param name="Id">The ID to search for.</param>
        /// <returns>The retrieved value as a string.</returns>
        internal static string GetById(Enum WantedAttribute, object Id)
        {
            //Filters null values
            if (Id is null || Id is 0)
                throw new InvalidEntityAccessException($"Read-Operation failed due to illegal Id value:{Id}.", null, WantedAttribute);

            DataTable result = ExecuteQuery($"SELECT {NameOf(WantedAttribute)} FROM {EntityNameOf(WantedAttribute)} WHERE id = '{Id}'");
            return result.Rows.Count > 0 ? result.Rows[0][NameOf(WantedAttribute)].ToString() : null;
        }

        /// <summary>
        /// Retrieves a value by a specified search attribute and value.
        /// </summary>
        /// <param name="WantedAttribute">The column to retrieve the value from.</param>
        /// <param name="SearchAttribute">The column to search.</param>
        /// <param name="SearchValue">The value to search for.</param>
        /// <returns>The retrieved value as a string.</returns>
        internal static string Get(Enum WantedAttribute, Enum SearchAttribute, object SearchValue)
        {
            string Query = $"SELECT {NameOf(WantedAttribute)} FROM {EntityNameOf(WantedAttribute)} WHERE {NameOf(SearchAttribute)} = '{SearchValue}'";
            return ExecuteQuery(Query).Rows[0][0].ToString();
        }

        /// <summary>
        /// Retrieves the last ID from a specified table.
        /// </summary>
        /// <param name="Table">The table to retrieve the last ID from.</param>
        /// <returns>The last ID as an integer.</returns>
        private static int GetLastId(Entities.Table Table)
        {
            string Query = $"SELECT MAX(id) FROM {NameOf(Table)}";
            string result = ExecuteQuery(Query).Rows[0][0].ToString();
            return string.IsNullOrEmpty(result) ? 0 : int.Parse(result);
        }
        #endregion

        #region Update

        /// <summary>
        /// Updates a record by ID with a specified attribute and value.
        /// Only updates when new value is not same as existing one.
        /// Also updates Entity.Updated_at attribute.
        /// </summary>
        /// <param name="Id">The ID of the record to update.</param>
        /// <param name="AttributeToUpdate">The attribute to update.</param>
        /// <param name="Value">The new value to set.</param>
        internal static void UpdateById(object Id, Enum AttributeToUpdate, object Value)
        {
            if (Id is null || Id is 0)
                throw new InvalidEntityAccessException($"Update-Operation failed due to illegal Id value:{Id}.", null, AttributeToUpdate);

            if(Value is string && Value.ToString().Contains("'"))
                //Handles syntax errors of Sql with single quote ( ' )
                Value = Value.ToString().Replace("'","''");

            //When the value to be updated is null, query will directly sets database attribute to null.
            string Query = $"UPDATE {EntityNameOf(AttributeToUpdate)} SET {NameOf(AttributeToUpdate)} = " + (Value != null ? $"'{Value}'" : "NULL") + $" WHERE id = '{Id}'";

            //Update only when value is not same as that of database
            if (!GetById(AttributeToUpdate, Id).Equals(Value?.ToString()))
            {
                //Update data Query
                ExecuteNonQuery(Query);
                //Update time-of-update (Updated_at)
                ExecuteNonQuery($"UPDATE {EntityNameOf(AttributeToUpdate)} SET updated_at = '{DateTime.UtcNow}' WHERE id = '{Id}'");
            }
        }
        #endregion

        #region Delete

        /// <summary>
        /// Deletes a record by ID from a specified table.
        /// </summary>
        /// <param name="Table">Table to delete the record from.</param>
        /// <param name="Id">ID of the record to delete.</param>
        internal static void DeleteById(Entities.Table Table, object Id)
        {
            if (Id is null || Id is 0)
                throw new InvalidEntityAccessException($"Delete-Operation failed due to illegal Id value:{Id}.", null, Table);

            string Query = $"DELETE FROM {NameOf(Table)} WHERE id = '{Id}'";
            ExecuteNonQuery(Query);
        }
        #endregion

        #region Utilities

        /// <summary>
        /// Computes the SHA-256 hash of a string.
        /// </summary>
        /// <param name="text">The text to hash.</param>
        /// <returns>The hash of the text as a string.</returns>
        internal static string GetHash(object text)
        {
            if (String.IsNullOrEmpty(text.ToString()))
                return String.Empty;

            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] textData = Encoding.UTF8.GetBytes(text.ToString());
                byte[] hash = sha.ComputeHash(textData);

                return BitConverter.ToString(hash).Replace("-", String.Empty);
            }
        }

        /// <summary>
        /// Generates a random string of a specified length.
        /// </summary>
        /// <param name="length">The length of the random string.</param>
        /// <returns>A random string of the specified length.</returns>
        private static string GenerateRandomString(int length)
        {
            Random Random = new Random();
            var stringBuilder = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                char randomChar = TokenAllowedCharacters[Random.Next(TokenAllowedCharacters.Length)];
                stringBuilder.Append(randomChar);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Retrieves the name of an enum value.
        /// </summary>
        /// <param name="Attribute">The enum value.</param>
        /// <returns>The name of the enum value.</returns>
        internal static string NameOf(Enum Attribute) =>
            Enum.GetName(Attribute.GetType(), Attribute);

        /// <summary>
        /// Retrieves the name of the entity type of an enum value.
        /// </summary>
        /// <param name="Table">The enum value representing a table.</param>
        /// <returns>The name of the entity type.</returns>
        internal static string EntityNameOf(Enum Table) =>
            Table.GetType().Name;
        #endregion
    }
}