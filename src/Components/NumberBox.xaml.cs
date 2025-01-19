using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FitTrack.Components
{
    /// <summary>
    /// Interaction logic for NumberBox.xaml
    /// </summary>
    public partial class NumberBox : UserControl
    {
        public NumberBox()
        {
            InitializeComponent();
        }

        private bool _isTextChanging;

        private string placeholder;
        public string Placeholder
        {
            get { return placeholder; }
            set { placeholder = value; PlaceHolderText.Text = value; }
        }

        public string Unit
        {
            get { return (string)GetValue(UnitProperty); }
            set { SetValue(UnitProperty, value); }
        }


        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        private void Input_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isTextChanging) return;

            _isTextChanging = true;
            Value = Input.Text;
            _isTextChanging = false;

            if (Value.Count(c => c == '.') > 1)
            {
                int caretIndex = Input.CaretIndex;
                Input.Text = Input.Text.Remove(Input.Text.LastIndexOf('.'));
                Input.CaretIndex = Math.Max(0, caretIndex - 1);
            }

            if (string.IsNullOrEmpty(Input.Text))
                PlaceHolderText.Visibility = Visibility.Visible;
            else 
                PlaceHolderText.Visibility = Visibility.Collapsed;
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(string), typeof(NumberBox),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    TextPropertyChanged, null, false, UpdateSourceTrigger.PropertyChanged));

        public static readonly DependencyProperty UnitProperty =
            DependencyProperty.Register("Unit", typeof(string), typeof(NumberBox),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    UnitPropertyChanged, null, false, UpdateSourceTrigger.PropertyChanged));

        private static void TextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is NumberBox numberBox)
            {
                numberBox.UpdateText();
            }
        }

        private static void UnitPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is NumberBox numberBox)
            {
                numberBox.UpdateUnit();
            }
        }

        private void Input_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void UpdateText()
        {
            if (!_isTextChanging)
            {
                Input.Text = Value;
            }
        }

        private void UpdateUnit()
        {
            UnitText.Text = Unit;
        }

        private bool IsTextAllowed(string text)
        {
            return text.All(c => char.IsDigit(c) || c == '.');
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Input.Focus();
        }
    }
}
