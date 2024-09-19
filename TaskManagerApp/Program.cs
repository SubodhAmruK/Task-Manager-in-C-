using System;
using System.Collections.Generic;
using System.IO;

class Task
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public string Priority { get; set; }
    public bool IsCompleted { get; set; }

    public Task(string name, string description, DateTime dueDate, string priority)
    {
        Name = name;
        Description = description;
        DueDate = dueDate;
        Priority = priority;
        IsCompleted = false;
    }

    public override string ToString()
    {
        return $"{Name}, {Description}, Due: {DueDate.ToShortDateString()}, Priority: {Priority}, Completed: {IsCompleted}";
    }
}

class TaskManager
{
    private List<Task> tasks = new List<Task>();
    private string filePath = "tasks.txt";

    public TaskManager()
    {
        LoadTasks();
    }

    public void AddTask(Task task)
    {
        tasks.Add(task);
        SaveTasks();
    }

    public void UpdateTask(int index, Task newTask)
    {
        if (index >= 0 && index < tasks.Count)
        {
            tasks[index] = newTask;
            SaveTasks();
        }
    }

    public void DeleteTask(int index)
    {
        if (index >= 0 && index < tasks.Count)
        {
            tasks.RemoveAt(index);
            SaveTasks();
        }
    }

    public void ListTasks()
    {
        for (int i = 0; i < tasks.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {tasks[i]}");
        }
    }

    public void MarkTaskCompleted(int index)
    {
        if (index >= 0 && index < tasks.Count)
        {
            tasks[index].IsCompleted = true;
            SaveTasks();
        }
    }

    public void SaveTasks()
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            foreach (var task in tasks)
            {
                writer.WriteLine($"{task.Name}|{task.Description}|{task.DueDate}|{task.Priority}|{task.IsCompleted}");
            }
        }
    }

    public void LoadTasks()
    {
        if (File.Exists(filePath))
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var data = line.Split('|');
                    var task = new Task(data[0], data[1], DateTime.Parse(data[2]), data[3])
                    {
                        IsCompleted = bool.Parse(data[4])
                    };
                    tasks.Add(task);
                }
            }
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        TaskManager taskManager = new TaskManager();
        Console.WriteLine("Welcome to Task Manager!");

        bool running = true;
        while (running)
        {
            Console.WriteLine("Options: 1. Add Task 2. List Tasks 3. Update Task 4. Delete Task 5. Mark Complete 6. Exit");
            Console.Write("Enter an option: ");
            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    Console.Write("Task Name: ");
                    string name = Console.ReadLine();
                    Console.Write("Description: ");
                    string description = Console.ReadLine();
                    Console.Write("Due Date (yyyy-mm-dd): ");
                    DateTime dueDate = DateTime.Parse(Console.ReadLine());
                    Console.Write("Priority (High/Medium/Low): ");
                    string priority = Console.ReadLine();
                    taskManager.AddTask(new Task(name, description, dueDate, priority));
                    break;
                case "2":
                    taskManager.ListTasks();
                    break;
                case "3":
                    Console.Write("Enter task number to update: ");
                    int updateIndex = int.Parse(Console.ReadLine()) - 1;
                    Console.Write("New Task Name: ");
                    string newName = Console.ReadLine();
                    Console.Write("New Description: ");
                    string newDescription = Console.ReadLine();
                    Console.Write("New Due Date (yyyy-mm-dd): ");
                    DateTime newDueDate = DateTime.Parse(Console.ReadLine());
                    Console.Write("New Priority (High/Medium/Low): ");
                    string newPriority = Console.ReadLine();
                    taskManager.UpdateTask(updateIndex, new Task(newName, newDescription, newDueDate, newPriority));
                    break;
                case "4":
                    Console.Write("Enter task number to delete: ");
                    int deleteIndex = int.Parse(Console.ReadLine()) - 1;
                    taskManager.DeleteTask(deleteIndex);
                    break;
                case "5":
                    Console.Write("Enter task number to mark complete: ");
                    int completeIndex = int.Parse(Console.ReadLine()) - 1;
                    taskManager.MarkTaskCompleted(completeIndex);
                    break;
                case "6":
                    running = false;
                    break;
            }
        }
    }
}
