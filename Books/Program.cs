using Books;
using System;
using System.Runtime.InteropServices;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the book managing system!");
        Console.WriteLine("You can do the following actions: add book, see the list of books, search book by title.");

        BookManager bookManager = new BookManager();

        bookManager.Start();
    }

}