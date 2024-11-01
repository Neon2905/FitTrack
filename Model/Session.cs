using System;
using FitTrack.Database;
using FitTrack.Exceptions;
using Attribute = FitTrack.Database.Entities.Session;
using FitTrack.Core;

namespace FitTrack.Model
{
    /// <summary>
    /// Represents a session in the system, including session details and operations such as account register and account sign in.
    /// </summary>
    class Session : ServerBase
    {
        #region Properties

        /// <summary>
        /// Gets the unique identifier of the session.
        /// </summary>
        private readonly int id;

        /// <summary>
        /// Gets the device ID associated with the session.
        /// </summary>
        private string DeviceId => Get(Attribute.DeviceId);

        /// <summary>
        /// Gets the <see cref="SessionDevice"/> associated with the session.
        /// </summary>
        public SessionDevice Device => new SessionDevice(DeviceId);

        /// <summary>
        /// Gets the date and time when the session is registered.
        /// </summary>
        public DateTime SessionTime => DateTime.Parse(Get(Attribute.SessionTime));

        /// <summary>
        /// Gets the date and time of the <see cref="SessionTime"/> in local time, formatted as "dd MMMM, yyyy-HH:mm".
        /// </summary>
        public string SessionTimeLocal => SessionTime.ToLocalTime().ToString("dd MMMM, yyyy-HH:mm");

        /// <summary>
        /// Gets the session type as a <see cref="string"/>.
        /// </summary>
        public string SessionTypeString => Get(Attribute.SessionType);

        /// <summary>
        /// Gets the session type as an enumeration value based on the <see cref="SessionTypeString"/>.
        /// </summary>
        public SessionKind SessionType
        {
            get
            {
                var sessionType = Get(Attribute.SessionType);
                switch (sessionType.ToString())
                {
                    case "SignIn":
                        return SessionKind.SignIn;
                    case "SignOut":
                        return SessionKind.SignOut;
                    case "FailedSignInAttempt":
                        return SessionKind.FailedSignInAttempt;
                    default:
                        return SessionKind.Unknown;
                }
            }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="Session"/> class with the specified identifier.
        /// </summary>
        /// <param name="Id">The unique identifier of the session.</param>
        /// <exception cref="ObjectNotFoundInDatabaseException">Thrown when the session with the specified identifier does not exist in the database.</exception>
        public Session(int Id)
        {
            if (Exists(Attribute.Id, Id))
                this.id = Id;
            else
                throw new ObjectNotFoundInDatabaseException($"Session with id:{Id} does not exists", null, Id, Entities.Table.Session);
        }

        #region Equality and Operator Overloads

        /// <summary>
        /// Compares two <see cref="Session"/> instances for equality.
        /// </summary>
        /// <param name="a">The first session.</param>
        /// <param name="b">The second session.</param>
        /// <returns><c>true</c> if the sessions are equal; otherwise, <c>false</c>.</returns>
        public static bool operator == (Session a, Session b)
        {
            if (a is null & b is null)
                return true;
            //only one value will be null in this state
            else if (a is null || b is null)
                return false;
            return a.id == b.id;
        }

        /// <summary>
        /// Compares two <see cref="Session"/> instances for inequality.
        /// </summary>
        /// <param name="a">The first session.</param>
        /// <param name="b">The second session.</param>
        /// <returns><c>true</c> if the sessions are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator != (Session a, Session b) =>
            !(a == b);

        /// <summary>
        /// Determines whether the specified object is equal to the current <see cref="Session"/> instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><c>true</c> if the specified object is a <see cref="Session"/> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(Object obj)
        {
            if (obj is Session session)
                return this == session;
            else
                return false;
        }

        /// <summary>
        /// Returns a hash code for the current <see cref="Session"/> instance.
        /// </summary>
        /// <returns>A hash code for the current <see cref="Session"/> instance.</returns>
        public override int GetHashCode() =>
            id.GetHashCode();

        #endregion

        #region Create

        /// <summary>
        /// Registers a new session with the specified session type and optional account ID. This method uses the session device stored in local storage as the initiator of the new session.
        /// </summary>
        /// <param name="SessionType">The type of the session to register. It determines the nature of the session (e.g., sign-in, sign-out, or failed sign-in attempt).</param>
        /// <param name="AccountId">Optional. The account ID associated with the session. If not provided, the session is registered without an associated account ID.</param>
        /// <exception cref="InvalidSessionDeviceException">Thrown when the session device is not valid or not found in the system.
        /// This occurs if the device ID is not set in local storage or if the device cannot be found in the system.</exception>
        /// <remarks>
        /// This method performs the following operations:
        /// <list type="bullet">
        /// <item>Checks if a valid session device is registered in local storage. If not, it throws an <see cref="InvalidSessionDeviceException"/>.</item>
        /// <item>Create a new session record into the database. It registers the device or session initiator, session type, and optionally, an account ID if provided.</item>
        /// <item>After inserting the new session record, it checks if the device has had more than eight recent failed sign-in attempts. If so, it triggers a sign-in cooldown for the device.</item>
        /// </list>
        /// </remarks>
        public static void Register(SessionKind SessionType, string AccountId=null)
        {
            //Return with exception if system's session device is not valid
            if (LocalStorage.DeviceId is null || !SessionDevice.Find(LocalStorage.DeviceId))
                throw new InvalidSessionDeviceException("No legal session device registered or found in system.", null, LocalStorage.SessionDevice);

            string Query;

            if(AccountId == null)
                Query = $"INSERT INTO {NameOf(Entities.Table.Session)} ({NameOf(Attribute.DeviceId)},{NameOf(Attribute.SessionType)}) " +
                    $"VALUES ('{LocalStorage.SessionDevice.Id}','{NameOf(SessionType)}');";
            else
                Query = $"INSERT INTO {NameOf(Entities.Table.Session)} ({NameOf(Attribute.DeviceId)},AccountId,{NameOf(Attribute.SessionType)}) " +
                    $"VALUES ('{LocalStorage.SessionDevice.Id}','{AccountId}','{NameOf(SessionType)}');";
            ExecuteNonQuery(Query);

            //Check if there's MultipleRecentFailedSignInLog on Device
            if (LocalStorage.SessionDevice.RecentFailedLoginAttemptSessions.Count > Rules.MaximumLoginAttempts)
                LocalStorage.SessionDevice.AddSignInCoolDown();
        }

        #endregion

        #region Read

        /// <summary>
        /// Retrieves the value of a specified attribute for the current <see cref="Session"/> instance.
        /// </summary>
        /// <param name="Attribute">The attribute to retrieve.</param>
        /// <returns>The value of the specified attribute.</returns>
        private string Get(Attribute Attribute) => 
            GetById(Attribute, this.id);

        #endregion

        #region Delete

        /// <summary>
        /// Deletes the current <see cref="Session"/> instance from the database.
        /// </summary>
        public void Delete()=>
            DeleteById(Entities.Table.Session,this.id);

        #endregion
    }
}