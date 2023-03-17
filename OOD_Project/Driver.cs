using System.Text.RegularExpressions;

namespace OOD_Project;

public class Driver
{
    private List<int> vehicle_ids;
    private List<Vehicle> vehicles;
    private string name;
    private string surname;
    private int seniority;

    public Driver(string DriverString)
    {
        this.Init(DriverString);
    }

    private void Init(string DriverString)
    {
        //DriverString format:
        //"<name> <surname>(<seniority>)@<vehicle id>,..."

        this.name = Regex.Match(DriverString, "[^ ]+").Value;
        this.surname = Regex.Match(DriverString, " [^(]+").Value[1..];
        this.seniority = int.Parse(
            Regex.Match(DriverString, "\\([^)]+").Value[1..]);

        this.vehicle_ids = new List<int>();
        var vehicle_ids = Regex.Match(DriverString, "@.+")
            .Value[1..].Split(",");
        foreach (var vehicleId in vehicle_ids)
        {
            this.vehicle_ids.Add(int.Parse(vehicleId));
        }
    }
}