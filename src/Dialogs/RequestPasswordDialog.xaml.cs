using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace FitTrack.Dialogs
{
    /// <summary>
    /// Interaction logic for UserAuthenticationDialog.xaml
    /// </summary>
    public partial class RequestPasswordDialog : Window
    {
        /// <summary>
        /// The password entered by the user.
        /// </summary>
        public string Password { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestPasswordDialog"/> class.
        /// </summary>
        /// <param name="title">The title of the dialog window. If <c>null</c>, the default title will be used.</param>
        /// <param name="message">The message to display in the dialog. If <c>null</c>, no message will be displayed.</param>
        public RequestPasswordDialog(string title = null, string message = null)
        {
            InitializeComponent();
            SubmitButton.IsEnabled = false;     //Disabled by default.
            PasswordBox.PreviewKeyUp += PasswordBox_PreviewKeyUp;

            this.Owner = Application.Current.MainWindow; // Set owner to main window
            if (title != null) TitleBlock.Text = title;
            if (message != null) MessageBlock.Text = message;
        }

        private void PasswordBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            //Disable Submit button when password is empty.
            SubmitButton.IsEnabled = !string.IsNullOrEmpty(PasswordBox.Password);
        }

        private void Submit(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PasswordBox.Password)) 
                return;

            Password = PasswordBox.Password;
            this.DialogResult = true; // Set dialog result to true
            this.Close(); // Close the dialog
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false; // Set dialog result to false
            this.Close(); // Close the dialog
        }

        private void MouseDrag(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            CloseGrid.Background = new SolidColorBrush(Colors.IndianRed);
            CloseIcon.Foreground = new SolidColorBrush(Colors.White);
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            CloseGrid.Background = null;
            CloseIcon.Foreground = new SolidColorBrush(Colors.Gray);
        }
    }
}
