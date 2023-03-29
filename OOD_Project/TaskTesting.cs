namespace OOD_Project;

public static class TaskTesting
{
    //Expects provided function to print the results
    public static void PerformOnAll(IEnumerable<City> cities, Action<City, bool> f, 
        string action = "", bool PrintAll = false)
    {
        foreach (var city in cities)
        {
            Console.WriteLine($"Performing {action} on {city.GetRepresentationName()}\n");
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
}