using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Translate
{
    internal class Program
    {
        private static List<TranslationEntry> translations;

        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;

            LoadTranslations();

            Console.WriteLine("Available languages:");
            Console.WriteLine(string.Join(", ", typeof(TranslationEntry).GetProperties().Select(p => p.Name)));

            Console.WriteLine("Enter the input language:");
            string inputLanguage = Console.ReadLine().ToLower();

            Console.WriteLine("Enter the output language:");
            string outputLanguage = Console.ReadLine().ToLower();

            Console.WriteLine("Enter a word or phrase:");
            string input = Console.ReadLine();

            string translation = Translate(inputLanguage, outputLanguage, input);
            if (string.IsNullOrEmpty(translation))
            {
                Console.WriteLine("Translation not found.");
            }
            else
            {
                Console.WriteLine($"Translation: {translation}");
            }
        }

        static void LoadTranslations()
        {
            string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "words.json");
            string json = File.ReadAllText(jsonPath);
            var data = JsonConvert.DeserializeObject<TranslationsData>(json);
            translations = data.Translations;
        }

        static string Translate(string inputLanguage, string outputLanguage, string word)
        {
            var entry = translations.FirstOrDefault(t => t.GetPropertyValue(inputLanguage) == word);
            return entry?.GetPropertyValue(outputLanguage);
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
}
