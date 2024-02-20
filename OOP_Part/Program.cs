using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace StudentManagement
{
    internal class StudentManager
    {
        public List<Student> Students { get; set; }

        public StudentManager()
        {
            Students = new List<Student>();
            LoadStudents();
        }

        public void AddStudent()
        {
            int id = ReadId();
            string name = ReadName();
            char grade = ReadGrade();

            Student student = new Student(id, name, grade);
            if (!Students.Any(s => s.Id == id))
            {
                Students.Add(student);
                Console.WriteLine("Student added successfully.");
            }
            else
            {
                Console.WriteLine("Student with the same ID already exists.");
            }
        }

        public void ShowStudents()
        {
            Console.WriteLine("List of Students:");
            foreach (var student in Students)
            {
                Console.WriteLine($"ID: {student.Id}, Name: {student.Name}, Grade: {student.Grade}");
            }
        }

        public void ModifyStudent()
        {
            Console.Write("Enter ID of the student to modify: ");
            int id = Convert.ToInt32(Console.ReadLine());

            Student student = Students.FirstOrDefault(s => s.Id == id);
            if (student != null)
            {
                Console.Write("Enter new name: ");
                string newName = ReadName();

                Console.Write("Enter new grade: ");
                char newGrade = ReadGrade();

                student.Name = newName;
                student.Grade = newGrade;
                Console.WriteLine("Student modified successfully.");
            }
            else
            {
                Console.WriteLine("Student not found.");
            }
        }

        public void DeleteStudent()
        {
            Console.Write("Enter ID of the student to delete: ");
            int id = Convert.ToInt32(Console.ReadLine());

            Student student = Students.FirstOrDefault(s => s.Id == id);
            if (student != null)
            {
                Students.Remove(student);
                Console.WriteLine("Student deleted successfully.");
            }
            else
            {
                Console.WriteLine("Student not found.");
            }
        }

        public void Start()
        {
            bool continueProgram = true;

            while (continueProgram)
            {
                Console.WriteLine("\nOptions:");
                Console.WriteLine("1. Add Student");
                Console.WriteLine("2. Show All Students");
                Console.WriteLine("3. Modify Student");
                Console.WriteLine("4. Delete Student");

                Console.Write("Enter your choice: ");
                int choice;
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            AddStudent();
                            break;
                        case 2:
                            ShowStudents();
                            break;
                        case 3:
                            ModifyStudent();
                            break;
                        case 4:
                            DeleteStudent();
                            break;
                        default:
                            Console.WriteLine("Invalid choice.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid choice.");
                }

                Console.Write("Do you want to continue? (yes/no): ");
                string continueInput = Console.ReadLine().ToLower();
                continueProgram = continueInput == "yes";
            }

            SaveStudents();
        }

        private int ReadId()
        {
            int id;
            Console.Write("Enter Student ID: ");
            while (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Invalid input. Please enter a valid ID:");
            }
            return id;
        }

        private string ReadName()
        {
            string name;
            do
            {
                Console.Write("Enter Student Name: ");
                name = Console.ReadLine();
                if (!IsValidName(name))
                {
                    Console.WriteLine("Invalid input. Please enter a valid name (letters, spaces, hyphens, and apostrophes only).");
                }
            } while (!IsValidName(name));
            return name;
        }

        private char ReadGrade()
        {
            char grade;
            do
            {
                Console.Write("Enter Student Grade (A to F): ");
                grade = char.ToUpper(Console.ReadKey().KeyChar);
                Console.WriteLine();
                if (!IsValidGrade(grade))
                {
                    Console.WriteLine("Invalid input. Please enter a valid grade (A to F).");
                }
            } while (!IsValidGrade(grade));
            return grade;
        }

        private bool IsValidName(string name)
        {
            string namePattern = "^[a-zA-Z]+(?:[' -][a-zA-Z]+)*$";
            return !string.IsNullOrWhiteSpace(name) && Regex.IsMatch(name, namePattern);
        }

        private bool IsValidGrade(char grade)
        {
            string gradePattern = "^[A-F]$";
            return Regex.IsMatch(grade.ToString(), gradePattern);
        }

        private void SaveStudents()
        {
            string json = JsonConvert.SerializeObject(Students);
            File.WriteAllText("students.json", json);
            Console.WriteLine("Students saved successfully.");
        }

        private void LoadStudents()
        {
            if (File.Exists("students.json"))
            {
                string json = File.ReadAllText("students.json");
                Students = JsonConvert.DeserializeObject<List<Student>>(json);
                Console.WriteLine("Students loaded successfully.");
            }
        }
    }

    internal class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public char Grade { get; set; }

        public Student(int id, string name, char grade)
        {
            Id = id;
            Name = name;
            Grade = grade;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            StudentManager studentManager = new StudentManager();
            studentManager.Start();
        }
    }
}
