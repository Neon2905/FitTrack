using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace FitTrack.Components
{
    /// <summary>
    /// Interaction logic for TextBox.xaml
    /// </summary>
    public partial class TextBox : UserControl
    {
        public TextBox()
        {
            InitializeComponent();
        }

        private string placeholder;

        public string Placeholder
        {
            get { return placeholder; }
            set { placeholder = value; PlaceHolderText.Text = value; }
        }

        private void Input_TextChanged(object sender, TextChangedEventArgs e)
        {
            _isTextChanging = true;
            Text = Input.Text;
            _isTextChanging = false;

            if (string.IsNullOrEmpty(Input.Text))
                PlaceHolderText.Visibility = Visibility.Visible;
            else PlaceHolderText.Visibility = Visibility.Collapsed;

            // Raise the custom TextChanged event
            OnTextChanged(e);
        }
        private bool _isTextChanging;

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(TextBox),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    TextPropertyChanged, null, false, UpdateSourceTrigger.PropertyChanged));

        private static void TextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox textBox)
            {
                textBox.UpdateText();
            }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        private void UpdateText()
        {
            if (!_isTextChanging)
            {
                Input.Text = Text;
            }
        }

        // Custom TextChanged event
        public event TextChangedEventHandler TextChanged;

        protected virtual void OnTextChanged(TextChangedEventArgs e)
        {
            TextChanged?.Invoke(this, e);
        }
    }
}
