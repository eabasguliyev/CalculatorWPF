using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace CalculatorWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Operation _operation;
        private bool _firsInput = true;
        private bool _textChanged;
        private bool _calculated;

        public MainWindow()
        {
            InitializeComponent();

            _operation = new Operation();
        }

        //private void UIElement_OnMouseEnter(object sender, MouseEventArgs e)
        //{
        //    if (!(sender is Button button))
        //        return;

        //    button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4A4A4A"));
        //}

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button button))
                return;

            if (_firsInput)
            {
                _textChanged = false;
                _firsInput = false;
                ClearInput();
            }

            AddValueToTextBlock(button.Content.ToString());
        }

        private void AddValueToTextBlock(string value)
        {
            if (value != "." && TextBlockValue.Text == "0")
                TextBlockValue.Text = String.Empty;

            var temp = TextBlockValue.Text + value;
            TextBlockValue.Text = temp.ToString(CultureInfo.InvariantCulture);
        }

        private bool CheckDot()
        {
            return TextBlockValue.Text.Contains(".");
        }

        private void ButtonDot_OnClick(object sender, RoutedEventArgs e)
        {
            if (CheckDot())
                return;

            AddValueToTextBlock(".");
        }

        private void ButtonCe_OnClick(object sender, RoutedEventArgs e)
        {
            ClearInput();
        }

        private void ClearInput()
        {
            TextBlockValue.Text = "0";
        }
        private void ButtonDel_OnClick(object sender, RoutedEventArgs e)
        {
            RemoveValueFromTextBox();
            TextBlockValue.Focus();

            if (!_calculated)
            {
                _operation.Result = TextBlockValue.Text;
            }
        }

        private void RemoveValueFromTextBox()
        {
            if (string.IsNullOrWhiteSpace(TextBlockValue.Text))
                return;
            
            TextBlockValue.Text = TextBlockValue.Text.Remove(TextBlockValue.Text.Length - 1, 1);
        }

        private void ButtonPlusMinus_OnClick(object sender, RoutedEventArgs e)
        {
            if (TextBlockValue.Text == "0")
                return;

            if (_calculated)
                return;
            var newValue = TextBlockValue.Text.StartsWith("-") ? 
                TextBlockValue.Text.Remove(0, 1) : 
                TextBlockValue.Text.Insert(0, "-");

            TextBlockValue.Text = newValue.ToString(CultureInfo.InvariantCulture);
        }

        private void ButtonOperator_OnClick(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button button))
                return;

            PerformOperation(button.Content.ToString());
        }

        private void PerformOperation(string oprt)
        {
            if (String.IsNullOrWhiteSpace(TextBlockValue.Text))
                return;

            if(String.IsNullOrWhiteSpace(_operation.FirstValue))
            {
                _operation.FirstValue = TextBlockValue.Text;
                _firsInput = true;
            }
            else if (!_calculated && String.IsNullOrWhiteSpace(_operation.SecondValue))
            {
                Calculate();       
            }

            _operation.OperationType = _operation.GetOperationType(oprt);
        }

        private void Calculate()
        {
            if (!String.IsNullOrWhiteSpace(_operation.Result))
            {
                _operation.FirstValue = _operation.Result;
            }

            _operation.SecondValue = TextBlockValue.Text;

            var result = String.Empty;

            try
            {
                result = _operation.Calculate();
            }
            catch(Exception ex)
            {
                result = $"Error: {ex.Message}";
            }

            _operation.Result = result;

            _operation.SecondValue = String.Empty;

            TextBlockValue.Text = _operation.Result;

            _calculated = true;
            _firsInput = true;
        }
        private void TextBlockValue_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            _textChanged = true;
            _calculated = false;
        }

        private void ButtonEqual_OnClick(object sender, RoutedEventArgs e)
        {
            if (!_calculated)
                Calculate();
        }

        private void ButtonC_OnClick(object sender, RoutedEventArgs e)
        {
            ClearInput();
            _operation.Clear();
        }
    }
}