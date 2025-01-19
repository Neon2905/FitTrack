using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FitTrack.Dialogs
{
    /// <summary>
    /// Interaction logic for ConfirmationDialog.xaml
    /// </summary>
    public partial class ConfirmationDialog : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfirmationDialog"/> class.
        /// </summary>
        /// <param name="message">The message to display in the dialog.</param>
        /// <param name="title">The title of the dialog window.</param>
        public ConfirmationDialog(string message, string title)
        {
            InitializeComponent();
            TitleBlock.Text = title;
            MessageBlock.Text = message;
            this.Owner = Application.Current.MainWindow; // Set owner to main window
        }

        /// <summary>
        /// Displays the confirmation dialog with the specified message and title.
        /// </summary>
        /// <param name="message">The message to display in the dialog.</param>
        /// <param name="title">The title of the dialog window. Defaults to "Confirm".</param>
        /// <returns>A nullable boolean indicating the user's response. Returns true if "Yes" is clicked, false if "No" is clicked, and null if the dialog is closed without a selection.</returns>
        public static bool? Show(string message, string title = "Confirm")
        {
            var window = new ConfirmationDialog(message, title);
            return window.ShowDialog();
        }

        private void Yes(object sender, RoutedEventArgs e)
        {
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

        private void Grid_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            CloseGrid.Background = new SolidColorBrush(Colors.IndianRed);
            CloseIcon.Foreground = new SolidColorBrush(Colors.White);
        }

        private void Grid_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            CloseGrid.Background = null;
            CloseIcon.Foreground = new SolidColorBrush(Colors.Gray);
        }
    }
}
