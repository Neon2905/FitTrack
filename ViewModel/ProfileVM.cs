using FitTrack.Core;
using FitTrack.Dialogs;
using FitTrack.Model;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace FitTrack.ViewModel
{
    class ProfileVM : ViewModelBase
    {
        public Account User { get; private set; }

        private bool isChanging;

        public bool IsChanging
        {
            get { return isChanging; }
            set { isChanging = value; OnPropertyChanged(); }
        }

        public bool IsMetric
        {
            get => Properties.Settings.Default.IsMetric;
            set { }
        }

        public string WeightUnit
        {
            get => IsMetric ? "kg" : "lb";
            set { }
        }

        private string firstName;
        public string FirstName
        {
            get => firstName;
            set
            {
                firstName = value;
                OnPropertyChanged();
            }
        }

        private string lastName;
        public string LastName
        {
            get => lastName;
            set
            {
                lastName = value;
                OnPropertyChanged();
            }
        }

        private string email;
        public string Email
        {
            get => email;
            set
            {
                email = value;
                OnPropertyChanged();
            }
        }

        private readonly ObservableCollection<string> genderItems = new ObservableCollection<string>() { "Male", "Female", "Other" };
        public ObservableCollection<string> GenderItems
        {
            get => genderItems;
            set { /*Dummy-Code*/ }
        }

        private string selectedGender;
        public string SelectedGender
        {
            get => selectedGender;
            set
            {
                selectedGender = value;
                OnPropertyChanged();
            }
        }

        private DateTime? dateOfBirth;
        public DateTime? DateOfBirth
        {
            get => dateOfBirth;
            set
            {
                dateOfBirth = value;
                OnPropertyChanged();
            }
        }

        //Assuming Minimum Age of user is 10 Years Old
        public DateTime BirthDateLimit { get; set; } = DateTime.UtcNow.AddYears(-10);

        private double weight;
        public double Weight
        {
            get => weight;
            set
            {
                weight = value;
                OnPropertyChanged();
            }
        }

        public ICommand NavigateSettingCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand DeleteDateOfBirthCommand { get; }

        public ProfileVM(Account User)
        {
            this.User = User;
            selectedGender = User.GenderKind;

            NavigateSettingCommand = new RelayCommand(() => EventAggregator.Publish(new ChangeViewMessage(new SettingVM(User))));
            EditCommand = new RelayCommand(Edit);
            CancelCommand = new RelayCommand(() => IsChanging = false);
            SaveCommand = new RelayCommand(Save);
            DeleteDateOfBirthCommand = new RelayCommand(() => DateOfBirth = null);

            DateOfBirth = User.DateOfBirth;
        }

        private void Edit()
        {
            IsChanging = true;
            FirstName = User.FirstName;
            LastName = User.LastName;
            Email = User.Email;
            Weight = User.Weight;
        }

        private void Save()
        {
            if (string.IsNullOrEmpty(LastName))
            {
                MessageDialog.Show("Last Name is required.");
                return;
            }

            if (DateOfBirth > BirthDateLimit)
            {
                MessageDialog.Show("Please enter a valid date of birth.", "Invalid Entry");
                return;
            }

            //Proceed Profile Change
            try
            {
                User.FirstName = FirstName;
                User.LastName = LastName;
                User.Email = Email;
                User.GenderKind = SelectedGender;
                User.DateOfBirth = DateOfBirth;
                User.Weight = Weight;
                OnPropertyChanged(nameof(User));
            }
            catch (Exception ex) { MessageDialog.Show(ex.Message, "Error"); }
            finally
            {
                IsChanging = false;
                MessageDialog.Show("Changes to personal profile has been saved.", "Changes Saved");
            }
        }
    }
}