using FitTrack.Core;
using System.Windows.Input;
using FitTrack.Model;
using System.Windows;
using FitTrack.Dialogs;

namespace FitTrack.ViewModel
{
    class MainWindowVM : ViewModelBase
    {
        private Account user;
        public Account User
        {
            get => user;
            set { user = value; OnPropertyChanged(); }
        }

        private Challenge selectedChallenge;

        public Challenge SelectedChallenge
        {
            get => selectedChallenge;
            set { selectedChallenge = value; OnPropertyChanged(); }
        }

        private object currentView;
        public object CurrentView
        {
            get => currentView;
            set 
            { 
                currentView = value;
                OnPropertyChanged();

                //Re-decorate which menu is currently selected
                DashboardIsChecked = value is HomeVM;
                ChallengeIsChecked = value is ChallengeVM;
                ProgressIsChecked = value is ProgressVM;
                NewChallengeIsChecked = value is StartNewChallengeVM;
                SettingIsChecked = value is SettingVM || value is ProfileVM || value is ChangeUsernameVM || value is ChangePasswordVM || value is SignInHistoryVM;

                //Store Challenge Data if ChallengeViewCommand has one
                if (value is ChallengeVM ChallengeVM)
                    SelectedChallenge = ChallengeVM.Challenge;
            }
        }

        private bool dashBoardIsChecked;

        public bool DashboardIsChecked
        {
            get => dashBoardIsChecked;
            set { dashBoardIsChecked = value; OnPropertyChanged(); }
        }

        private bool challengeIsChecked;

        public bool ChallengeIsChecked
        {
            get => challengeIsChecked;
            set { challengeIsChecked = value; OnPropertyChanged(); }
        }

        private bool progressIsChecked;

        public bool ProgressIsChecked
        {
            get => progressIsChecked;
            set { progressIsChecked = value; OnPropertyChanged(); }
        }

        private bool newChallengeIsChecked;

        public bool NewChallengeIsChecked
        {
            get => newChallengeIsChecked;
            set { newChallengeIsChecked = value; OnPropertyChanged(); }
        }

        private bool settingIsChecked;

        public bool SettingIsChecked
        {
            get => settingIsChecked;
            set { settingIsChecked = value; OnPropertyChanged(); }
        }

        public ICommand HomeCommand { get; }
        public ICommand ChallengeCommand { get; }
        public ICommand ProgressCommand { get; }
        public ICommand NewChallengeCommand { get; }
        public ICommand SettingCommand { get; }
        public ICommand LogoutCommand { get; }

        private void Home() => CurrentView = new HomeVM(this.User);      //Dashboard View
        private void Challenge() => CurrentView = Model.Challenge.Find(SelectedChallenge?.Id) ? new ChallengeVM(this.User, SelectedChallenge) : new ChallengeVM(this.User);       //Challenge View
        private void Progress() => CurrentView = new ProgressVM(this.User);      //Progress View
        private void NewChallenge() => CurrentView = new StartNewChallengeVM(this.User);     //StartNewChallenge View
        private void Setting() => CurrentView = new SettingVM(this.User);        //Setting View
        private void OnChangeView(ChangeViewMessage message) => CurrentView = message.ViewModel;        //On ChangeView message is received

        public MainWindowVM(Account User)
        {
            this.User = User;

            //Default Challenge is User's ActiveChallenge
            SelectedChallenge = this.User.ActiveChallenge;

            //Default View is Dashboard
            CurrentView = new HomeVM(this.User);     

            //Assign RelayCommands
            HomeCommand = new RelayCommand(Home);
            ChallengeCommand = new RelayCommand(Challenge);
            ProgressCommand = new RelayCommand(Progress);
            NewChallengeCommand = new RelayCommand(NewChallenge);
            SettingCommand = new RelayCommand(Setting);
            LogoutCommand = new CloseWindowRelayCommand<Window>(Logout);

            //Listens for ChangeViewMessage. Respond with OnChangeView.
            EventAggregator.Subscribe<ChangeViewMessage>(OnChangeView);
        }

        private void Logout(Window window)
        {
            if (ConfirmationDialog.Show("Are you sure you want to Logout?","Confirm Logout") == true)
            {
                this.User.SignOut();

                var SigninWindow = new SignInWindow() { DataContext = new SignInWindowVM() };
                SigninWindow.Show();
                window.Close();
            }
        }
    }
}