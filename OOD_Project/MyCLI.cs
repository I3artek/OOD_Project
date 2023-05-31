using System.Collections;
using System.Text.RegularExpressions;

namespace OOD_Project;

public static class MyCLI
{
    private static Dictionary<string, Action<string>> Commands = new();
    private static List<IVisitable> AllObjectsList = new();
    private static Vector<IVisitable> AllObjectsVector;
    private static DoublyLinkedList<IVisitable> AllObjectsDoublyLinkedList;
    private static MaxHeap<IVisitable> AllObjectsMaxHeap;
    private static IMyCollection<IVisitable> AllObjects;
    private static CommandQueue CMDQ = new();
    private static CommandHistory CMDH = new();

    static MyCLI()
    {
        InitAllObjects();
        Commands.Add("list", List);
        Commands.Add("find", Find);
        Commands.Add("add", Add);
        Commands.Add("edit", Edit);
        Commands.Add("delete", Delete);
        //Commands.Add("queue commit", QueueCommit);
        //Commands.Add("queue print", QueuePrint);
        //Commands.Add("queue dismiss", QueueDismiss);
        //Commands.Add("queue export", QueueExport);
        //Commands.Add("queue load", QueueLoad);
        //todo Commands.Add("export", QueueExport);
        //todo Commands.Add("load", QueueLoad);
    }

    public static List<string> GetConditions(string command)
    {
        var conditions = new List<string>();
        var id = Regex.Match(command, ".+? .+? ").Length;
        var args = Regex.Matches(command[id..],
            "[^ ]+?[=<>][^\" ]+|[^ ]+?[=<>]\"[^\"]+\"");
        foreach (Match o in args)
        {
            conditions.Add(o.Value.Replace("\"", ""));
        }

        return conditions;
    }

    private static void InitAllObjects()
    {
        //necessary as the string representation
        //does not implement editing
        var city = new BaseCity(new StringCity());
        AllObjectsList.AddRange(city.vehicles);
        AllObjectsList.AddRange(city.drivers);
        AllObjectsList.AddRange(city.lines);
        AllObjectsList.AddRange(city.stops);
        UseVector();
        //UseMaxHeap();
    }

    private static void UseVector()
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
    
    private static void UseDoublyLinkedList()
    {
        if (AllObjectsDoublyLinkedList == null)
        {
            AllObjectsDoublyLinkedList = new DoublyLinkedList<IVisitable>();
            foreach (var visitable in AllObjectsList)
            {
                AllObjectsDoublyLinkedList.Add(visitable);
            }
        }

        AllObjects = AllObjectsDoublyLinkedList;
    }
    
    private static void UseMaxHeap()
    {
        if (AllObjectsMaxHeap == null)
        {
            AllObjectsMaxHeap= new MaxHeap<IVisitable>(
                (v1, v2) => false);
            foreach (var visitable in AllObjectsList)
            {
                AllObjectsMaxHeap.Add(visitable);
            }
        }

        AllObjects = AllObjectsMaxHeap;
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
        // if (action == "queue")
        // {
        //     action = 
        //         Regex.Match(command, "queue .+? ").Value[..^1];
        //     Commands[action](command);
        // }
        // else if (Commands.ContainsKey(action))
        // {
        //     CMDQ.Add(action, command);
        // }
        if (Commands.ContainsKey(action))
        {
            CMDH.AddCmd(action, command);
            Commands[action](command);
        }
        else if(action == "undo")
        {
            CMDH.Undo(AllObjects);
        }
        else if(action == "redo")
        {
            var (a, c) = CMDH.Redo(AllObjects);
            if (a != "")
            {
                Commands[a](c);
            }
        }
        else
        {
            Console.WriteLine($"There's no command \"{action}\"");
        }
    }

    private static void List(string command)
    {
        var isRedo = CMDH.IsRedo();
        if (!isRedo)
        {
            CMDH.SetAffectedObjectBefore(null);
            CMDH.SetAffectedObjectAfter(null);
        }

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
        var isRedo = CMDH.IsRedo();
        if (!isRedo)
        {
            CMDH.SetAffectedObjectBefore(null);
            CMDH.SetAffectedObjectAfter(null);
        }
        
        var predicate = CreatePredicate(command);
        try
        {
            MyAlgorithms.PrintIf(AllObjects, predicate);
        }
        catch (Exception e)
        {
            // ignored
        }
    }

    private static void Add(string command)
    {
        var isRedo = CMDH.IsRedo();
        if(!isRedo)
            CMDH.SetAffectedObjectBefore(null);

        var values = command.Split(" ");
        var created = false;
        try
        {
            var obj = ObjectModifier.Create(values[1], values[2]);
            if (obj == null) return;
            AllObjects.Add(obj);
            if(!isRedo)
                CMDH.SetAffectedObjectAfter(obj);
            created = true;
            TaskTesting.WriteLineWithColor("Created object:\n" + obj, ConsoleColor.Yellow);
        }
        catch (Exception e)
        {
            // ignored
        }
        finally
        {
            if (!created)
            {
                TaskTesting.WriteLineWithColor("Object not created", ConsoleColor.DarkRed);
            }
        }
    }

    private static void Edit(string command)
    {
        var isRedo = CMDH.IsRedo();
        
        var xd = command.Split(" ");
        var typeName = xd[1];

        var predicate = CreatePredicate(command);

        try
        {
            //check if the requirements identify one record uniquely
            var objectToEdit = MyAlgorithms.IdentifyUniquely(AllObjects, predicate);
            if (objectToEdit == null)
            {
                TaskTesting.WriteLineWithColor(
                    "Provided requirements do not identify one record uniquely!", ConsoleColor.Red);
                return;
            }
            TaskTesting.WriteLineWithColor(
                "Found object:\n" + objectToEdit, ConsoleColor.DarkBlue);
            if(!isRedo)
                CMDH.SetAffectedObjectBefore(objectToEdit);

            if (ObjectModifier.Modify(typeName, objectToEdit))
            {
                TaskTesting.WriteLineWithColor(
                    "Modified object:\n" + objectToEdit, ConsoleColor.Yellow);
            }
            else
            {
                TaskTesting.WriteLineWithColor(
                    "Object not modified!", ConsoleColor.Red);
            }
            if(!isRedo)
                CMDH.SetAffectedObjectAfter(objectToEdit);
        }
        catch (Exception e)
        {
            // ignored
        }
    }

    private static void QueuePrint(string command)
    {
        Console.WriteLine(CMDQ);
    }

    private static void QueueCommit(string command)
    {
        var c = CMDQ.Pop();
        while (c != null)
        {
            TaskTesting.WriteLineWithColor(
                "Executing \"" + c.Value.Item2 + "\"\n", ConsoleColor.Green);
            Commands[c.Value.Item1](c.Value.Item2);
            c = CMDQ.Pop();
        }
    }

    private static void QueueExport(string command)
    {
        var args = command.Replace(
            "queue export ", "").Split(" ");
        switch (args[1])
        {
            case "XML":
                CMDQ.ExportToXML(args[0]);
                break;
            case "plaintext":
                CMDQ.ExportToPlainText(args[0]);
                break;
        }
    }

    private static void QueueLoad(string command)
    {
        var filename = command
            .Replace("queue load ", "")
            .Replace(" ", "");
        var ext = Regex.Match(filename, "[.].+").Value[1..];
        switch (ext)
        {
            case "xml":
                CMDQ.LoadFromXML(filename);
                break;
            case "txt":
                CMDQ.LoadFromPlainText(filename);
                break;
        }
    }

    private static void Delete(string command)
    {
        var predicate = CreatePredicate(command);
        
        try
        {
            //check if the requirements identify one record uniquely
            var objectToDelete = MyAlgorithms.IdentifyUniquely(AllObjects, predicate);
            if (objectToDelete == null)
            {
                TaskTesting.WriteLineWithColor(
                    "Provided requirements do not identify one record uniquely!", ConsoleColor.Red);
                return;
            }
            TaskTesting.WriteLineWithColor(
                "Found object:\n" + objectToDelete, ConsoleColor.DarkBlue);
            
            CMDH.SetAffectedObjectBefore(objectToDelete);

            AllObjects.Remove(objectToDelete);
            TaskTesting.WriteLineWithColor(
                "Deleted the object from collection", ConsoleColor.DarkBlue);
        }
        catch (Exception e)
        {
            // ignored
        }
    }

    private static Func<IVisitable, bool> CreatePredicate(string command)
    {
        var xd = command.Split(" ");
        var typeVisitor = new TypeNameVisitor();
        var typeName = xd[1];

        var conditions = GetConditions(command);
        bool predicate(IVisitable o)
        {
            if (typeVisitor.GetTypeName(o) != typeName)
                return false;
            var compareVisitor = new CompareFieldVisitor();
            foreach (var condition in conditions)
            {
                try
                {
                    if (!compareVisitor.Compare(o, condition))
                    {
                        return false;
                    }
                }
                catch (Exception e)
                {
                    var missingField = Regex.Match(condition, "[^=<>]+").Value;
                    Console.WriteLine($"Class {typeName} does not have field {missingField}");
                    throw;
                }
            }

            return true;
        }

        return predicate;
    }

    private static void QueueDismiss(string command)
    {
        CMDQ.Clear();
    }
}
