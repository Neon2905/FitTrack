using FitTrack.Core;
using FitTrack.Dialogs;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
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
        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Run(() => PlayStartupSong());
            // The startup song is currently disabled to respect user preferences and avoid unnecessary interruptions.
        }

        private void PlayStartupSong()
        {
            MediaPlayer mediaPlayer;
            mediaPlayer = new MediaPlayer();
            mediaPlayer.Open(new Uri("Resources/start-up.mp3", UriKind.Relative));
            //mediaPlayer.Play(); /* Uncomment this line to enable the startup sound. */
        }
    }
}
