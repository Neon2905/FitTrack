using FitTrack.Core;
using System;
using System.Windows.Input;
using FitTrack.Model;
using FitTrack.Dialogs;

namespace FitTrack.ViewModel
{
    class StartNewChallengeVM : ViewModelBase
    {     
        private DateTime? goalDate = null;
        public DateTime? GoalDate
        {
            get => goalDate;
            set
            {
                if (value != goalDate)
                {
                    goalDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public string CaloriesUnit
        {
            get => LocalStorage.EnergyInKCalories ? "kCalories" : "calories";
            set { CaloriesUnit = value; OnPropertyChanged(); }
        }


        private Account user;

        public Account User
        {
            get => user;
            set { user = value; OnPropertyChanged(); }
        }

        public DateTime Today { get; set; } = DateTime.UtcNow;

        private double calories;
        public double Calories
        {
            get => LocalStorage.EnergyInKCalories? Utilities.Convert.CalToKcal(calories) : calories;
            set
            {
                calories = value > 0 ? (LocalStorage.EnergyInKCalories ? Utilities.Convert.KcalToCal(value) : value) : 0;
                OnPropertyChanged();
                UpdateHasValidInputs();
            }
        }

        private string challengeName;

        public string ChallengeName
        {
            get => challengeName;
            set { challengeName = value; OnPropertyChanged(); UpdateHasValidInputs(); }
        }

        private bool hasValidInputs;
        public bool HasValidInputs
        {
            get => hasValidInputs;
            set { hasValidInputs = value; OnPropertyChanged(); }
        }
        public ICommand SubmitCommand => new RelayCommand(CreateChallenge);
        public ICommand IncrementCaloriesCommand => new RelayCommand(() => Calories += LocalStorage.EnergyInKCalories ? 1 : 10);
        public ICommand DecrementCaloriesCommand => new RelayCommand(() => Calories -= LocalStorage.EnergyInKCalories ? 1 : 10);
        public ICommand DeleteDateCommand => new RelayCommand(() => GoalDate = null );

        private void CreateChallenge()
        {
            if(GoalDate?.Date < Today.Date)
            {
                MessageDialog.Show($"The target date cannot be set before today's date: {Today:dd-MMM-yyyy}","Invalid Entry");
                return;
            }

            var NewChallenge = User.CreateChallenge(this.calories, ChallengeName , GoalDate);

            MessageDialog.Show($"Your challenge '{NewChallenge.Name}' is now registered.","Challenge Created");
            EventAggregator.Publish(new ChangeViewMessage(new ProgressVM(user) { SelectedChallenge = NewChallenge}));
        }

        private void UpdateHasValidInputs()
        {
            HasValidInputs = !string.IsNullOrEmpty(challengeName) && Calories != 0;
        }

        public StartNewChallengeVM(Account User)
        {
            this.User = User;
        }
    }
}
