using FitTrack.Core;
using FitTrack.Dialogs;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace FitTrack
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }
        private void Minimize(object sender, RoutedEventArgs e) 
        {
            this.WindowState = WindowState.Minimized;
        }
        private void Close(object sender, RoutedEventArgs e)
        {
            if(ConfirmationDialog.Show("Are you sure you want to exist?") == true)
            {
                Properties.Settings.Default.Save();
                this.Close();
            }
        }
        private void MouseDrag(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //PlayStartupSong();
        }

        private void PlayStartupSong()
        {
            MediaPlayer mediaPlayer;
            mediaPlayer = new MediaPlayer();
            mediaPlayer.Open(new Uri("")); // Adjust the path to audio file
            mediaPlayer.Play();
        }
    }
}
