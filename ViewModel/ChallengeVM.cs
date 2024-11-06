using FitTrack.Core;
using System.Collections.ObjectModel;
using FitTrack.Model;
using FitTrack.Utilities;
using System.Windows.Input;
using LiveCharts;
using LiveCharts.Wpf;
using FitTrack.Dialogs;
using System;

namespace FitTrack.ViewModel
{
    class ChallengeVM : ViewModelBase
    {
        #region Properties
        private Account user;
        public Account User
        {
            get => user;
            set { user = value; }
        }

        private Challenge challenge;
        public Challenge Challenge
        {
            get => challenge;
            set { challenge = value; OnPropertyChanged(); }
        }

        public bool HasChallenge { get; }

        public bool IsChallengeActive { get; }
        
        public string EnergyUnit
        {
            get => LocalStorage.EnergyInKCalories ? "kcalories" : "calories";
            set => EnergyUnit = value;
        }

        private ObservableCollection<Exercise> exerciseMenuItems;
        public ObservableCollection<Exercise> ExerciseMenuItems
        {
            get => exerciseMenuItems;
            set { exerciseMenuItems = value; OnPropertyChanged(); }
        }

        private Exercise selectedExercise;
        public Exercise SelectedExercise
        {
            get => selectedExercise;
            set 
            { 
                selectedExercise = value; 
                OnPropertyChanged(); 
                UpdateValidActivityEntries();

                Low_MET_Expression = value.LOW_MET_EXPRESSION;
                Medium_MET_Expression= value.MEDIUM_MET_EXPRESSION;
                Extreme_MET_Expression = value.EXTREME_MET_EXPRESSION;
            }
        }

        private string low_MET_Expression = "Low Effort";
        public string Low_MET_Expression
        {
            get => low_MET_Expression;
            set { low_MET_Expression = value; OnPropertyChanged(); }
        }

        private string medium_MET_Expression = "Medium Effort";
        public string Medium_MET_Expression
        {
            get => medium_MET_Expression;
            set { medium_MET_Expression = value; OnPropertyChanged(); }
        }

        private string extreme_MET_Expression = "Extreme Effort";
        public string Extreme_MET_Expression
        {
            get => extreme_MET_Expression;
            set { extreme_MET_Expression = value; OnPropertyChanged(); }
        }

        public string WeightUnit
        {
            get => LocalStorage.WeightInKg ? "kg" : "lb";
            set => WeightUnit = value;
        }

        public string DurationUnit
        {
            get => LocalStorage.DurationInHour ? "hrs" : "mins";
            set => DurationUnit = value;
        }

        private double duration;
        public double Duration
        {
            get => LocalStorage.DurationInHour ? Utilities.Convert.MinuteToHour(duration) : duration;
            set 
            {
                duration = LocalStorage.DurationInHour ? Utilities.Convert.HourToMinute(value) : value;
                OnPropertyChanged();
                UpdateValidActivityEntries();
            }
        }

        private double weight_in_Metric;
        public double Weight
        {
            get => LocalStorage.WeightInKg? weight_in_Metric : Utilities.Convert.KgToLb(weight_in_Metric);
            set 
            {
                weight_in_Metric = LocalStorage.WeightInKg ? value : Utilities.Convert.LbToKg(value); 
                OnPropertyChanged();
                UpdateValidActivityEntries();
            }
        }

        private ExerciseIntensity exerciseIntensity;
        public ExerciseIntensity ExerciseIntensity
        {
            get => exerciseIntensity;
            set { exerciseIntensity = value; OnPropertyChanged(); }
        }

        private bool isAddingNewActivity;
        public bool IsAddingNewActivity
        {
            get => isAddingNewActivity;
            set { isAddingNewActivity = value; OnPropertyChanged(); }
        }

        private bool validActivityEntries;
        public bool ValidActivityEntries
        {
            get => validActivityEntries;
            set 
            { validActivityEntries = value; OnPropertyChanged(); }
        }

        public SeriesCollection PieSeriesCollection { get; set; }

    #endregion

        private void UpdateValidActivityEntries() =>
                ValidActivityEntries = Weight > 0 && Duration > 0 && SelectedExercise != null;

        public ICommand SetActiveCommand { get; }
        public ICommand LowMETCommand { get; } 
        public ICommand MidMETCommand { get; }
        public ICommand ExtremeMETCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand SubmitCommand { get; }
        public ICommand AddActivityCommand { get; }

        public ChallengeVM(Account User, Challenge Challenge = null)
        {
            this.User = User;

            //Set selected challenge if Challenge parameter is not given
            this.Challenge = Challenge is null ? User.ActiveChallenge : Challenge;

            HasChallenge = !(this.Challenge is null);

            IsChallengeActive = this.Challenge == User.ActiveChallenge;

            ExerciseMenuItems = new ObservableCollection<Exercise>( Exercise.GetAll() );

            ExerciseIntensity = ExerciseIntensity.Medium;

            this.Weight = User.Weight;

            SetActiveCommand = new RelayCommand(SetChallengeAsActive);
            LowMETCommand = new RelayCommand(() => ExerciseIntensity = ExerciseIntensity.Low);
            MidMETCommand = new RelayCommand(() => ExerciseIntensity = ExerciseIntensity.Medium);
            ExtremeMETCommand = new RelayCommand(() => ExerciseIntensity = ExerciseIntensity.Extreme);
            AddActivityCommand = new RelayCommand(AddActivity);
            CancelCommand = new RelayCommand(() => IsAddingNewActivity = false);
            SubmitCommand = new RelayCommand(SubmitActivity);

            PieSeriesCollection = new SeriesCollection();
            UpdatePieSeries();
        }

        private void AddActivity()
        {
            if (this.Challenge.GoalAlreadyReached)
                MessageDialog.Show("Cannot make a new activity for this challenge because your target is already reached!", "Challenge Already Complete");
            else if (this.Challenge.Expired)
                //Challenge Expired
                MessageDialog.Show("Sorry, You cannot make new activity to a challenge already passed target date.", "Cannot Make New Activity");
            else
                //Add Activity View
                IsAddingNewActivity = true;
        }

        private void SubmitActivity()
        {
            double Burned_Calories = Calculator.CalculateTotalCaloriesBurned(SelectedExercise.METof(ExerciseIntensity), this.Weight, this.Duration);
            this.Challenge.CreateActivity(selectedExercise.Id, Weight, Duration, Burned_Calories);
            if (this.Challenge.GoalAlreadyReached)
            {
                this.Challenge.Finished_At = DateTime.UtcNow;
                MessageDialog.Show($"Congratulation!!! You have reached your target to burn {this.Challenge?.Calories_Goal}{EnergyUnit}!", "Target Reached");
            }
            else
                MessageDialog.Show($"You have burned {Burned_Calories} {EnergyUnit}. \nKeep going! Every bit counts toward your goal!", "Activity Registered");
            IsAddingNewActivity = false;
            Refresh();
        }

        private void SetChallengeAsActive()
        {
            if(this.Challenge.GoalAlreadyReached)
                MessageDialog.Show("Cannot activate completed challenge.", "Challenge Cannot Be Activated");
            else if(this.Challenge.Expired)
                MessageDialog.Show("Sorry, you cannot activate expired challenges. Try new challenge instead!", "Challenge Cannot Be Activated");
            else
            {
                User.ActiveChallenge = this.Challenge;
                Refresh();
            }
        }

        private void UpdatePieSeries()
        {
            //PieSeriesCollection.Clear();
            if (this.Challenge != null) 
            { 
                foreach (Exercise exercise in Exercise.GetAll())
                {
                    if(Challenge.Toatl_Progressed_Calories(exercise.Id) >0)
                        PieSeriesCollection.Add(new PieSeries
                        {
                            Title = exercise.Name,
                            Values = new ChartValues<double> { Challenge.Toatl_Progressed_Calories(exercise.Id) }
                        });

                }
            }
        }

        private void Refresh() =>
            EventAggregator.Publish(new ChangeViewMessage(new ChallengeVM(User)));
    }
}