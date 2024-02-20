using System;

namespace Calculator
{
    class Program
    {
        public static void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }

        public static void Main(string[] args)
        {
            Calculator calculator = new Calculator(ShowMessage);
            
            calculator.Start();
        }
    }

    class Calculator
    {
        public delegate void DisplayMessage(string message);
        public DisplayMessage showMessage;

        public Calculator(DisplayMessage showMessage)
        {
            this.showMessage = showMessage;
        }

        public void Start()
        {
            double num1 = 0;
            double num2 = 0;
            while (true)
            {
                if (num1 == 0)
                {
                    showMessage("Enter the first number:");
                    num1 = ReadNumber();
                }

                showMessage("Enter the second number:");
                num2 = ReadNumber();

                showMessage("Enter the operation (+, -, *, /):");
                string operation = Console.ReadLine();

                try
                {
                    double result = Calculate(num1, num2, operation);
                    showMessage($"Result: {result}");

                    showMessage("Do you want to perform another calculation? (yes (write new number)/no/yc  (for continue calculation from old result!):");
                    string answer = Console.ReadLine();
                    if (answer.ToLower() == "no")
                        break;
                    else if (answer.ToLower() == "yc")
                    {
                        num1 = result;
                        continue; 
                    }
                    else if (answer.ToLower() == "yes")
                    {
                        num1 = 0; 
                        continue; 
                    }
                }
                catch (Exception ex)
                {
                    showMessage($"Error: {ex.Message}");
                }
                
            }
        }

        public double Calculate(double x, double y, string operation)
        {
            switch (operation)
            {
                case "+":
                    return x + y;
                case "-":
                    return x - y;
                case "*":
                    return x * y;
                case "/":
                    if (y == 0)
                    {
                        showMessage("You can't divide by ZERO!!!");
                        return double.NaN;
                    }
                    return x / y;
                default:
                    throw new ArgumentException($"Invalid operation: {operation}. Please provide one of the following operations: +, -, *, /");
            }
        }

        private double ReadNumber()
        {
            double number;
            while (!double.TryParse(Console.ReadLine(), out number))
            {
                showMessage("Invalid input. Please enter a valid number:");
            }
            return number;
        }
    }
}
