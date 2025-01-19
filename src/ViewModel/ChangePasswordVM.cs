using FitTrack.Core;
using System.Windows.Input;
using FitTrack.Model;
using FitTrack.Utilities;
using FitTrack.Exceptions;
using FitTrack.Dialogs;

namespace FitTrack.ViewModel
{
    class ChangePasswordVM : ViewModelBase
    {
        public Account User { get; private set; }

        private bool canChangePassword;
        public bool CanChangePassword
        {
            get => canChangePassword;
            set
            {
                canChangePassword = value;
                OnPropertyChanged();
            }
        }

        private void UpdateCanChangePassword() => CanChangePassword = NewPassword.Length > 0 && ConfirmPassword.Length > 0;

        private string passwordsNotMatchError;
        public string PasswordsNotMatchError
        {
            get => passwordsNotMatchError;
            set { passwordsNotMatchError = value; OnPropertyChanged(); }
        }

        private string passwordLengthNotMetError;
        public string PasswordLengthNotMetError
        {
            get => passwordLengthNotMetError;
            set { passwordLengthNotMetError = value; OnPropertyChanged(); }
        }

        private string passwordRequirementsNotMetError;
        public string PasswordRequirementsNotMetError
        {
            get => passwordRequirementsNotMetError;
            set { passwordRequirementsNotMetError = value; OnPropertyChanged(); }
        }


        private string newPassword = "";
        public string NewPassword
        {
            get => newPassword;
            set { newPassword = value; OnPropertyChanged(); UpdateCanChangePassword(); }
        }

        private string confirmPassword = "";
        public string ConfirmPassword
        {
            get => confirmPassword;
            set { confirmPassword = value; OnPropertyChanged(); UpdateCanChangePassword(); }
        }

        public ICommand NavigateSettingCommand { get; }
        public ICommand SubmitCommand { get; }

        public ChangePasswordVM(Account User)
        {
            this.User = User;
            NavigateSettingCommand = new RelayCommand(() => EventAggregator.Publish(new ChangeViewMessage(new SettingVM(User))));
            SubmitCommand = new RelayCommand(ChangePassword);
        }

        private void ChangePassword()
        {
            if (NewPasswordIsAllowed())
            {
                //Check if new password does not match original password
                if (User.Authenticate(NewPassword))
                {
                    MessageDialog.Show("New password cannot be your old password.", "Cannot change password.");
                    return;
                }

                //Confirms user to change password
                if (ConfirmationDialog.Show($"Are you sure you want to change your password?") != true)
                    return;

                //Proceed password change
                try
                {
                    User.ChangePassword(NewPassword, LocalStorage.LoginToken, Database.AuthenticationMethod.ByLoginToken);
                }
                catch (AccessDeniedException e)
                {
                    MessageDialog.Show(e.Message); return;
                }
                finally
                {
                    MessageDialog.Show("Your password has changed successfully!", "Success");
                    EventAggregator.Publish(new ChangeViewMessage(new SettingVM(User)));
                }
            }
        }

        private bool NewPasswordIsAllowed()
        {
            bool isValid = true;

            if (NewPassword == ConfirmPassword)
            {
                PasswordsNotMatchError = null;

                if (NewPassword.Length == Rules.PasswordMustHaveLength)
                    PasswordLengthNotMetError = null;
                else
                {
                    PasswordLengthNotMetError = ErrorMessages.RequiredPasswordLengthNotMetMessage;
                    isValid = false;
                }

                StringScanner scanner = StringScanner.Scan(NewPassword);

                if (scanner.HasUpperCaseLetter && scanner.HasLowerCaseLetter)
                    PasswordRequirementsNotMetError = null;
                else
                {
                    PasswordRequirementsNotMetError = ErrorMessages.PasswordRequirementsNotMetMessage;
                    isValid = false;
                }
            }
            else
            {
                PasswordsNotMatchError = ErrorMessages.PasswordsNotMatchMessage;
                isValid = false;
            }

            return isValid;
        }

    }
}