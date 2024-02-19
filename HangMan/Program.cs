using System;
using System.Collections.Generic;

namespace Hangman
{
    internal class Program
    {

        public static void Main(string[] args)
        {
            Hangman hangman = new Hangman();
            hangman.Start();

        }
    }

    class Hangman
    {
        private Dictionary<string, string> words = new Dictionary<string, string>
        {
            {"Elephant", "Large mammal"},
            {"Computer", "Electronic device"},
            {"Guitar", "Musical instrument"},
            {"Sunshine", "Direct light from the sun"},
            {"Pineapple", "Tropical fruit"},
            {"Butterfly", "Colorful insect"},
            {"Rainbow", "Arc of colors in the sky"},
            {"Chocolate", "Sweet food"},
            {"Mountain", "Large landform with steep sides"},
            {"Adventure", "Exciting and unusual experience"},
            {"Pancake", "Flat cake often served for breakfast"},
            {"Universe", "All of space and everything in it"},
            {"Fireworks", "Explosive display of light and color"},
            {"Waterfall", "Flowing water over a cliff"},
            {"Kangaroo", "Marsupial found in Australia"},
            {"Banana", "Yellow fruit with a curved shape"},
            {"Octopus", "Sea creature with eight arms"},
            {"Jazz", "Type of music characterized by improvisation"},
            {"Diamond", "Precious gemstone known for its hardness"},
            {"Rocket", "Vehicle propelled by engines"},
            {"Whale", "Large marine mammal"},
            {"Dragon", "Legendary creature with serpentine or reptilian traits"},
            {"Moonlight", "Light from the moon"},
            {"Squirrel", "Small rodent with a bushy tail"},
            {"Galaxy", "System of stars, dust, and gas"},
            {"Penguin", "Flightless bird found in cold regions"},
            {"Zebra", "African mammal with black and white stripes"},
            {"Volcano", "Opening in Earth's crust that emits lava"},
            {"Thunder", "Sound caused by lightning"},
            {"Tornado", "Violently rotating column of air"}
        };

        private KeyValuePair<string, string> wordAndTip;
        private int guesses = 5;

        public Hangman()
        {
            wordAndTip = words.ElementAt(RandomNumGenerator(words.Count));
        }

        public void Start()
        {
            ShowMessage("Welcome to the Hangman Game!");
            ShowMessage("\nRules:");
            ShowMessage("Try to guess the word by entering one letter at a time.");
            ShowMessage("You have a total of 5 incorrect guesses allowed.");
            ShowMessage("\nLet's begin!");

            string word = wordAndTip.Key.ToLower();
            string tip = wordAndTip.Value.ToLower();
            string encrypted = "";

            for (int i = 0; i < word.Length; i++)
            {
                encrypted += '_';
            }

            ShowMessage("\nHere is a tip and an encrypted word:");
            ShowMessage(tip);
            ShowMessage(encrypted);
            ShowMessage("\nYou have 5 guesses. LETS PLAY!!!");


            int tries = 0;

            while (tries <= guesses)
            {
                if (tries == guesses)
                {
                    ShowMessage("\nYou are out of guesses, so you have LOST the game!");
                    break;
                }

                ShowMessage("Do you want to enter the whole word(w) or only one character(c)? (w/c):");
                char choosen = ReadCharacter();
                if (choosen == 'w')
                {
                    ShowMessage("Okay, enter the word! but take into consideration that if this word is incorrect you will LOSE game!");
                    string guessedWord = ReadWord();

                    if (guessedWord.ToUpper() == word.ToUpper())
                    {
                        ShowMessage($"\nCongratulations! guessed word {guessedWord} is correct!");
                        ShowMessage("YOU WON!");
                        break;
                    }
                    else
                    {
                        ShowMessage($"\nSorry! guessed word {guessedWord} is incorrect!");
                        ShowMessage($"Correct word was {word.ToUpper()}!");
                        ShowMessage("YOU LOST!");
                        break;
                    }
                }
                else if (choosen == 'c')
                {
                    ShowMessage("\nGuess the CHARACTER:");
                    char guessed = ReadCharacter();
                    

                    if (word.Contains(guessed) && !encrypted.Contains(Char.ToUpper(guessed)))
                    {
                        
                        ShowMessage("\nCongratulation! your guessed character is correct!");
                        encrypted = Decryption(word, encrypted, guessed);
                        ShowMessage(tip);
                        ShowMessage(encrypted);
                        ShowMessage($"\nYou have {guesses - tries} Guesses left.");
                        continue;
                    }
                    else if (word.Contains(guessed) && encrypted.Contains(Char.ToUpper(guessed)))
                    {
                        tries++;
                        ShowMessage("\nYou already guessed this character!");
                        ShowMessage(tip);
                        ShowMessage(encrypted);
                        ShowMessage($"\nYou have {guesses - tries} Guesses left.");
                        continue;
                    }
                    else
                    {
                        tries++;
                        ShowMessage("\nSorry! your guessed character is incorrect!");
                        ShowMessage(tip);
                        ShowMessage(encrypted);
                        ShowMessage($"\nYou have {guesses - tries} Guesses left.");
                        continue;
                    }
                }
                else
                {
                    ShowMessage("\nPlease, enter one of the above options c for character, or w for word!");
                    continue;
                }
            }

            ShowMessage($"\nDo you want to play again? (Y/N)");

            string answer = Console.ReadLine();
            if (answer.ToLower() == "y")
            {
                wordAndTip = words.ElementAt(RandomNumGenerator(words.Count));
                tries = 0;
                Start();
            }
            else if (answer.ToLower() == "n")
            {
                ShowMessage($"Hope you liked the game. Be back soon!!!");
                return;
            }



        }

        private string Decryption(string word, string encrypted, char character)
        {
            string result = "";
            for (int i = 0; i < word.Length; i++)
            {
                if (word[i] == character)
                {
                    result += character;
                }
                else
                {
                    result += encrypted[i];
                }
            }

            return result.ToUpper();
        }

        private int RandomNumGenerator(int b)
        {
            Random random = new Random();
            int generatedNumber = random.Next(0, b);
            return generatedNumber;
        }

        private void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }

        private string ReadWord()
        {
            string guessedWord = Console.ReadLine();

            while (guessedWord == null)
            {
                ShowMessage("Invalid input. Please enter a valid word:");
                guessedWord = Console.ReadLine();
            }

            return guessedWord;
        }

        private char ReadCharacter()
        {
            char character;
            while (!char.TryParse(Console.ReadLine(), out character))
            {
                ShowMessage("Invalid input. Please enter a valid character (Accepted only ONE character!!!):");
            }
            return Char.ToLower(character);
        }
    }
}