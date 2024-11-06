using FitTrack.Core;
using FitTrack.Dialogs;
using FitTrack.Exceptions;
using FitTrack.Model;
using FitTrack.Utilities;
using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;

namespace FitTrack.ViewModel
{
    class SignInWindowVM : ViewModelBase
    {
        private string username = "";
        public string Username
        {
            get => username;
            set 
            { 
                username = value;
                OnPropertyChanged();
                UpdateCanSignIn();
            }
        }

        private string password = "";
        public string Password
        {
            get => password;
            set 
            { 
                password = value;
                OnPropertyChanged();
                UpdateCanSignIn();
            }
        }

        private bool canSignIn;

        public bool CanSignIn
        {
            get => canSignIn;
            set 
            { 
                canSignIn = value;
                OnPropertyChanged();
            }
        }

        private string firstName;

        public string FirstName
        {
            get => firstName;
            set { firstName = value; OnPropertyChanged(); }
        }

        private string lastName;

        public string LastName
        {
            get => lastName;
            set { lastName = value; OnPropertyChanged(); UpdateCanSignUp(); }
        }

        private string newUsername;

        public string NewUsername
        {
            get => newUsername;
            set { newUsername = value; OnPropertyChanged(); UpdateCanSignUp(); }
        }

        private string email;

        public string Email
        {
            get => email;
            set { email = value; OnPropertyChanged(); }
        }

        private string newPassword;

        public string NewPassword
        {
            get => newPassword;
            set { newPassword = value; OnPropertyChanged(); UpdateCanSignUp(); }
        }

        private string confirmPassword;

        public string ConfirmPassword
        {
            get => confirmPassword;
            set { confirmPassword = value; OnPropertyChanged(); UpdateCanSignUp(); }
        }

        private char gender = 'O';

        public char Gender
        {
            get => gender;
            set { gender = value; OnPropertyChanged(); }
        }

        private bool canSignUp;
        public bool CanSignUp
        {
            get => canSignUp;
            set { canSignUp = value; OnPropertyChanged(); }
        }

        private bool passwordRequiredLengthNotMet = false;
        public bool PasswordRequiredLengthNotMet
        {
            get => passwordRequiredLengthNotMet;
            set
            {
                passwordRequiredLengthNotMet = value;
                OnPropertyChanged();
            }
        }
        private bool passwordsNotMatch = false;
        public bool PasswordsNotMatch
        {
            get => passwordsNotMatch;
            set
            {
                passwordsNotMatch = value;
                OnPropertyChanged();
            }
        }

        private bool isSigningUp;
        public bool IsSigningUp
        {
            get => isSigningUp;
            set { isSigningUp = value; OnPropertyChanged(); }
        }

        private bool invalidLoginData = false;

        public bool InvalidLoginData
        {
            get => invalidLoginData;
            set { invalidLoginData  = value; OnPropertyChanged(); }
        }

        private string invalidLoginError;
        public string InvalidLoginError
        {
            get => invalidLoginError;
            set { invalidLoginError = value; OnPropertyChanged(); }
        }

        private string passwordLengthNotMetError;
        public string PasswordLengthNotMetError
        {
            get => passwordLengthNotMetError;
            set { passwordLengthNotMetError = value; OnPropertyChanged(); }
        }

        private string passwordsNotMatchError;
        public string PasswordsNotMatchError
        {
            get => passwordsNotMatchError;
            set { passwordsNotMatchError = value; OnPropertyChanged(); }
        }

        private string passwordRequirementsNotMetError;
        public string PasswordRequirementsNotMetError
        {
            get => passwordRequirementsNotMetError;
            set { passwordRequirementsNotMetError = value; OnPropertyChanged(); }
        }

        private string usernameAlreadyTakenError;
        public string UsernameAlreadyTakenError
        {
            get => usernameAlreadyTakenError;
            set { usernameAlreadyTakenError = value; OnPropertyChanged(); }
        }

        private string usernameNotMetRequirementError;
        public string UsernameNotMetRequirementError
        {
            get => usernameNotMetRequirementError;
            set { usernameNotMetRequirementError = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand { get; }
        public ICommand SetMaleCommand { get; }
        public ICommand SetFemaleCommand { get; }
        public ICommand SetOtherCommand { get; }
        public ICommand SignUpViewCommand { get; }
        public ICommand SignInViewCommand { get; }
        public ICommand SignUpCommand { get; }

        public SignInWindowVM()
        {
            LoginCommand = new CloseWindowRelayCommand<Window>(Login);
            SetMaleCommand = new RelayCommand(() => Gender = 'M');
            SetFemaleCommand = new RelayCommand(() => Gender = 'F');
            SetOtherCommand = new RelayCommand(() => Gender = 'O');
            SignInViewCommand = new RelayCommand(() => IsSigningUp = false);
            SignUpViewCommand = new RelayCommand(() => IsSigningUp = true);
            SignUpCommand = new CloseWindowRelayCommand<Window>(SignUp);
        }

        private void Login(Window Parent)
        {
            try
            {
                var account = Account.SignIn(Username, Password);
                Window Window = new MainWindow() { DataContext = new MainWindowVM(account) };
                Window.Show();
                Parent?.Close();
            }
            catch(InvalidLoginCreditentialException)
            {
                InvalidLoginError = ErrorMessages.InvalidLoginDataMessage;
            }
            catch(DeviceOnSignInCoolDownException exception)
            {
                MessageDialog.Show($"You can't log in due to multiple failure login attempts. Please try again after: " +
                    $"{DateTime.Parse(exception.SignInCoolDown.ToString()).ToLocalTime():dd MMMM, HH:mm}", "Login Error");
            }
        }

        private void SignUp(Window Parent)
        {
            if (ValidateSignUpData())
            {
                try
                {
                    var user = Account.Create(NewUsername, NewPassword);

                    user.Gender = Gender;

                    if (!string.IsNullOrEmpty(Email))
                        user.Email = Email;

                    if (!string.IsNullOrEmpty(FirstName))
                        user.FirstName = FirstName;

                    if (!string.IsNullOrEmpty(LastName))
                        user.LastName = LastName;

                    MessageDialog.Show("Account Created Successfully!", "Signup Succeed");
                    Window main_window = new MainWindow() { DataContext = new MainWindowVM(user) };
                    main_window.Show();
                    Parent?.Close();
                }
                catch (ConflictUsernameException)
                {
                   UsernameAlreadyTakenError = ErrorMessages.UsernameAlreadyTakenMessage;
                }
                catch (SqlException e)
                {
                    MessageDialog.Show(e.Message+e.InnerException,"Error");
                }
            }
        }

        private void UpdateCanSignIn() => CanSignIn = Password?.Length > 0 && Username.Length > 0;

        private void UpdateCanSignUp() => CanSignUp = !string.IsNullOrEmpty(LastName) &&
                                            !string.IsNullOrEmpty(NewUsername) &&
                                            !string.IsNullOrEmpty(NewPassword);

        private bool ValidateSignUpData()
        {
            bool valid = true;

            if (StringScanner.Scan(NewUsername).HasSpecialCharacterAndSpace)
            {
                UsernameNotMetRequirementError = ErrorMessages.UsernameRequirementsNotMetMessage;
                valid = false;
            }
            else UsernameNotMetRequirementError = null;

            if (NewPassword == ConfirmPassword)
            {
                PasswordsNotMatchError = null;

                if (NewPassword.Length == Rules.PasswordMustHaveLength)
                    PasswordLengthNotMetError = null;
                else
                {
                    PasswordLengthNotMetError = ErrorMessages.RequiredPasswordLengthNotMetMessage;
                    valid = false;
                }

                StringScanner scanner =  StringScanner.Scan(NewPassword);

                if (scanner.HasUpperCaseLetter && scanner.HasLowerCaseLetter)
                    PasswordRequirementsNotMetError = null;
                else
                {
                    PasswordRequirementsNotMetError = ErrorMessages.PasswordRequirementsNotMetMessage;
                    valid = false;
                }
            }
            else
            {
                PasswordsNotMatchError = ErrorMessages.PasswordsNotMatchMessage;
                valid = false;
            }

            return valid;
        }
    }
}