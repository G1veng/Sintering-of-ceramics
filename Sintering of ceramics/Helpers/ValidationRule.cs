using System.Globalization;
using System;
using System.Windows.Controls;
using Sintering_of_ceramics.Enums;
using System.Windows;

namespace Sintering_of_ceramics.Helpers
{
    public class ValidationRule : System.Windows.Controls.ValidationRule
    {
        public CompareTypeEnum CompareType { get; set; }
        public Type? ValidationType { get; set; }
        public string? ErrorMessage { get; set; }
        public Wrapper Wrapper { get; set; } = null!;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var strValue = Convert.ToString(value);

            if (string.IsNullOrEmpty(strValue))
                return new ValidationResult(false, $"Значение не может быть конвертировано.");

            bool canConvert = false;
            switch (ValidationType?.Name)
            {
                case "Int32":
                    int intVal = 0;
                    canConvert = int.TryParse(strValue, out intVal);
                    if (!canConvert)
                        new ValidationResult(false, null);

                    canConvert = IsValid(intVal);
                    break;
                case "Double":
                    double doubleVal = 0;
                    canConvert = double.TryParse(strValue, out doubleVal);
                    if (!canConvert)
                        new ValidationResult(false, null);

                    canConvert = IsValid(doubleVal);
                    break;
                default:
                    throw new InvalidCastException($"{ValidationType?.Name ?? string.Empty} не поддерживается");
            }

            return canConvert ? new ValidationResult(true, null) : new ValidationResult(false, ErrorMessage == null ? null : string.Format(ErrorMessage, Wrapper.MinValue, Wrapper.MaxValue, value));
        }

        private bool IsValid(double passedValue)
            => CompareType switch
            {
                CompareTypeEnum.Greater => passedValue > (double)Wrapper.MinValue,
                CompareTypeEnum.Less => passedValue < (double)Wrapper.MaxValue,
                CompareTypeEnum.GreaterOrEqual => passedValue >= (double)Wrapper.MinValue,
                CompareTypeEnum.LessOrEqual => passedValue <= (double)Wrapper.MaxValue,
                CompareTypeEnum.InRange => passedValue >= (double)Wrapper.MinValue && passedValue <= (double)Wrapper.MaxValue,
                _ => false
            };

        private bool IsValid(int passedValue)
            => CompareType switch
            {
                CompareTypeEnum.Greater => passedValue > (int)Wrapper.MinValue,
                CompareTypeEnum.Less => passedValue < (int)Wrapper.MaxValue,
                CompareTypeEnum.GreaterOrEqual => passedValue >= (int)Wrapper.MinValue,
                CompareTypeEnum.LessOrEqual => passedValue <= (int)Wrapper.MaxValue,
                CompareTypeEnum.InRange => passedValue >= (int)Wrapper.MinValue && passedValue <= (int)Wrapper.MaxValue,
                _ => false
            };
    }

    public class Wrapper : DependencyObject
    {
        public static readonly DependencyProperty MinValueProperty =
             DependencyProperty.Register("MinValue", typeof(object),
             typeof(Wrapper), new FrameworkPropertyMetadata(0));

        public static readonly DependencyProperty MaxValueProperty =
             DependencyProperty.Register("MaxValue", typeof(object),
             typeof(Wrapper), new FrameworkPropertyMetadata(0));

        public object MinValue
        {
            get { return GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        public object MaxValue
        {
            get { return GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }
    }
}
