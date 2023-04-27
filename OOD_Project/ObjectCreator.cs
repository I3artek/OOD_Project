namespace OOD_Project;

public static class ObjectCreator
{
    private static Dictionary<string, string> GetAvailableFields(string typeName)
    {
        return typeName switch
        {
            "Line" => new Dictionary<string, string>
            {
                { "NumberHex", "" },
                { "NumberDec", "" },
                { "CommonName", "" }
            },
            "Stop" => new Dictionary<string, string>
            {
                { "Id", "" },
                { "Name", "" },
                { "Type", "" }
            },
            "Bytebus" => new Dictionary<string, string>
            {
                { "Id", "" },
                { "EngineClass", "" }
            },
            "Trambit" => new Dictionary<string, string>
            {
                { "Id", "" },
                { "CarsNumber", "" }
            },
            "Driver" => new Dictionary<string, string>
            {
                { "Name", "" },
                { "Surname", "" },
                { "Seniority", "" }
            },
            _ => throw new Exception()
        };
    }
    
    public static IVisitable? Create(string typeName, string repName)
    {
        var reps = new List<string>
        {
            "Base",
            "String",
            "HashMap"
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

        switch (typeName)
        {
            case "Line":
                // var lineString = new LineString($"{availableFields["NumberHex"]}({availableFields["NumberDec"]})" 
                //                                + $"`{availableFields["CommonName"]}`@!");
                // return repName switch
                // {
                //     "Base" => new Line(lineString),
                //     "String" => lineString,
                //     "HashMap" => new LineHashMap(lineString)
                // };
                try
                {
                    var line = new Line(availableFields["NumberHex"], 
                        int.Parse(availableFields["NumberDec"]), 
                        availableFields["CommonName"]);
                    return repName switch
                    {
                        "Base" => line,
                        "String" => line.ToRep1(),
                        "HashMap" => new LineHashMap(line)
                    };
                }
                catch (Exception e)
                {
                    Console.WriteLine("Provided field values are in wrong format");
                    throw;
                }
            case "Stop":
                var stopString = new StopString($"#{availableFields["Id"]}(){availableFields["Name"]}" +
                                               $"/{availableFields["Type"]}");
                return repName switch
                {
                    "Base" => new Stop(stopString),
                    "String" => stopString,
                    "HashMap" => new StopHashMap(stopString)
                };
            case "Bytebus":
                var bytebusString = new BytebusString($"#{availableFields["Id"]}" +
                                                  $"^{availableFields["EngineClass"]}*");
                return repName switch
                {
                    "Base" => new Bytebus(bytebusString),
                    "String" => bytebusString,
                    "HashMap" => new BytebusHashMap(bytebusString)
                };
            case "Trambit":
                var trambitString = new TrambitString($"#{availableFields["Id"]}" +
                                                  $"({availableFields["CarsNumber"]})");
                return repName switch
                {
                    "Base" => new Trambit(trambitString),
                    "String" => trambitString,
                    "HashMap" => new TrambitHashMap(trambitString)
                };
            case "Driver":
                var driverString = new DriverString($"{availableFields["Name"]} {availableFields["Surname"]}" 
                                                 + $"({availableFields["Seniority"]}@");
                return repName switch
                {
                    "Base" => new Driver(driverString),
                    "String" => driverString,
                    "HashMap" => new DriverHashMap(driverString)
                };
            default:
                throw new Exception();
        }
        
    }

    private static IVisitable CreateStringRep(string typeName, Dictionary<string, string> availableFields)
    {
        switch (typeName)
        {
            case "Line":
                return new LineString($"{availableFields["NumberHex"]}({availableFields["NumberDec"]})" + $"`{availableFields["CommonName"]}@!`");
            case "Stop":
                return new StopString($"#{availableFields["Id"]}(){availableFields["Name"]}/{availableFields["Type"]}");
            case "Bytebus":
                return new BytebusString($"#{availableFields["Id"]}^{availableFields["EngineClass"]}*");
            case "Trambit":
                return new TrambitString($"#{availableFields["Id"]}({availableFields["CarsNumber"]})");
            case "Driver":
                return new DriverString($"{availableFields["Name"]} {availableFields["Surname"]}" + $"({availableFields["Seniority"]}@");
            default:
                throw new Exception();
        }
    }
}