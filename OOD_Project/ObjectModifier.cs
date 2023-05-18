namespace OOD_Project;

public static class ObjectModifier
{
    public static Dictionary<string, string> GetAvailableFields(string typeName)
    {
        return typeName switch
        {
            "Line" => new Dictionary<string, string>
            {
                { "NumberHex", "0" },
                { "NumberDec", "0" },
                { "CommonName", "placeholder name" }
            },
            "Stop" => new Dictionary<string, string>
            {
                { "Id", "0" },
                { "Name", "placeholder name" },
                { "Type", "bus" }
            },
            "Bytebus" => new Dictionary<string, string>
            {
                { "Id", "0" },
                { "EngineClass", "gibgaz" }
            },
            "Trambit" => new Dictionary<string, string>
            {
                { "Id", "0" },
                { "CarsNumber", "0" }
            },
            "Driver" => new Dictionary<string, string>
            {
                { "Name", "placeholder name" },
                { "Surname", "placeholder surname" },
                { "Seniority", "0" }
            },
            _ => throw new Exception()
        };
    }

    public static bool Modify(string typeName, IVisitable objectToEdit)
    {
        Dictionary<string, string> availableFields;
        try
        {
            availableFields = GetAvailableFields(typeName);
        }
        catch (Exception e)
        {
            Console.WriteLine($"No class with name {typeName}");
            throw;
        }

        var isModified = new Dictionary<string, bool>();
        foreach (var (key, _) in availableFields)
        {
            isModified.Add(key, false);
        }
        
        TaskTesting.WriteLineWithColor(
            "Available Fields:", ConsoleColor.Cyan);
        foreach (var availableFieldsKey in availableFields.Keys)
        {
            Console.WriteLine(availableFieldsKey);
        }

        Console.Write(">");
        var command = Console.ReadLine();
        while (command != "exit" && command != "done")
        {
            var values = command.Replace("\"", "")
                .Split("=");
            if (availableFields.ContainsKey(values[0]))
            {
                availableFields[values[0]] = values[1];
                isModified[values[0]] = true;
            }
            else
            {
                Console.WriteLine($"Class {typeName} does not have field {values[0]}");
            }
            Console.Write(">");
            command = Console.ReadLine();
        }

        if (command == "exit")
            return false;

        try
        {
            var Editor = new EditingVisitor();
            foreach (var (key, modified) in isModified)
            {
                if (modified)
                {
                    Editor.Set(objectToEdit, key, availableFields[key]);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Provided field values are in wrong format");
            throw;
        }

        return true;
    }
    
    public static IVisitable? Create(string typeName, string repName)
    {
        var reps = new List<string>
        {
            "base",//base
            "secondary"//hashmap
        };
        if (!reps.Contains(repName))
        {
            Console.WriteLine($"No representation with name {repName}");
            Console.WriteLine("Available names");
            reps.ForEach(Console.WriteLine);
            throw new Exception();
        }

        Dictionary<string, string> availableFields;
        try
        {
            availableFields = GetAvailableFields(typeName);
        }
        catch (Exception e)
        {
            Console.WriteLine($"No class with name {typeName}");
            throw;
        }
        TaskTesting.WriteLineWithColor(
            "Available Fields:", ConsoleColor.Cyan);
        foreach (var availableFieldsKey in availableFields.Keys)
        {
            Console.WriteLine(availableFieldsKey);
        }

        Console.Write(">");
        var command = Console.ReadLine();
        while (command != "exit" && command != "done")
        {
            var values = command.Replace("\"", "")
                .Split("=");
            if (availableFields.ContainsKey(values[0]))
            {
                availableFields[values[0]] = values[1];
            }
            else
            {
                Console.WriteLine($"Class {typeName} does not have field {values[0]}");
            }
            Console.Write(">");
            command = Console.ReadLine();
        }

        if (command == "exit")
            return null;

        try
        {
            switch (typeName)
            {
                case "Line":
                    var line = new Line(availableFields["NumberHex"], 
                            int.Parse(availableFields["NumberDec"]), 
                            availableFields["CommonName"]);
                    return repName switch
                    {
                        "base" => line,
                        "secondary" => new LineHashMap(line)
                    };
                case "Stop":
                    var stop = new Stop(int.Parse(availableFields["Id"]), availableFields["Name"], 
                        (typeEnum)Enum.Parse(typeof(typeEnum), availableFields["Type"]));
                    return repName switch
                    {
                        "base" => stop,
                        "secondary" => new StopHashMap(stop)
                    };
                case "Bytebus":
                    var bytebus = new Bytebus(int.Parse(availableFields["Id"]), 
                        (engineClassEnum)Enum.Parse(typeof(engineClassEnum), availableFields["EngineClass"]));
                    return repName switch
                    {
                        "base" => bytebus,
                        "secondary" => new BytebusHashMap(bytebus)
                    };
                case "Trambit":
                    var trambit = new Trambit(int.Parse(availableFields["Id"]), 
                        int.Parse(availableFields["CarsNumber"]));
                    return repName switch
                    {
                        "base" => trambit,
                        "secondary" => new TrambitHashMap(trambit)
                    };
                case "Driver":
                    var driver = new Driver(availableFields["Name"], availableFields["Surname"], 
                        int.Parse(availableFields["Seniority"]));
                    return repName switch
                    {
                        "base" => driver,
                        "secondary" => new DriverHashMap(driver)
                    };
                default:
                    throw new Exception();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Provided field values are in wrong format");
            throw;
        }
        
    }
}