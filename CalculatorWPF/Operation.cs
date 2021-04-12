using System;
using System.Globalization;

namespace CalculatorWPF
{
    public class Operation
    {
        public string FirstValue { get; set; }
        public string SecondValue { get; set; }
        public OperationType OperationType { get; set; }
        public string Result { get; set; }

        public Operation()
        {
            FirstValue = String.Empty;
            SecondValue = String.Empty;
        }

        public void Clear()
        {
            FirstValue = String.Empty;
            SecondValue = String.Empty;
            Result = String.Empty;
        }
        public string Calculate()
        {
            if (String.IsNullOrWhiteSpace(FirstValue))
                return String.Empty;

            if (String.IsNullOrWhiteSpace(SecondValue))
                return FirstValue;


            var firstValue = Double.Parse(FirstValue);
            var secondValue = Double.Parse(SecondValue);
            double result;
            switch (OperationType)
            {
                case OperationType.Add:
                    result = firstValue + secondValue;
                    break;
                case OperationType.Sub:
                    result = firstValue - secondValue;
                    break;
                case OperationType.Mul:
                    result = firstValue * secondValue;
                    break;
                case OperationType.Div:
                {
                    if (secondValue == 0)
                        throw new DivideByZeroException("Divide by zero");
                    
                    result = firstValue / secondValue;
                }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return result.ToString(CultureInfo.InvariantCulture);
        }

        public OperationType GetOperationType(string operation)
        {
            switch (operation)
            {
                case "+":
                    return OperationType.Add;
                case "-":
                    return OperationType.Sub;
                case "*":
                    return OperationType.Mul;
                case "/":
                    return OperationType.Div;
                case "=":
                    return OperationType.Equal;
                default:
                    throw new InvalidOperationException("Unknown operation keyword");
            }
        }
    }

    public enum OperationType
    {
        Add,
        Sub,
        Mul,
        Div,
        Equal
    }
}