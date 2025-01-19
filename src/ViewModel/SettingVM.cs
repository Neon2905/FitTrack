using FitTrack.Core;
using FitTrack.Dialogs;
using FitTrack.Model;
using System;
using System.Windows;
using System.Windows.Input;

namespace FitTrack.ViewModel
{
    class SettingVM : ViewModelBase
    {
        private Account user;
        public Account User
        {
            get => user;
            private set { user = value; }
        }

        public bool IsMetric
        {
            get => Properties.Settings.Default.IsMetric;
            set => Properties.Settings.Default.IsMetric = value;
        }

        public bool DurationInHour
        {
            get => Properties.Settings.Default.DurationInHour;
            set => Properties.Settings.Default.DurationInHour = value;
        }

        public bool IsKCalories
        {
            get => Properties.Settings.Default.IsKCalories;
            set => Properties.Settings.Default.IsKCalories = value;
        }

        public ICommand EditPersonalProfileCommand { get; }
        public ICommand ChangePasswordCommand { get; }
        public ICommand ChangeUsernameCommand { get; }
        public ICommand DeleteAccountCommand { get; }
        public ICommand AccountLogCommand { get; }

        public SettingVM(Account user)
        {
            this.User = user;
            EditPersonalProfileCommand = new RelayCommand(() => EventAggregator.Publish(new ChangeViewMessage(new ProfileVM(User))));
            ChangeUsernameCommand = new RelayCommand(ChangeViewToChangeUsernameView);
            ChangePasswordCommand = new RelayCommand(ChangeViewToChangePasswordView);
            DeleteAccountCommand = new CloseWindowRelayCommand<Window>(DeleteAccount);
            AccountLogCommand = new RelayCommand(() => EventAggregator.Publish(new ChangeViewMessage(new SignInHistoryVM(User))));
        }

        private void ChangeViewToChangePasswordView()
        {
            var PasswordDialog = new RequestPasswordDialog();
            bool? result = PasswordDialog.ShowDialog();
            if (result == true)
            {
                if (User.Authenticate(PasswordDialog.Password))
                    EventAggregator.Publish(new ChangeViewMessage(new ChangePasswordVM(User)));
                else
                    MessageDialog.Show("Your password was incorrect.");
            }
        }

        private void ChangeViewToChangeUsernameView()
        {
            var PasswordDialog = new RequestPasswordDialog();
            bool? result = PasswordDialog.ShowDialog();
            if (result == true)
            {
                if (User.Authenticate(PasswordDialog.Password))
                    EventAggregator.Publish(new ChangeViewMessage(new ChangeUsernameVM(User)));
                else
                    MessageDialog.Show("Your password was incorrect.");
            }
        }

        private void DeleteAccount(Window window)
        {
            var PasswordDialog = new RequestPasswordDialog();

            //First, request password
            bool? result = PasswordDialog.ShowDialog();

            if (result != true)
                return;

            if (!User.Authenticate(PasswordDialog.Password))
            {
                MessageDialog.Show("Your password was incorrect.");
                return;
            }

            var ConfirmText = "Delete My Account";
            var ConfirmAccountDeletion = new RequestUserTextInputDialog("Confirm Deletion", $"Please enter the following text to procced account deletion: '{ConfirmText}'");
            //Confirm action by making user type {ConfirmText} correctly
            bool? deletionConfirmation = ConfirmAccountDeletion.ShowDialog();

            if (deletionConfirmation != true)
                return;

            //Check User-Text-Input
            if (!ConfirmAccountDeletion.UserInput.Equals(ConfirmText))
            {
                MessageDialog.Show("The text you entered was incorrect.", "Incorrect Input");
                return;
            }

            //Last Confirmation
            if (ConfirmationDialog.Show("Are you sure you want to delete your account? This can never be undone!", "Confirm Action") != true)
                return;

            //Finally proceed to delete
            try
            {
                User.Delete(PasswordDialog.Password);
            }
            catch (Exception exception)
            {
                MessageDialog.Show(exception.Message, "An Error Occured");
                return;
            }
            finally
            {
                MessageDialog.Show("Your Account was deleted Successfully.");

                LocalStorage.LoginToken = string.Empty;

                SignInWindow SigninWindow = new SignInWindow() { DataContext = new SignInWindowVM() };
                SigninWindow.Show();
                window.Close();
            }
        }
    }
}