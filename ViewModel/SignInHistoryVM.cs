using FitTrack.Core;
using FitTrack.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace FitTrack.ViewModel
{
    class SignInHistoryVM : ViewModelBase
    {
        private ReadOnlyCollection<Session> signInSessions;

        public ReadOnlyCollection<Session> SignInSessions
        {
            get => signInSessions;
            set { signInSessions = value; OnPropertyChanged(); }
        }

        public ICommand NavigateSettingCommand { get; }

        public SignInHistoryVM(Account User)
        {
            this.SignInSessions = User.SignInSessions;
            NavigateSettingCommand = new RelayCommand(()=> EventAggregator.Publish(new ChangeViewMessage(new SettingVM(User))));
        }
    }
}