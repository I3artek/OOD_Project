using System.Collections;
using System.Text.RegularExpressions;

namespace OOD_Project;

public static class MyCLI
{
    private static Dictionary<string, Action<string>> Commands = new();
    private static List<IVisitable> AllObjectsList = new();
    private static Vector<IVisitable> AllObjectsVector;
    private static DoublyLinkedList<IVisitable> AllObjectsDoublyLinkedList;
    private static IMyCollection<IVisitable> AllObjects;

    static MyCLI()
    {
        InitAllObjects();
        Commands.Add("list", List);
    }

    private static void InitAllObjects()
    {
        var city = new StringCity();
        AllObjectsList.AddRange(city.vehicles);
        AllObjectsList.AddRange(city.drivers);
        AllObjectsList.AddRange(city.lines);
        AllObjectsList.AddRange(city.stops);
        UseVector();
    }

    public static void UseVector()
    {
        if (AllObjectsVector == null)
        {
            AllObjectsVector = new Vector<IVisitable>();
            foreach (var visitable in AllObjectsList)
            {
                AllObjectsVector.Add(visitable);
            }
        }

        AllObjects = AllObjectsVector;
    }
    
    public static void UseDoublyLinkedList()
    {
        if (AllObjectsVector == null)
        {
            AllObjectsDoublyLinkedList = new DoublyLinkedList<IVisitable>();
            foreach (var visitable in AllObjectsList)
            {
                AllObjectsDoublyLinkedList.Add(visitable);
            }
        }

        AllObjects = AllObjectsDoublyLinkedList;
    }

    public static void PrintCommands()
    {
        TaskTesting.WriteLineWithColor(
            "Available Commands:", ConsoleColor.Magenta);
        foreach (var (key, value) in Commands)
        {
            TaskTesting.WriteLineWithColor(
                key, ConsoleColor.Magenta);
        }
        TaskTesting.WriteLineWithColor(
            "exit", ConsoleColor.Magenta);
        Console.Write(">");
    }

    public static void Do(string command)
    {
        var action = 
            Regex.Match(command, ".+? ").Value[..^1];
        if (Commands.ContainsKey(action))
        {
            Commands[action](command);
        }
        else
        {
            Console.WriteLine($"There's no command \"{action}\"");
        }
    }

    private static void List(string command)
    {
        var typeVisitor = new TypeNameVisitor();
        var typeName = Regex.Match(command, " [^ ]+").Value[1..];
        TaskTesting.WriteLineWithColor(
            $"\nListing all objects of class {typeName}\n", 
            ConsoleColor.Yellow);
        MyAlgorithms.Print(AllObjects, 
            visitable => typeVisitor.GetTypeName(visitable) == typeName);
        TaskTesting.WriteLineWithColor(
            $"No more objects of class {typeName} to print\n", 
            ConsoleColor.Yellow);
    }

    private static void Find(string command)
    {
        var args = command.Split(" ");
    }
}
