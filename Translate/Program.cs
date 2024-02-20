using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Translate
{
    class Translator
    {
        private List<TranslationEntry> translations;

        delegate void OutputDelegate(string value);

        public void Run()
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;

            LoadTranslations();

            OutputDelegate writeLine = Console.WriteLine;

            writeLine("Available languages:");
            writeLine(string.Join(", ", typeof(TranslationEntry).GetProperties().Select(p => p.Name)));

            writeLine("Enter the input language:");
            string inputLanguage = Console.ReadLine().ToLower();

            writeLine("Enter the output language:");
            string outputLanguage = Console.ReadLine().ToLower();

            bool continueTranslation = true;
            while (continueTranslation)
            {
                writeLine("Enter a word:");
                string input = Console.ReadLine();

                string translation = Translate(inputLanguage, outputLanguage, input);
                if (string.IsNullOrEmpty(translation))
                {
                    writeLine("Translation not found.");
                    writeLine("Would you like to add a new Translation? (Y/N)");
                    string addTranslationOption = Console.ReadLine().ToLower();
                    if (addTranslationOption == "y")
                    {
                        AddNewTranslation(inputLanguage, outputLanguage, input);
                        LoadTranslations(); // Reload translations after adding new one
                    }
                }
                else
                {
                    writeLine($"Translation: {translation}");
                    writeLine("Would you like to save this translation? (Y/N)");
                    string saveOption = Console.ReadLine().ToLower();
                    if (saveOption == "y")
                    {
                        SaveTranslationsToJson();
                    }
                }

                writeLine("Do you want to continue translation? (Y/N)");
                string continueOption = Console.ReadLine().ToLower();
                continueTranslation = continueOption == "y";
            }
        }

        private void LoadTranslations()
        {
            string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "words.json");
            string json = File.ReadAllText(jsonPath);
            var data = JsonConvert.DeserializeObject<TranslationsData>(json);
            translations = data.Translations;
        }

        private string Translate(string inputLanguage, string outputLanguage, string word)
        {
            var entry = translations.FirstOrDefault(t => t.GetPropertyValue(inputLanguage)?.ToLower() == word.ToLower());
            return entry?.GetPropertyValue(outputLanguage);
        }

        private void AddNewTranslation(string inputLanguage, string outputLanguage, string word)
        {
            OutputDelegate writeLine = Console.WriteLine;
            writeLine($"Enter the translation for {inputLanguage}:");
            string inputTranslation = Console.ReadLine();

            writeLine($"Enter the translation for {outputLanguage}:");
            string outputTranslation = Console.ReadLine();

            TranslationEntry newTranslation = new TranslationEntry();
            switch (inputLanguage)
            {
                case "georgian":
                    newTranslation.Georgian = word;
                    break;
                case "english":
                    newTranslation.English = word;
                    break;
                case "russian":
                    newTranslation.Russian = word;
                    break;
                default:
                    throw new ArgumentException("Invalid input language.");
            }

            switch (outputLanguage)
            {
                case "georgian":
                    newTranslation.Georgian = outputTranslation;
                    break;
                case "english":
                    newTranslation.English = outputTranslation;
                    break;
                case "russian":
                    newTranslation.Russian = outputTranslation;
                    break;
                default:
                    throw new ArgumentException("Invalid output language.");
            }

            translations.Add(newTranslation);
            SaveTranslationsToJson();
        }

        private void SaveTranslationsToJson()
        {
            TranslationsData data = new TranslationsData
            {
                Translations = translations
            };

            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "words.json");
            File.WriteAllText(jsonPath, json);
        }
    }

    public class TranslationEntry
    {
        public string Georgian { get; set; }
        public string English { get; set; }
        public string Russian { get; set; }

        public string GetPropertyValue(string language)
        {
            switch (language)
            {
                case "georgian":
                    return Georgian;
                case "english":
                    return English;
                case "russian":
                    return Russian;
                default:
                    throw new ArgumentException("Invalid language.");
            }
        }
    }

    public class TranslationsData
    {
        public List<TranslationEntry> Translations { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Translator translator = new Translator();
            translator.Run();
        }
    }
}
