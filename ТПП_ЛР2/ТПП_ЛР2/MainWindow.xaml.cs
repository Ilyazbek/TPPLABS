using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Data;

namespace CalculatorApp
{
    public partial class MainWindow : Window
    {
        private string _currentInput = "";
        private List<string> _history = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
        }
      
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                if (button.Content.ToString() == "." && _currentInput.Contains("."))
                {
                    return;
                }

                _currentInput += button.Content.ToString();
                Display.Text = _currentInput;
            }
        }

        private void Equal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = EvaluateExpression(_currentInput);
                _history.Add($"{_currentInput} = {result}");
                Display.Text = result.ToString();

                HistoryDisplay.Text = string.Join("\n", _history);
                _currentInput = result.ToString();

                Display.Foreground = System.Windows.Media.Brushes.Black;
            }
            catch (Exception ex)
            {
                Display.Foreground = System.Windows.Media.Brushes.Red;
                Display.Text = $"Ошибка: {ex.Message}";
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            _currentInput = "";
            Display.Text = "";
            Display.Foreground = System.Windows.Media.Brushes.Black;
        }

        private double EvaluateExpression(string expression)
        {
            if (expression.Contains("/0"))
            {
                throw new InvalidOperationException(" деление на ноль.");
            }

            int openBrackets = 0;
            foreach (char c in expression)
            {
                if (c == '(') openBrackets++;
                if (c == ')') openBrackets--;
            }
            if (openBrackets != 0)
            {
                throw new InvalidOperationException(" незакрытые скобки.");
            }

            var table = new DataTable();
            try
            {
                var result = table.Compute(expression, string.Empty);
                return Convert.ToDouble(result);
            }
            catch
            {
                throw new InvalidOperationException("Некорректное выражение.");
            }
        }
    }
}
