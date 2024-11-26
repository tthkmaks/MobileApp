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

            // Если ожидается закрывающая скобка для научной операции
            if (isSciOpWaiting)
            {
                InputText += ")";
                isSciOpWaiting = false;
            }

            try
            {
                // Нормализуем строку ввода, чтобы заменить операторы
                var inputString = NormalizeInputString();
                var expression = new Expression(inputString);
                var result = expression.Evaluate();

                // Присваиваем результат в свойство CalculatedResult
                CalculatedResult = result.ToString();
            }
            catch (Exception ex)
            {
                // Если произошла ошибка, показываем "NaN"
                CalculatedResult = "NaN";
            }
        }

        private string NormalizeInputString()
        {
            // Маппинг для математических и научных операторов
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

            // Применяем замену операторов
            foreach (var key in _opMapper.Keys)
            {
                retString = retString.Replace(key, _opMapper[key]);
            }

            // Удаляем лишние пробелы, если они есть
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
            // Если ожидается скобка для научной операции, закрываем ее
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
            // Если ожидается скобка для научной операции, закрываем ее
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
