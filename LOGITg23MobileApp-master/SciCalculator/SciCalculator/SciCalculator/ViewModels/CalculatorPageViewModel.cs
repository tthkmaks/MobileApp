using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NCalc;
using System;
using System.Collections.Generic;

namespace ViewModels
{
    [INotifyPropertyChanged]
    internal partial class CalculatorPageViewModel //: ObservableObject
    {
        [ObservableProperty]
        private string inputText = string.Empty;

        [ObservableProperty]
        private string calculatedResult = "0";

        private bool isSciOpWaiting = false;

        [RelayCommand]
        private void Reset()
        {
            CalculatedResult = "0";
            InputText = string.Empty;
            isSciOpWaiting = false;
        }

        [RelayCommand]
        private void Calculate()
        {
            if (InputText.Length == 0)
            {
                return;
            }

            // If a closing parenthesis is expected for a scientific operation
            if (isSciOpWaiting)
            {
                InputText += ")";
                isSciOpWaiting = false;
            }

            try
            {
                // Normalize the input string to replace operators
                var inputString = NormalizeInputString();
                var expression = new Expression(inputString);
                var result = expression.Evaluate();

                // Assigning the result to a property CalculatedResult
                CalculatedResult = result.ToString();
            }
            catch (Exception ex)
            {
                // If an error occurs, we show "NaN"
                CalculatedResult = "NaN";
            }
        }

        private string NormalizeInputString()
        {
            // Mapping for mathematical and scientific operators
            Dictionary<string, string> _opMapper = new()
            {
                {"×", "*"},
                {"÷", "/"},
                {"SIN", "Sin"},
                {"COS", "Cos"},
                {"TAN", "Tan"},
                {"ASIN", "Asin"},
                {"ACOS", "Acos"},
                {"ATAN", "Atan"},
                {"LOG", "Log"},
                {"EXP", "Exp"},
                {"LOG10", "Log10"},
                {"POW", "Pow"},
                {"SQRT", "Sqrt"},
                {"ABS", "Abs"},
            };

            var retString = InputText;

            // We use operator replacement
            foreach (var key in _opMapper.Keys)
            {
                retString = retString.Replace(key, _opMapper[key]);
            }

            // Remove extra spaces if there are any
            retString = retString.Replace(" ", string.Empty);

            return retString;
        }

        [RelayCommand]
        private void Backspace()
        {
            Console.WriteLine($"Before Backspace: {InputText}");
            if (InputText.Length > 0)
            {
                InputText = InputText.Substring(0, InputText.Length - 1);
                Console.WriteLine($"After Backspace: {InputText}");
            }
        }

        [RelayCommand]
        private void NumberInput(string key)
        {
            InputText += key;
        }

        [RelayCommand]
        private void MathOperator(string op)
        {
            // If a parenthesis is expected for a scientific operation, close it
            if (isSciOpWaiting)
            {
                InputText += ")";
                isSciOpWaiting = false;
            }
            InputText += $" {op} ";
        }

        [RelayCommand]
        private void RegionOperator(string op)
        {
            // If a parenthesis is expected for a scientific operation, close it
            if (isSciOpWaiting)
            {
                InputText += ")";
                isSciOpWaiting = false;
            }
            InputText += $" {op} ";
        }

        [RelayCommand]
        private void ScientificOperator(string op)
        {
            InputText += $"{op}(";
            isSciOpWaiting = true;
        }
    }
}
