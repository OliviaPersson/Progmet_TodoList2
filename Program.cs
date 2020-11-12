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
        /* CLASS: Activity
         * PURPOSE: Create an object with tree attributes and a constructor that will run when the class is initiated
         */
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
            bool moveItems = false;
            bool delTask = false;
            bool addTask = false;
            bool setState = false;
            bool quit = false;
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
                        quit = QuitProgram(moveItems, delTask, addTask, setState, quit);
                        break;
                    case "load":
                        fileName = LoadFile(commandWord, todoList);
                        break;
                    case "show":
                        ShowTodoList(commandWord, todoList);
                        break;
                    case "move":
                        moveItems = MoveItemsInList(commandWord, todoList);
                        break;
                    case "delete":
                        delTask = DeleteTask(commandWord, todoList);
                        break;
                    case "add":
                        addTask = AddNewActivity(commandWord, todoList);
                        break;
                    case "set":
                        setState = ChangeStateOfTask(commandWord, todoList);
                        break;
                    case "save":
                        fileName = Save(commandWord, todoList, fileName);
                        moveItems = false;
                        delTask = false;
                        addTask = false;
                        setState = false;
                        break;
                    default:
                        Console.WriteLine("{0} is an unknown command", commandWord[0]);
                        break;
                }
            } while (quit != true);
        }
        /* METHOD: QuitProgram (static)
         * PURPOSE: Quits program and asks user to save changes before closing the program
         * PARAMETERS: moveItems, delTask, addTask, setState - determens if there are changes in the list
         * RETURN VALUE: Returns true or false in Quit variable depending on whether a change has been made
         */
        private static bool QuitProgram(bool moveItems, bool delTask, bool addTask, bool setState, bool quit)
        {
            if (moveItems || delTask || addTask || setState)
            {
                Console.WriteLine("Vill du spara ändringarna 'ja/nej'?");
                string answer = Console.ReadLine();
                if (answer == "nej")
                {
                    Console.WriteLine("Bye!");
                    quit = true;
                }
                else if (answer == "ja")
                {
                    Console.WriteLine("Skriv 'save' för att spara");
                }
                else
                {
                    Console.WriteLine("{0} is an unknown command", answer);
                }
            }
            else
            {
                Console.WriteLine("Bye!");
                quit = true;
            }
            return quit;
        }
        /* METHOD: AddNewActivity (static)
         * PURPOSE: Adds new activity with date and title
         * PARAMETERS: commandWord - user input, todolist - List that contains todo items
         * RETURN VALUE: Returns the value true if new activity has been added
         */
        private static bool AddNewActivity(string[] commandWord, List<Activity> todoList)
        {
            todoList.Add(new Activity(commandWord[1], "v", commandWord[2]));
            return true;
        }
        /* METHOD: Save (static)
         * PURPOSE: Saves the changes
         * PARAMETERS: commandWord - user input, todolist - List that contains todo items, fileName - stores current filepath
         * RETURN VALUE: Returns file path in fileName variable
         */
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
        /* METHOD: ChangeStateOfTask (static)
         * PURPOSE: Changes the status of the task depending on input commandWord[1]
         * PARAMETERS: commandWord - user input, todolist - List that contains todo items
         * RETURN VALUE: Returns the value true if status has been updated
         */
        private static bool ChangeStateOfTask(string[] commandWord, List<Activity> todoList)
        {
            for (int i = 0; i < todoList.Count(); i++)
            {
                if (commandWord[1] == (i + 1).ToString())
                {
                    if (commandWord[2] == "avklarad")
                    {
                        todoList[i].state = "*";
                        return true;
                    }
                    else if (commandWord[2] == "pågående")
                    {
                        todoList[i].state = "P";
                        return true;
                    }
                    else if (commandWord[2] == "väntar")
                    {
                        todoList[i].state = "v";
                        return true;
                    }
                }
            }
            return false;
        }
        /* METHOD: DeleteTask (static)
         * PURPOSE: Delete task depending on input commandWord[1]
         * PARAMETERS: commandWord - user input, todolist - List that contains todo items
         * RETURN VALUE: Return true if status has been updated and false if not
         */
        private static bool DeleteTask(string[] commandWord, List<Activity> todoList)
        {
            for (int i = 0; i < todoList.Count(); i++)
            {
                if (commandWord[1] == (i + 1).ToString())
                {
                    todoList.Remove(todoList[i]);
                    return true;
                }
            }
            return false;
        }
        /* METHOD: MoveItemsInList (static)
         * PURPOSE: Moves an activity up or down depending on input in commandWord[1]
         * PARAMETERS: commandWord - user input, todolist - List that contains todo items
         * RETURN VALUE: Return true if activity in list has been updated and false if not
         */
        private static bool MoveItemsInList(string[] commandWord, List<Activity> todoList)
        {
            for (int i = 0; i < todoList.Count(); i++)
            {
                int position = i + 1;
                Activity item = todoList[i];
                if (commandWord[1].Equals(position.ToString()))
                {
                    if (commandWord[2] == "up")
                    {
                        todoList.RemoveAt(i);
                        todoList.Insert(i - 1, item);
                        return true;
                    }
                    else if (commandWord[2] == "down")
                    {
                        todoList.RemoveAt(i);
                        todoList.Insert(i + 1, item);
                        return true;
                    }
                }
            }
            return false;
        }
        /* METHOD: ShowTodoList (static)
         * PURPOSE: Shows todolist depending on input in commandWord[1]
         * PARAMETERS: commandWord - user input, todolist - List that contains todo items
         */
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
        /* METHOD: LoadFile (static)
         * PURPOSE: Read file line by line and split line '#' and add in List Activity todolist
         * PARAMETERS:  commandWord - user input, todolist - List that will contain todo items
         * RETURN VALUE: Returns filepath in variable fileName
         */
        private static string LoadFile(string[] commandWord, List<Activity> todoList)
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
                    todoList.Add(A);
                }
            }
            return fileName;
        }
    }
}
