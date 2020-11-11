using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Progmet_TodoList2
{
    class Program
    {
        class Activity
        {
            public string date, state, title;

            public Activity(string date, string state, string title)
            {
                this.date = date;
                this.state = state;
                this.title = title;
            }
        }
        static void Main(string[] args)
        {
            string[] commandWord;
            List<Activity> todoList = new List<Activity>();
            string fileName = @"C:\Users\Olivi\todo.lis";

            Console.WriteLine("Hello and welcome to Todo List!");
            Console.WriteLine("Write 'quit' to quit!");

            do
            {
                Console.Write("> ");
                commandWord = Console.ReadLine().Split(' ');

                switch (commandWord[0])
                {
                    case "quit":
                        Console.WriteLine("Bye!");
                        break;
                    case "load":
                        fileName = LoadTodoFile(commandWord, todoList);
                        break;
                    case "show":
                        ShowTodoList(commandWord, todoList);
                        break;
                    case "move":
                        MoveItemsInList(commandWord, todoList);
                        break;
                    case "delete":
                        DeleteTask(commandWord, todoList);
                        break;
                    case "add":
                        AddNewActivity(commandWord, todoList);
                        break;
                    case "set":
                        ChangeState(commandWord, todoList);
                        break;
                    case "save":
                        fileName = Save(commandWord, todoList, fileName);
                        break;
                    default:
                        Console.WriteLine("{0} is an unknown command", commandWord[0]);
                        break;
                }
            } while (commandWord[0] != "quit");
        }

        private static void AddNewActivity(string[] commandWord, List<Activity> todoList)
        {
            todoList.Add(new Activity(commandWord[1], "v", commandWord[2]));
        }

        private static string Save(string[] commandWord, List<Activity> todoList, string fileName)
        {
            if (commandWord.Length > 1)
            {
                fileName = commandWord[1];
            }
            using (StreamWriter writer = new StreamWriter(fileName))
                for (int i = 0; i < todoList.Count(); i++)
                {
                    writer.WriteLine($"{todoList[i].date}#{todoList[i].state}#{todoList[i].title}");
                }
            return fileName;
        }

        private static void ChangeState(string[] commandWord, List<Activity> todoList)
        {
            for (int i = 0; i < todoList.Count(); i++)
            {
                if (commandWord[1] == (i + 1).ToString())
                {
                    if (commandWord[2] == "avklarad")
                    {
                        todoList[i].state = "*";
                    }
                    else if (commandWord[2] == "pågående")
                    {
                        todoList[i].state = "P";
                    }
                    else if (commandWord[2] == "väntar")
                    {
                        todoList[i].state = "v";
                    }
                }
            }
        }

        private static void DeleteTask(string[] commandWord, List<Activity> todoList)
        {
            for (int i = 0; i < todoList.Count(); i++)
            {
                if (commandWord[1] == (i + 1).ToString())
                {
                    todoList.Remove(todoList[i]);
                }
            }
        }

        private static void MoveItemsInList(string[] commandWord, List<Activity> todoList)
        {
            for (int i = 0; i < todoList.Count(); i++)
            {
                if (commandWord[2] == "up")
                {
                    int position = i + 1;
                    if (commandWord[1].Equals(position.ToString()))
                    {
                        Activity item = todoList[i];

                        todoList.RemoveAt(i);
                        todoList.Insert(i - 1, item);
                    }
                }
                else if (commandWord[2] == "down")
                {
                    int position = i + 1;
                    if (commandWord[1].Equals(position.ToString()))
                    {
                        Activity item = todoList[i];

                        todoList.RemoveAt(i);
                        todoList.Insert(i + 1, item);
                    }
                }
            }
        }

        private static void ShowTodoList(string[] commandWord, List<Activity> todoList)
        {
            Console.WriteLine("N  Datum  S Rubrik");
            Console.WriteLine("----------------------------------------");
            for (int i = 0; i < todoList.Count(); i++)
            {
                if (commandWord.Length > 1)
                {
                    if (commandWord[1] == "allt")
                    {
                        Console.WriteLine("{0}: {1,-7}{2,-2}{3,-20}", i + 1, todoList[i].date, todoList[i].state, todoList[i].title);
                    }
                    else if (commandWord[1] == "klara")
                    {
                        if (todoList[i].state == "*")
                        {
                            Console.WriteLine("{0}: {1,-7}{2,-2}{3,-20}", i + 1, todoList[i].date, todoList[i].state, todoList[i].title);
                        }
                    }
                }
                else
                {
                    if (todoList[i].state != "*")
                    {
                        Console.WriteLine("{0}: {1,-7}{2,-2}{3,-20}", i + 1, todoList[i].date, todoList[i].state, todoList[i].title);
                    }
                }
            }
            Console.WriteLine("----------------------------------------");
        }

        private static string LoadTodoFile(string[] commandWord, List<Activity> todoList)
        {
            string fileName = commandWord[1];
            Console.WriteLine("Reading file {0}", fileName);
            using (StreamReader file = new StreamReader(fileName))
            {
                while (file.Peek() >= 0)
                {
                    string line = file.ReadLine();
                    string[] word = line.Split('#');
                    Activity A = new Activity(word[0], word[1], word[2]);
                    //Console.WriteLine("{0} - {1} - {2}", A.date, A.state, A.title);
                    todoList.Add(A);
                }
            }
            return fileName;
        }
    }
}
