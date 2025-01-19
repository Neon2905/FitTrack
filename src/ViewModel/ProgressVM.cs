using FitTrack.Core;
using FitTrack.Dialogs;
using FitTrack.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace FitTrack.ViewModel
{
    class ProgressVM : ViewModelBase
    {
        private Account user;

        public Account User
        {
            get => user;
            set { user = value; OnPropertyChanged(); }
        }

        public string EnergyUnit
        {
            get => LocalStorage.EnergyInKCalories ? "kCalories" : "Calories";
            set => EnergyUnit = value;
        }

        private bool hasSelectedChallenge;
        public bool HasSelectedChallenge
        {
            get => hasSelectedChallenge;
            set
            {
                hasSelectedChallenge = value;
                OnPropertyChanged();
            }
        }

        private bool isSelectedChallengeActive;
        public bool IsSelectedChallengeActive
        {
            get => isSelectedChallengeActive; 
            set { isSelectedChallengeActive = value; OnPropertyChanged(); }
        }


        public ObservableCollection<Challenge> Challenges
        {
            get => new ObservableCollection<Challenge> (User.Challenges);
            set { Challenges = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Activity> activities;
        public ObservableCollection<Activity> Activities
        {
            get => activities;
            set { activities = value; OnPropertyChanged(); }
        }

        private Challenge selectedChallenge;
        public Challenge SelectedChallenge
        {
            get => selectedChallenge;
            set
            {
                selectedChallenge = value;
                HasSelectedChallenge = !(selectedChallenge is null);
                IsSelectedChallengeActive = SelectedChallenge == User.ActiveChallenge;
                OnPropertyChanged();
            }
        }

        public ICommand CreateChallengeCommand { get; }
        public ICommand SetActiveCommand { get; }
        public ICommand DeleteSelectedChallengeCommand { get; }
        public ICommand ViewSelectedChallengeCommand { get; }

        public ProgressVM(Account User)
        {
            this.User = User;

            //Select a challenge on display
            SelectedChallenge = User.ActiveChallenge ?? (User.Challenges.Count > 0 ? User.Challenges[0] : null);

            CreateChallengeCommand = new RelayCommand(StartNewChallenge);
            SetActiveCommand = new RelayCommand(SetSelectedChallengeAsActive);
            DeleteSelectedChallengeCommand = new RelayCommand(DeleteSelectedChallenge);
            ViewSelectedChallengeCommand = new RelayCommand(() => EventAggregator.Publish(new ChangeViewMessage(new ChallengeVM(User, SelectedChallenge))));
        }

        private void StartNewChallenge() =>
            EventAggregator.Publish(new ChangeViewMessage(new StartNewChallengeVM(User)));

        private void SetSelectedChallengeAsActive()
        {
            if (this.SelectedChallenge.GoalAlreadyReached)
            {
                MessageDialog.Show("Cannot activate completed challenge.", "Challenge Cannot Be Activated");
                return;
            }
            if (this.SelectedChallenge.Expired)
            {
                MessageDialog.Show("Sorry, you cannot activate expired challenges. Try new challenge instead!", "Challenge Cannot Be Activated");
                return;
            }
            
            //Finally Set Active
            User.ActiveChallenge = this.SelectedChallenge;
            EventAggregator.Publish(new ChangeViewMessage(new ChallengeVM(User)));
        }

        private void DeleteSelectedChallenge()
        {
            if (ConfirmationDialog.Show("Are you sure you want to delete this challenge history? This cannot be undone.", "Confirm To Delete Challenge") == true)
            {
                User.RemoveChallenge(SelectedChallenge);
                Refresh();
            }
        }

        private void Refresh() =>
            EventAggregator.Publish(new ChangeViewMessage(new ProgressVM(User)));
    }
}