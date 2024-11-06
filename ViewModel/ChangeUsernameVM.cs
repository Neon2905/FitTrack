using FitTrack.Core;
using FitTrack.Dialogs;
using FitTrack.Exceptions;
using FitTrack.Model;
using FitTrack.Utilities;
using System;
using System.Windows.Input;

namespace FitTrack.ViewModel
{
    class ChangeUsernameVM : ViewModelBase
    {
        public Account User { get; private set; }

        private string newUsername;
        public string NewUsername
        {
            get => newUsername;
            set { newUsername = value; OnPropertyChanged(); }
        }


        private string usernameNotMetRequirementError;
        public string UsernameNotMetRequirementError
        {
            get => usernameNotMetRequirementError;
            set { usernameNotMetRequirementError = value; OnPropertyChanged(); }
        }


        public ICommand NavigateSettingCommand { get; }
        public ICommand ChangeUsernameCommand { get; }

        public ChangeUsernameVM(Account User)
        {
            this.User = User;
            NavigateSettingCommand = new RelayCommand(() => EventAggregator.Publish(new ChangeViewMessage(new SettingVM(User))));
            ChangeUsernameCommand = new RelayCommand(ChangeUsername);
        }

        private void ChangeUsername()
        {
            //Check if username is valid
            if (StringScanner.Scan(NewUsername).HasSpecialCharacterAndSpace)
            {
                UsernameNotMetRequirementError = ErrorMessages.UsernameRequirementsNotMetMessage;
                return;
            }

            UsernameNotMetRequirementError = null;

            //Check if username is new
            if (NewUsername.Equals(User.Username))
            {
                MessageDialog.Show("New username cannot be current username.", "Cannot change username");
                return;
            }

            //Confirms user to change username
            if (ConfirmationDialog.Show($"Are you sure you want to change your username to {NewUsername}?") != true)
                return;

            //Proceed to change username
            try
            {
                User.ChangeUsername(NewUsername, LocalStorage.LoginToken, Database.AuthenticationMethod.ByLoginToken);
            }
            catch (AccessDeniedException Exception) { MessageDialog.Show(Exception.Message, "Failed to change username"); }

            catch (ConflictUsernameException)
            {
                MessageDialog.Show("Your username is already taken by someone else. Please choose a different one.",
                                                                    "Username already taken");
            }
            finally
            {
                MessageDialog.Show($"Your username has successfully changed. Your login username will be: '{NewUsername}'", "Success");
                EventAggregator.Publish(new ChangeViewMessage(new SettingVM(User)));
            }
        }
    }
}
