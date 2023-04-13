namespace OOD_Project;

public static class TaskTesting
{
    public static void WriteLineWithColor(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ResetColor();
    }
    
    //Expects provided function to print the results
    public static void PerformOnAll(IEnumerable<City> cities, Action<City, bool> f, 
        string action = "", bool PrintAll = false)
    {
        foreach (var city in cities)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Performing {action} on {city.GetRepresentationName()}\n");
            Console.ResetColor();
            f(city, PrintAll);
        }
    }
    public static void Task2(City c, bool PrintAll)
    {
        if (PrintAll)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Task: Print the lines (number and name) on which vehicles driven" + 
                              " by drivers with at least 10 years of experience are operated.\n");
            Console.ResetColor();
        }
        var selectedDrivers = 
            c.drivers.Where(driver => driver.GetSeniority() >= 10);
        if (PrintAll)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Drivers with at least 10 years of experience:\n");
            Console.ResetColor();
            foreach (var selectedDriver in selectedDrivers)
            {
                Console.WriteLine(selectedDriver);
            }
        }
        var selectedVehicleIds = new List<int>();
        //select all vehicleIds
        foreach (var selectedDriver in selectedDrivers)
        {
            for (var i = 0; i < selectedDriver.GetVehicleIdsCount(); i++)
            {
                selectedVehicleIds.Add(selectedDriver.GetVehicleId(i));
            }
        }

        var selectedVehicles = selectedVehicleIds
            .Distinct().Select(c.GetVehicle).ToList();

        if (PrintAll)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Vehicles they drive:\n");
            Console.ResetColor();
            foreach (var selectedVehicle in selectedVehicles)
            {
                Console.WriteLine(selectedVehicle);
            }
        }

        var selectedLineIds = new List<int>();
        foreach (var selectedVehicle in selectedVehicles)
        {
            switch (selectedVehicle)
            {
                case IBytebus selectedBytebus:
                    for (var i = 0; i < selectedBytebus.GetLineIdsCount(); i++)
                    {
                        selectedLineIds.Add(selectedBytebus.GetLineId(i));
                    }
                    break;
                case ITrambit selectedTrambit:
                    selectedLineIds.Add(selectedTrambit.GetLineId());
                    break;
            }
        }

        var selectedLines = selectedLineIds
            .Distinct().Select(c.GetLine).ToList();
        
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(PrintAll ? "Lines on which they operate:\n"
            : "Selected lines:\n");
        Console.ResetColor();
        foreach (var selectedLine in selectedLines)
        {
            Console.WriteLine(selectedLine);
        }
    }

    public static void Task3(City city, bool _)
    {
        var linked = new DoublyLinkedList<IStop>();
        var vector = new Vector<IStop>();
        foreach (var cityStop in city.stops)
        {
            linked.Add(cityStop);
            vector.Add(cityStop);
        }

        WriteLineWithColor("Testing", ConsoleColor.Blue);
        WriteLineWithColor("Find Linked Forward", ConsoleColor.Green);
        Console.WriteLine(MyAlgorithms.Find<IStop>(linked, Task3TempPred));
        WriteLineWithColor("Find Linked Reverse", ConsoleColor.Green);
        Console.WriteLine(MyAlgorithms.Find<IStop>(linked, Task3TempPred, false));
        WriteLineWithColor("Find Vector Forward", ConsoleColor.Green);
        Console.WriteLine(MyAlgorithms.Find<IStop>(linked, Task3TempPred));
        WriteLineWithColor("Find Vector Reverse", ConsoleColor.Green);
        Console.WriteLine(MyAlgorithms.Find<IStop>(linked, Task3TempPred, false));
        
        WriteLineWithColor("Testing", ConsoleColor.Blue);
        WriteLineWithColor("Print Linked Forward", ConsoleColor.Green);
        MyAlgorithms.Print<IStop>(linked, Task3TempPred);
        WriteLineWithColor("Print Linked Reverse", ConsoleColor.Green);
        MyAlgorithms.Print<IStop>(linked, Task3TempPred, false);
        WriteLineWithColor("Print Vector Forward", ConsoleColor.Green);
        MyAlgorithms.Print<IStop>(linked, Task3TempPred);
        WriteLineWithColor("Print Vector Reverse", ConsoleColor.Green);
        MyAlgorithms.Print<IStop>(linked, Task3TempPred, false);
    }

    private static bool Task3TempPred(IStop stop)
    {
        return stop != null && stop.GetLineId(0) == 16;
    }
    
    public static void Task4(City city, bool _)
    {
        var linked = new DoublyLinkedList<IStop>();
        var vector = new Vector<IStop>();
        var heap = new MaxHeap<IStop>(((stop, stop1) => false));
        foreach (var cityStop in city.stops)
        {
            linked.Add(cityStop);
            vector.Add(cityStop);
            heap.Add(cityStop);
        }

        WriteLineWithColor("Testing", ConsoleColor.Blue);
        WriteLineWithColor("Find Linked Forward with iterator", ConsoleColor.Green);
        Console.WriteLine(MyAlgorithms.Find<IStop>(linked.GetForwardEnumerator(), Task3TempPred));
        WriteLineWithColor("Find Linked Reverse iterator", ConsoleColor.Green);
        Console.WriteLine(MyAlgorithms.Find<IStop>(linked.GetReverseEnumerator(), Task3TempPred));
        WriteLineWithColor("Find heap Forward", ConsoleColor.Green);
        Console.WriteLine(MyAlgorithms.Find<IStop>(heap, Task3TempPred));
        WriteLineWithColor("Find heap Reverse", ConsoleColor.Green);
        Console.WriteLine(MyAlgorithms.Find<IStop>(heap.GetReverseEnumerator(), Task3TempPred));
        
        WriteLineWithColor("Testing", ConsoleColor.Blue);
        WriteLineWithColor("foreach heap Forward", ConsoleColor.Green);
        MyAlgorithms.ForEach(heap.GetForwardEnumerator(), Console.WriteLine);
        WriteLineWithColor("Print Vector Forward", ConsoleColor.Green);
        MyAlgorithms.Print<IStop>(vector, Task3TempPred);
        WriteLineWithColor("countif Vector Reverse", ConsoleColor.Green);
        Console.WriteLine(
            MyAlgorithms.CountIf<IStop>(vector.GetForwardEnumerator(), Task3TempPred));
    }
}
