using System;

namespace GuessTheNumber
{

    internal class Program
    {
        public static void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }
        public static void Main(string[] args)
        {
            GuessTheNumber guessTheNumber = new GuessTheNumber(ShowMessage);

            guessTheNumber.Start();
        }
    }
    class GuessTheNumber
    {
        public int guesses = 5;
        public delegate void DisplayMessage(string message);
        public DisplayMessage showMessage;

        public GuessTheNumber(DisplayMessage showMessage)
        {
            this.showMessage = showMessage;
        }

        public void Start()
        {
            int tries = 1;
            showMessage("This is Guessing the number game!");
            showMessage("Rules are pretty easy. User is choosing the range of number, in which they want to play!");

            showMessage("\nChoose 1st number of the Range!");
            int num1 = ReadNumber();
            showMessage("\nChoose 2st number of the Range!");
            int num2 = ReadNumber();
            showMessage("\nYou have 5 guesses. LETS PLAY!!!");
            showMessage("\nWhat is your guess?!");
            
            int generatedNum = RandomNumGenerator(num1, num2);


            while (true)
            {

               
                try
                {
                    int UsersGuess = ReadNumber();
                    if (UsersGuess > num2 || UsersGuess < num1)
                    {
                        showMessage($"Enter only numbers in range. You want loose guesses!");
                        showMessage("\nWhat is your guess!!!");
                        continue;

                    }
                    if (generatedNum == UsersGuess)
                    {
                        showMessage($"Congragulation!!! You guessed the number in {tries} try");
                        break;
                        
                    }
                     if (guesses <= 1)
                    {
                        showMessage($"You have left {guesses - 1} Guesses left. YOU LOST!!!");

                        break;
                    }
                    else if (generatedNum > UsersGuess)
                    {
                        tries += 1;
                        showMessage($"A bit too low!!! Go higher. you have left {guesses - 1} Guesses left");
                        guesses--;
                        
                    }
                    else if (generatedNum < UsersGuess)
                    {
                        tries += 1;
                        showMessage($"A bit too High!!! Go Lower. you have left {guesses - 1} Guesses left");
                        guesses--;
                        
                    }
                   
                    
                    

                }
                
                catch (Exception ex)
                {
                    showMessage($"Error: {ex.Message}");
                }
            }
            showMessage($"Do you want to play again? (Y/N)");
           
            string answer = Console.ReadLine();

            if (answer.ToLower() == "y")
            {
                tries = 1;
                guesses = 5;
                Start();
            }
            else 
            {
                showMessage($"Hope you liked the game. Be back soon!!!");
                return;
            }
            

        }
        public int RandomNumGenerator(int a, int b)
        {
            Random random = new Random();
            int generatedNumber = random.Next(a, b);
            return generatedNumber;
        }
        private int ReadNumber()
        {
            int number;
            while (!int.TryParse(Console.ReadLine(), out number))
            {
                showMessage("Invalid input. Please enter a valid number (Accepted only WHOLE Numbers!!!):");
            }
            return number;
        }
    }
}
