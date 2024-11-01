using FitTrack.Core;
using FitTrack.Database;
using FitTrack.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Attribute = FitTrack.Database.Entities.SessionDevice;

namespace FitTrack.Model
{
    /// <summary>
    /// Represents a session device in the system. Provides properties and methods to manage and interact with session devices, including device-specific details.
    /// </summary>
    class SessionDevice : ServerBase
    {
        #region Properties
        private readonly object id;

        /// <summary>
        /// Gets the unique identifier of the session device.
        /// </summary>
        public object Id => this.id;

        /// <summary>
        /// Gets the name of the session device.
        /// </summary>
        public string DeviceName => Get(Attribute.DeviceName);

        /// <summary>
        /// Gets the operating system information of the session device.
        /// </summary>
        public string OSInformation => Get(Attribute.OSInformation);

        /// <summary>
        /// Gets the sign-in cooldown timestamp of the session device. Returns null if no cooldown is set.
        /// </summary>
        public DateTime? SignInCoolDown
        {
            get => Get(Attribute.SignInCoolDown).Length > 0 ? DateTime.Parse(Get(Attribute.SignInCoolDown)) : new DateTime?();
            private set => Update(Attribute.SignInCoolDown, value);
        }

        /// <summary>
        /// Gets the sign-in cooldown timestamp formatted in local time. Returns null if no cooldown is set.
        /// </summary>
        public string SignInCoolDownLocal => SignInCoolDown?.ToLocalTime().ToString("dd, MMMM yyyy, HH:mm");

        /// <summary>
        /// Determines whether the session device has an active sign-in cooldown.
        /// </summary>
        public bool HasSignInCoolDown => SignInCoolDown != null && SignInCoolDown > DateTime.UtcNow;

        /// <summary>
        /// Gets a read-only collection of sessions associated with the session device, ordered by <see cref="Session.SessionTime"/> in descending order.
        /// </summary>
        public ReadOnlyCollection<Session> Sessions
        {
            get
            {
                List<Session> sessions = new List<Session>();
                foreach (var sessionId in GetAll(Entities.Session.Id, Entities.Session.DeviceId, this.id))
                    sessions.Add(new Session(int.Parse(sessionId)));
                return new ReadOnlyCollection<Session>(sessions.OrderByDescending(x => x.SessionTime).ToList());
            }
        }

        /// <summary>
        /// Gets a read-only collection of failed login attempt sessions associated with the session device.
        /// </summary>
        public ReadOnlyCollection<Session> FailedLoginAttemptSessions => new ReadOnlyCollection<Session>(Sessions.Where(Session => Session.SessionType == SessionKind.FailedSignInAttempt).ToList());

        /// <summary>
        /// Gets a read-only collection of recent failed login attempt sessions (within the last 5 minutes) associated with the session device.
        /// </summary>
        public ReadOnlyCollection<Session> RecentFailedLoginAttemptSessions => new ReadOnlyCollection<Session>(FailedLoginAttemptSessions.Where(Session => Session.SessionTime > DateTime.UtcNow.AddMinutes(-5)).ToList());

        /// <summary>
        /// Gets the timestamp of when the session device was last updated.
        /// </summary>
        public DateTime Updated_at => DateTime.Parse(Get(Attribute.Updated_at));

        /// <summary>
        /// Gets the timestamp of when the session device was created.
        /// </summary>
        public DateTime Created_at => DateTime.Parse(Get(Attribute.Created_at));

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionDevice"/> class with the specified device ID.
        /// </summary>
        /// <param name="Id">The unique identifier of the session device.</param>
        /// <exception cref="ObjectNotFoundInDatabaseException">Thrown when the session device with the specified ID does not exist in the database.</exception>
        public SessionDevice(object Id)
        {
            if (Exists(Attribute.Id, Id))
                this.id = Id;
            else
                throw new ObjectNotFoundInDatabaseException($"SessionDevice with id:{Id} does not exists", null, Id, Entities.Table.SessionDevice);
        }

        /// <summary>
        /// Checks if a session device with the specified device ID exists in the database.
        /// </summary>
        /// <param name="DeviceId">The unique identifier of the session device to check.</param>
        /// <returns>True if the session device exists; otherwise, false.</returns>
        public static bool Find(object DeviceId) => 
            Exists(Attribute.Id, DeviceId);

        /// <summary>
        /// Adds a sign-in cooldown period to the session device. The cooldown period is set to <see cref="Rules.DurationPerCoolDown"/> minutes from the current UTC time.
        /// </summary>
        public void AddSignInCoolDown() =>
            SignInCoolDown = DateTime.UtcNow.AddMinutes(Rules.DurationPerCoolDown);

        #region Equality and Operator Overloads

        /// <summary>
        /// Determines whether two <see cref="SessionDevice"/> instances are equal.
        /// </summary>
        /// <param name="a">The first session device.</param>
        /// <param name="b">The second session device.</param>
        /// <returns>True if both session devices are equal; otherwise, false.</returns>
        public static bool operator == (SessionDevice a, SessionDevice b)
        {
            if (a is null & b is null)
                return true;
            //only one value will be null in this state
            else if (a is null || b is null)
                return false;
            return a.id == b.id;
        }

        /// <summary>
        /// Determines whether two <see cref="SessionDevice"/> instances are not equal.
        /// </summary>
        /// <param name="a">The first session device.</param>
        /// <param name="b">The second session device.</param>
        /// <returns>True if both session devices are not equal; otherwise, false.</returns>
        public static bool operator != (SessionDevice a, SessionDevice b) =>
            !(a == b);

        /// <summary>
        /// Determines whether the specified object is equal to the current <see cref="SessionDevice"/> instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current session device.</param>
        /// <returns>True if the specified object is equal to the current session device; otherwise, false.</returns>
        public override bool Equals(Object obj)
        {
            if (obj is SessionDevice sessionDevice)
                return this == sessionDevice;
            else
                return false;
        }

        /// <summary>
        /// Serves as a hash function for the <see cref="SessionDevice"/> class.
        /// </summary>
        /// <returns>A hash code for the current session device.</returns>
        public override int GetHashCode() =>
            id.GetHashCode();

        #endregion

        #region Create

        /// <summary>
        /// Registers a new session device in the system with generated device ID and system information.
        /// </summary>
        /// <returns>A new instance of the <see cref="SessionDevice"/> class with the registered device ID.</returns>
        public static SessionDevice Register()
        {
            var Id = GenerateNewDeviceId();

            ExecuteNonQuery($"INSERT INTO {NameOf(Entities.Table.SessionDevice)}({NameOf(Attribute.Id)},{NameOf(Attribute.DeviceName)},{NameOf(Attribute.OSInformation)}) " +
                            $"VALUES('{Id}', '{SystemInfo.DeviceName}', '{SystemInfo.OSInformation}');");

            return new SessionDevice(Id);
        }

        #endregion

        #region Read

        private string Get(Attribute wanted_attribute) =>
            GetById(wanted_attribute, this.id).ToString();

        #endregion

        #region Update

        /// <summary>
        /// Synchronizes the session device information with the cloud, updating the device name and OS information of the current device.
        /// </summary>
        public void SyncWithCloud()
        {
            Update(Attribute.DeviceName, SystemInfo.DeviceName);
            Update(Attribute.OSInformation, SystemInfo.OSInformation);
        }

        private void Update(Attribute Attribute, object value) => 
                UpdateById(this.id, Attribute, value);

        #endregion

        #region Delete

        /// <summary>
        /// Deletes the session device from the database.
        /// </summary>
        public void Delete() =>
            DeleteById(Entities.Table.SessionDevice, this.Id);

        #endregion
    }
}
