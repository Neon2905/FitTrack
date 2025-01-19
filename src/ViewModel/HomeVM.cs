using FitTrack.Core;
using FitTrack.Model;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace FitTrack.ViewModel
{
    class HomeVM : ViewModelBase
    {
        private Account user;
        public Account User
        {
            get { return user; }
            private set { user = value; OnPropertyChanged(); }
        }

        public string[] WeeklyLabels { get; private set; }

        public ReadOnlyCollection<Activity> Activities { get; }


        private SeriesCollection activitySeries = new SeriesCollection();
        public SeriesCollection ActivitySeries
        {
            get { return activitySeries; }
            private set { activitySeries = value; OnPropertyChanged(); }
        }

        public Func<double, string> GaugeLabelFormatter { get; set; }

        public Challenge FeaturedChallenge1
        {
            get => User.InactiveChallenges.Count>0 ? User.InactiveChallenges[0] : null;
        }

        public Challenge FeaturedChallenge2
        {
            get => User.InactiveChallenges.Count>1 ? User.InactiveChallenges[1] : null;
        }

        public ICommand ChallengeViewCommand { get; }
        public ICommand ProfileViewCommand { get; }
        public ICommand ProgressViewCommand { get; }
        public ICommand ActiveChallengeDetailCommand { get; }
        public ICommand FeaturedChallenge1DetailCommand { get; }
        public ICommand FeaturedChallenge2DetailCommand { get; }

        public HomeVM(Account User)
        {
            this.User = User;

            ChallengeViewCommand = new RelayCommand(()=> EventAggregator.Publish( new ChangeViewMessage(new ChallengeVM(User)) ) );
            ProfileViewCommand = new RelayCommand(() => EventAggregator.Publish(new ChangeViewMessage(new ProfileVM(User)) ) );
            ProgressViewCommand = new RelayCommand(() => EventAggregator.Publish(new ChangeViewMessage(new ProgressVM(User)) ) );
            ActiveChallengeDetailCommand = new RelayCommand(() => EventAggregator.Publish(new ChangeViewMessage(
                                                        User.ActiveChallenge is null ? (object)new StartNewChallengeVM(User) : (object)new ChallengeVM(User, User.ActiveChallenge) ) ) );
            FeaturedChallenge1DetailCommand = new RelayCommand(() => EventAggregator.Publish(new ChangeViewMessage(
                                                        FeaturedChallenge1 is null ? (object)new StartNewChallengeVM(User) : (object)new ChallengeVM(User, FeaturedChallenge1) ) ) );
            FeaturedChallenge2DetailCommand = new RelayCommand(() => EventAggregator.Publish(new ChangeViewMessage(
                                                        FeaturedChallenge2 is null ? (object)new StartNewChallengeVM(User) : (object)new ChallengeVM(User, FeaturedChallenge2))));

            GaugeLabelFormatter = value => $"{value}%"; // Format as double and add %

            InitiateActivityReportChart(User.Activities);
        }

        private void InitiateActivityReportChart(ReadOnlyCollection<Activity> Activities)
        {
            WeeklyLabels = Enumerable.Range(0, 7)
            .Select(offset => DateTime.UtcNow.AddDays(-6).AddDays(offset).ToString("dddd"))
            .ToArray();

            var endDate = DateTime.UtcNow;
            var startDate = endDate.AddDays(-6);        //Last 7 days

            // Group and sum the activities by exercise type and date
            var groupedActivities = Activities
                .Where(a => a.Created_at >= startDate && a.Created_at <= endDate)
                .GroupBy(a => new { a.ExerciseId, a.Created_at.Date })
                .Select(g => new
                {
                    g.Key.ExerciseId,
                    g.Key.Date,
                    TotalCalories = g.Sum(a => a.Burned_Calories)
                })
                .ToList();

            foreach (var exercise in Exercise.GetAll())
            {
                var values = new ChartValues<double>();

                for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
                {
                    var activityForDate = groupedActivities
                        .FirstOrDefault(g => g.ExerciseId == exercise.Id && g.Date == date);

                    values.Add(activityForDate?.TotalCalories ?? 0);
                }

                // Only add the series if the total calories are greater than 0
                if (values.Sum() > 0)
                {
                    ActivitySeries.Add(new LineSeries
                    {
                        Title = exercise.Name,
                        Values = values
                    });
                }
            }
        }
    }
}