using FitTrack.Exceptions;
using FitTrack.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using FitTrack.Database;
using Attribute = FitTrack.Database.Entities.Account;
using FitTrack.Core;

namespace FitTrack.Model
{
    /// <summary>
    /// Represents an account from the database, providing methods and properties related to user account information.
    /// </summary>
    class Account : ServerBase
    {
        #region Properties

        private string id;

        /// <summary>
        /// Gets the username associated with the account.
        /// </summary>
        public string Username
        {
            get => Get(Attribute.Username);
            private set
            {
                Update(Attribute.Username, value);
                Session.Register(SessionKind.UsernameChange, this.id);
            }
        }

        private string PasswordHash
        {
            get => Get(Attribute.PasswordHash);
            set
            {
                Update(Attribute.PasswordHash, value);
                Session.Register(SessionKind.PasswordChange, this.id);
            }
        }

        private string LoginTokenHash
        {
            get => Get(Attribute.LoginTokenHash);
            set => Update(Attribute.LoginTokenHash, value);
        }

        /// <summary>
        /// Gets or sets the first name of the account holder.
        /// </summary>
        public string FirstName
        {
            get => Get(Attribute.FirstName);
            set => Update(Attribute.FirstName, value);
        }

        /// <summary>
        /// Gets or sets the last name of the account holder.
        /// </summary>
        public string LastName
        {
            get => Get(Attribute.LastName);
            set => Update(Attribute.LastName, value);
        }

        /// <summary>
        /// Gets the full name of the account holder, combining first and last names.
        /// </summary>
        public string Name => FirstName + " " + LastName;

        /// <summary>
        /// Gets or sets the email address associated with the account.
        /// </summary>
        public string Email
        {
            get => Get(Attribute.Email);
            set => Update(Attribute.Email, value);
        }

        /// <summary>
        /// Gets or sets the date of birth of the account holder. If not set, returns null.
        /// </summary>
        public DateTime? DateOfBirth
        {
            get
            {
                var value = Get(Attribute.DateOfBirth);
                return string.IsNullOrEmpty(value) ? new DateTime?() : DateTime.Parse(value); //returns null value of DateTime? if Attribute is null
            }
            set => Update(Attribute.DateOfBirth, value);
        }

        /// <summary>
        /// Gets the formatted birthday string of the account holder.
        /// </summary>
        public string Birthday => DateOfBirth != null ? DateTime.Parse(DateOfBirth.ToString()).ToString("dd MMMM, yyyy") : "Not set";

        /// <summary>
        /// Gets or sets the gender of the account holder.
        /// </summary>
        public char Gender
        {
            get => Get(Attribute.Gender)[0];
            set => Update(Attribute.Gender, value);
        }

        /// <summary>
        /// Gets or sets the formatted gender description of the account holder.
        /// </summary>
        public string GenderKind
        {
            get
            {
                switch (Gender)
                {
                    case ('N'):
                        return "Not Set";
                    case ('M'):
                        return "Male";
                    case ('F'):
                        return "Female";
                    case ('O'):
                        return "Other";
                    default:
                        return "NULL";
                }
            }
            set
            {
                switch (value.ToLower())
                {
                    case ("male"):
                        Gender = 'M';
                        break;
                    case ("female"):
                        Gender = 'F';
                        break;
                    case ("other"):
                        Gender = 'O';
                        break;
                    default:
                        Gender = 'N';
                        break;
                }
            }
        }

        /// <summary>
        /// Gets or sets the weight of the account holder, converted to appropriate unit based on settings.
        /// </summary>
        public double Weight
        {
            get
            {
                double weight = double.Parse(Get(Attribute.Weight));
                return Settings.Default.IsMetric ? weight : Utilities.Convert.KgToLb(weight);
            }
            set => Update(Attribute.Weight, Settings.Default.IsMetric ? value : Utilities.Convert.LbToKg(value));
        }

        /// <summary>
        /// Gets the collection of challenges associated with the account.
        /// </summary>
        public ReadOnlyCollection<Challenge> Challenges
        {
            get
            {
                List<Challenge> challenges = new List<Challenge>();

                //Get all associated challenges
                foreach (var ChallengeId in GetAll(Entities.Challenge.Id, Entities.Challenge.UserId, this.id))
                    challenges.Add(new Challenge(int.Parse(ChallengeId)));

                return new ReadOnlyCollection<Challenge>(challenges.OrderByDescending(x => x.Created_at).ToList());
            }
        }

        /// <summary>
        /// Gets or sets the ID of the active challenge for the account.
        /// </summary>
        private int ActiveChallengeId
        {
            get => int.Parse(Get(Attribute.Active_Challenge_Id));
            set => Update(Attribute.Active_Challenge_Id, value);
        }

        /// <summary>
        /// Gets or sets the active challenge associated with the account.
        /// </summary>
        public Challenge ActiveChallenge
        {
            get => Challenge.Find(ActiveChallengeId) ? new Challenge(ActiveChallengeId) : null;
            set => ActiveChallengeId = Challenges.Where(Challenge => Challenge.Id == value.Id) != null ? value.Id : 
                                            throw new AccessDeniedException("Challenge does not exist nor belongs to user.");
        }

        /// <summary>
        /// Gets the collection of challenges associated with the account except the active challenge.
        /// </summary>
        public ReadOnlyCollection<Challenge> InactiveChallenges => new ReadOnlyCollection<Challenge>(Challenges.Where(Challenge => Challenge != ActiveChallenge).ToList());

        /// <summary>
        /// Gets the collection of all activities associated with the account.
        /// </summary>
        public ReadOnlyCollection<Activity> Activities
        {
            get
            {
                var activities = new List<Activity>();
                foreach (var challenge in Challenges)
                    foreach (var activity in challenge.Activities)
                        activities.Add(activity);

                return new ReadOnlyCollection<Activity>(activities.OrderByDescending(x => x.Created_at).ToList());
            }
        }

        /// <summary>
        /// Gets the log of sessions associated with the account.
        /// </summary>
        public ReadOnlyCollection<Session> Sessions
        {
            get
            {
                var log = new List<Session>();
                foreach (var sessionId in GetAll(Entities.Session.Id, Entities.Session.AccountId, this.id))
                    log.Add(new Session(int.Parse(sessionId)));

                return new ReadOnlyCollection<Session>(log.OrderByDescending(x => x.SessionTime).ToList());
            }
        }

        /// <summary>
        /// Gets a read-only collection of sign-in sessions from the list of sessions.
        /// </summary>
        public ReadOnlyCollection<Session> SignInSessions => new ReadOnlyCollection<Session>(Sessions.Where(Session => Session.SessionType is SessionKind.SignIn).ToList());

        /// <summary>
        /// Gets the last updated date and time of the account, converted to local time.
        /// </summary>
        public DateTime UpdatedDateTime => Updated_at.ToLocalTime();

        /// <summary>
        /// Gets the creation date and time of the account, converted to local time.
        /// </summary>
        public DateTime CreatedDateTime => Created_at.ToLocalTime();

        /// <summary>
        /// Gets the updated date and time of the account.
        /// </summary>
        public DateTime Updated_at => DateTime.Parse(Get(Attribute.Updated_at));

        /// <summary>
        /// Gets the creation date and time of the account.
        /// </summary>
        public DateTime Created_at => DateTime.Parse(Get(Attribute.Created_at));

        #endregion

        #region Equality and Operator Overloads

        /// <summary>
        /// Compares two account instances for equality based on their IDs.
        /// </summary>
        /// <param name="a">The first account to compare.</param>
        /// <param name="b">The second account to compare.</param>
        /// <returns>True if both accounts have the same ID; otherwise, false.</returns>
        public static bool operator ==(Account a, Account b)
        {
            if (a is null & b is null)
                return true;
            //only one value will be null in this state
            else if (a is null || b is null)
                return false;
            else if (a.id != null && b.id != null)
                return a.id.Equals(b.id);
            else
                return false;
        }

        /// <summary>
        /// Compares two account instances for inequality based on their IDs.
        /// </summary>
        /// <param name="a">The first account to compare.</param>
        /// <param name="b">The second account to compare.</param>
        /// <returns>True if the accounts have different IDs; otherwise, false.</returns>
        public static bool operator !=(Account a, Account b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current account instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current account instance.</param>
        /// <returns>True if the specified object is an account and has the same ID as the current instance; otherwise, false.</returns>
        public override bool Equals(Object obj)
        {
            if (obj is Account account)
                return this == account;
            else
                return false;
        }

        /// <summary>
        /// Serves as the default hash function for the account instance.
        /// </summary>
        /// <returns>The hash code for the current account instance.</returns>
        public override int GetHashCode() =>
            this.id == null ? 0 : id.GetHashCode();

        #endregion

        /// <summary>
        /// Authenticates the account using the specified authenticator and authentication method.
        /// </summary>
        /// <param name="Authenticator">The authenticator used for authentication. Can be password or token.</param>
        /// <param name="AuthMethod">The authentication method <see cref="AuthenticationMethod"/> to use.</param>
        /// <returns>True if authentication is successful; otherwise, false.</returns>
        public bool Authenticate(object Authenticator, AuthenticationMethod AuthMethod = AuthenticationMethod.ByPassword)
        {
            bool CanAuthenticate;

            CanAuthenticate =
                AuthMethod == AuthenticationMethod.ByPassword ? this.PasswordHash.Equals(GetHash(text: Authenticator)) :
                AuthMethod == AuthenticationMethod.ByLoginToken && this.LoginTokenHash.Equals(GetHash(text: Authenticator));

            return CanAuthenticate;
        }

        /// <summary>
        /// Verifies the username and password against the stored credentials.
        /// </summary>
        /// <param name="Username">The username to verify.</param>
        /// <param name="Password">The password to verify.</param>
        /// <returns>True if the username and password match the stored credentials; otherwise, false.</returns>
        private static bool Verify(string Username, string Password)
        {
            if (!Exists(Attribute.Username, Username))
                return false;

            //else
            return Get(Attribute.PasswordHash, Attribute.Username, Username).Equals(GetHash(Password));
        }

        /// <summary>
        /// Signs in a user using the specified username and password.
        /// </summary>
        /// <param name="Username">The username to sign in.</param>
        /// <param name="Password">The password to sign in.</param>
        /// <returns>The signed-in account.</returns>
        /// <exception cref="DeviceOnSignInCoolDownException">Thrown if signing in is temporarily unavailable.</exception>
        /// <exception cref="InvalidLoginCreditentialException">Thrown if the login credentials are incorrect.</exception>
        public static Account SignIn(string Username, string Password)
        {
            if (LocalStorage.SessionDevice.HasSignInCoolDown)
                throw new DeviceOnSignInCoolDownException("Signing in from thie device is temporarily unavailable due to recent device behavior.", null, LocalStorage.SessionDevice, LocalStorage.SessionDevice.SignInCoolDown);

            if (!Verify(Username, Password))
            {
                Session.Register(SessionKind.FailedSignInAttempt);
                throw new InvalidLoginCreditentialException("Access Denied Due To Incorrect Login Information", null, Username, Password);
            }

            Account Account = new Account() { id = Get(Attribute.Id, Attribute.Username, Username) };

            //Register SigIn Session
            Session.Register(SessionKind.SignIn, Account.id);

            //Store new LoginToken
            LocalStorage.LoginToken = Account.RegenerateLoginToken();

            return Account;
        }

        /// <summary>
        /// Signs in a user using the specified login token.
        /// </summary>
        /// <param name="LoginToken">The login token to sign in.</param>
        /// <returns>The signed-in account.</returns>
        /// <exception cref="InvalidLoginCreditentialException">Thrown if the login token is invalid.</exception>
        public static Account SignIn(string LoginToken)
        {
            if (!Exists(Attribute.LoginTokenHash, GetHash(LoginToken)))
                throw new InvalidLoginCreditentialException("Access Denied Due To Incorrect LoginToken");

            //Search Id via Token
            Account Account = new Account() { id = Get(Attribute.Id, Attribute.LoginTokenHash, GetHash(LoginToken)) };

            //Store new LoginToken
            LocalStorage.LoginToken = Account.RegenerateLoginToken();

            return Account;
        }

        /// <summary>
        /// Signs out the current user and clears their account access.
        /// </summary>
        public void SignOut()
        {
            Session.Register(SessionKind.SignOut, this.id);

            //Refresh LoginToken. This time, No one will know.
            LoginTokenHash = GetHash(GenerateNewLoginToken());

            //Reset stored LoginToken
            LocalStorage.LoginToken = string.Empty;

            this.id = null;
        }

        #region CREATE

        /// <summary>
        /// Creates a new account with the specified username and password.
        /// </summary>
        /// <param name="Username">The username for the new account.</param>
        /// <param name="Password">The password for the new account.</param>
        /// <returns>The newly created account.</returns>
        /// <exception cref="ConflictUsernameException">Thrown if the username is already taken.</exception>
        public static Account Create(string Username, string Password)
        {
            if (Exists(Attribute.Username, Username))
                throw new ConflictUsernameException();

            ExecuteNonQuery($"INSERT INTO Account({NameOf(Attribute.Id)},{NameOf(Attribute.Username)}, {NameOf(Attribute.PasswordHash)}) " +
                            $"VALUES('{GenerateNewAccountId()}','{Username}','{GetHash(Password)}')");

            var new_account = Account.SignIn(Username, Password);

            Session.Register(SessionKind.RegisterAccount, new_account.id);

            return new_account;
        }

        /// <summary>
        /// Creates a new challenge associated with the account.
        /// </summary>
        /// <param name="Calories_Goal">The calorie goal for the challenge.</param>
        /// <param name="Name">The name of the challenge.</param>
        /// <param name="To_Reach_At">The optional date and time by which the challenge should be reached.</param>
        /// <returns>The newly created challenge.</returns>
        public Challenge CreateChallenge(double Calories_Goal, string Name = "", DateTime? To_Reach_At = null)
        {
            Challenge challenge = Challenge.Create(this.id, Calories_Goal, To_Reach_At);
            challenge.Name = Name;
            return challenge;
        }

        private string RegenerateLoginToken()
        {
            //Regenerate LoginToken
            var NewLoginToken = GenerateNewLoginToken();
            this.LoginTokenHash = GetHash(NewLoginToken);

            return NewLoginToken;
        }

        #endregion

        #region Read

        /// <summary>
        /// Retrieves the value of the specified attribute for the account.
        /// </summary>
        /// <param name="Wanted_Attribute">The attribute to retrieve.</param>
        /// <returns>The value of the specified attribute.</returns>
        private string Get(Attribute Wanted_Attribute) =>
            GetById(Wanted_Attribute, this.id);

        #endregion

        #region Update

        /// <summary>
        /// Updates the specified attribute of the account with the given value.
        /// </summary>
        /// <param name="Attribute">The attribute to update.</param>
        /// <param name="Value">The new value for the attribute.</param>
        private void Update(Attribute Attribute, object Value) =>
            UpdateById(this.id, Attribute, Value);

        /// <summary>
        /// Changes the username of the account.
        /// </summary>
        /// <param name="New_Username">The new username.</param>
        /// <param name="Authenticator">The authenticator to verify the change. Can be password or token.</param>
        /// <param name="AuthMethod">The authentication method (default is by password).</param>
        /// <exception cref="ConflictUsernameException">Thrown when the new username already exists.</exception>
        /// <exception cref="AccessDeniedException">Thrown when the authenticator or authentication method is incorrect.</exception>
        public void ChangeUsername(string New_Username, object Authenticator, AuthenticationMethod AuthMethod = AuthenticationMethod.ByPassword)
        {
            if (!Authenticate(Authenticator, AuthMethod))
                throw new AccessDeniedException("Authenticator or AuthenticationMethod is incorrect.", null, this.Username);

            //else
            if (Exists(Attribute.Username, New_Username))
                throw new ConflictUsernameException("Username is already taken.", null, New_Username);

            this.Username = New_Username;
        }

        /// <summary>
        /// Changes the password of the account.
        /// </summary>
        /// <param name="New_Password">The new password.</param>
        /// <param name="Authenticator">The authenticator to verify the change. Can be password or token.</param>
        /// <param name="AuthMethod">The authentication method (default is by password).</param>
        /// <exception cref="AccessDeniedException">Thrown when the authenticator or authentication method is incorrect.</exception>
        public void ChangePassword(string New_Password, object Authenticator, AuthenticationMethod AuthMethod = AuthenticationMethod.ByPassword)
        {
            if (!Authenticate(Authenticator, AuthMethod))
                throw new AccessDeniedException("Authenticator or AuthenticationMethod is incorrect.", null, this.Username);

            this.PasswordHash = GetHash(New_Password);
        }
        #endregion

        #region Delete

        /// <summary>
        /// Deletes the account and all associated properties.
        /// </summary>
        /// <param name="Password">The password for authentication.</param>
        /// <exception cref="AccessDeniedException">Thrown when the authentication is invalid.</exception>
        public void Delete(string Password)
        {
            if (!Authenticate(Password))
                throw new AccessDeniedException("Cannot delete due to incorrect authenticator.", null, this.Username);

            //Delete challenges
            foreach (var challenge in Challenges)
                RemoveChallenge(challenge);

            //Delete user sessions
            foreach (var session in Sessions)
                session.Delete();

            DeleteById(Entities.Table.Account, this.id);
            this.id = null;     //Resets to null

        }

        /// <summary>
        /// Removes a challenge associated with the account.
        /// </summary>
        /// <param name="Challenge">The challenge to remove.</param>
        /// <exception cref="InvalidEntityAccessException">Thrown when the Challenge to remove does not belong to the account.</exception>
        public void RemoveChallenge(Challenge Challenge)
        {
            if (!this.Challenges.Contains(Challenge))
                throw new InvalidEntityAccessException("Illegal Delete Operation. Challenge does not belong to this Account");

            //ActiveChallengeId will be reset if Challenge to remove is Active Challenge
            if (Challenge == this.ActiveChallenge)
                this.ActiveChallengeId = 0;

            Challenge.Delete();
        }

        #endregion
    }
}