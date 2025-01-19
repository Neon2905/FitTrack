using System.Windows;
using System.Windows.Controls;

namespace FitTrack.Components
{
    public partial class PasswordBox : UserControl
    {
        private bool _isPasswordChanging;

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(PasswordBox),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    PasswordPropertyChanged));

        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register("Placeholder", typeof(string), typeof(PasswordBox),
                new PropertyMetadata("Enter password"));

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        private static void PasswordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox passwordBox && !passwordBox._isPasswordChanging)
            {
                passwordBox.UpdatePassword();
            }
        }

        public PasswordBox()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!_isPasswordChanging)
            {
                _isPasswordChanging = true;
                Password = passwordBox.Password;
                textBox.Text = Password;
                _isPasswordChanging = false;
                UpdatePlaceholderVisibility();
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_isPasswordChanging)
            {
                Password = textBox.Text;
                passwordBox.Password = Password;
                UpdatePlaceholderVisibility();
            }
        }

        private void UpdatePassword()
        {
            if (!_isPasswordChanging)
            {
                passwordBox.Password = Password;
                textBox.Text = Password;
                UpdatePlaceholderVisibility();
            }
        }

        private void UpdatePlaceholderVisibility()
        {
            if (!string.IsNullOrEmpty(Password))
                PlaceHolderText.Visibility = Visibility.Collapsed;
            else
                PlaceHolderText.Visibility = Visibility.Visible;
        }

        private void ShowPassword(object sender, RoutedEventArgs e)
        {
            passwordBox.Visibility = Visibility.Collapsed;
            textBox.Visibility = Visibility.Visible;
        }

        private void HidePassword(object sender, RoutedEventArgs e)
        {
            passwordBox.Visibility = Visibility.Visible;
            textBox.Visibility = Visibility.Collapsed;
        }
    }
}