using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using Newtonsoft.Json;

namespace Books
{
    internal class BookManager
    {
        public List<Book> Books { get; set; }

        public BookManager()
        {
            Books = new List<Book>();
            LoadBooks();
        }

        public void AddBook()
        {
            string title = ReadTitle();
            string author = ReadAuthor();
            int year = ReadYear();

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(author))
            {
                WriteMessage("Title and author cannot be empty. Please try again.");
                return;
            }

            bool exists = false;

            for (int i = 0; i < Books.Count; i++)
            {
                if (Books[i].Title == title && Books[i].Author == author && Books[i].PublicationYear == year)
                {
                    WriteMessage("The book already exists in the collection.");
                    exists = true;
                    return;
                }
            }

            if (!exists)
            {
                Books.Add(new Book(title, author, year));
                WriteMessage("Book added succesfully");
                return;
            }
        }

        public void ShowBooks()
        {
            WriteMessage("Here is the list of books:");

            foreach (Book book in Books)
            {
                WriteMessage($"Title: {book.Title}, author: {book.Author}, publication year: {book.PublicationYear}.");
            }
        }

        public void FindByTitle()
        {
            string title = ReadTitle();
            bool exists = false;

            foreach (Book book in Books)
            {
                if (book.Title.Equals(title, StringComparison.OrdinalIgnoreCase))
                {
                    exists = true;
                    WriteMessage($"Title: {book.Title}, author: {book.Author}, publication year: {book.PublicationYear}.");
                }
            }

            if (!exists)
            {
                WriteMessage("Book with the given title doesn't exist.");
            }
        }

        public void Start()
        {
            WriteMessage("\nWhat do you want to do? Write one of the following: add, see, search");

            string action;
            do
            {
                action = Console.ReadLine().ToLower();
                if (action != "add" && action != "see" && action != "search")
                {
                    WriteMessage("Invalid action. Please enter one of the following: add, see, search");
                }
            } while (action != "add" && action != "see" && action != "search");

            if (action == "add")
            {
                AddBook();
            }
            else if (action == "see")
            {
                ShowBooks();
            }
            else
            {
                FindByTitle();
            }

            WriteMessage("\nDo you want to do any more actions? Y/N:");

            string answer;
            do
            {
                answer = Console.ReadLine().ToUpper();
                if (answer != "Y" && answer != "N")
                {
                    WriteMessage("Invalid input. Please enter 'Y' for Yes or 'N' for No:");
                }
            } while (answer != "Y" && answer != "N");

            if (answer == "N")
            {
                SaveBooks();
                return;
            }
            else
            {
                Start();
            }
        }

        private int ReadYear()
        {
            int year;
            WriteMessage("Please enter the publication date of the book:");
            while (!int.TryParse(Console.ReadLine(), out year))
            {
                Console.WriteLine("Invalid Input. Please enter a whole Number:");
            }
            return year;
        }

        private string ReadTitle()
        {
            string title;
            do
            {
                Console.WriteLine("Enter the title of the book:");
                title = Console.ReadLine();
            } while (!IsValidTitle(title));

            return title;
        }

        private string ReadAuthor()
        {
            string author;
            do
            {
                Console.WriteLine("Enter the author's name (letters only):");
                author = Console.ReadLine();
            } while (!IsValidAuthorName(author));

            return author;
        }

        private bool IsValidAuthorName(string name)
        {
            string pattern = @"^[a-zA-Z]+([-. ]?[a-zA-Z]+)*$";
            return !string.IsNullOrEmpty(name) && Regex.IsMatch(name, pattern);
        }

        private bool IsValidTitle(string title)
        {
            return !string.IsNullOrEmpty(title);
        }


        private void WriteMessage(string message)
        {
            Console.WriteLine(message);
        }


        private void SaveBooks()
        {
            string json = JsonConvert.SerializeObject(Books);
            File.WriteAllText("books.json", json);
            WriteMessage("Books saved successfully.");
        }

        private void LoadBooks()
        {
            if (File.Exists("books.json"))
            {
                string json = File.ReadAllText("books.json");
                Books = JsonConvert.DeserializeObject<List<Book>>(json);
            }
        }
    }
}