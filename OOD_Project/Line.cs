using System.ComponentModel;
using System.Text.RegularExpressions;

namespace OOD_Project;

public class Line
{
    private string numberHex;
    private int numberDec;
    private string commonName;
    private List<int> stop_ids;
    private List<Stop> stops;
    private List<int> vehicle_ids;
    private List<Vehicle> vehicles;

    public Line(string LineString)
    {
        this.Init(LineString);
    }

    private void Init(string LineString)
    {
        //LineString format:
        //"<numberHex>(<numberDec>)`<commonName>`@<stop id>,...!<vehicle id>,..."
        
        this.numberHex = Regex.Match(LineString, "[^(]+").Value;
        this.numberDec = int.Parse(
            Regex.Match(LineString, "\\([^)]+")
                .Value[1..]);
        this.commonName = Regex.Match(LineString, "`[^`]+")
            .Value[1..];
        
        this.stop_ids = new List<int>();
        var stop_ids = Regex.Match(LineString, "@[^!]+")
            .Value[1..].Split(",").ToList();
        foreach (var stopId in stop_ids)
        {
            this.stop_ids.Add(int.Parse(stopId));
        }

        this.vehicle_ids = new List<int>();
        var vehicle_ids = Regex.Match(LineString, "!.+")
            .Value[1..].Split(",").ToList();
        foreach (var vehicleId in vehicle_ids)
        {
            this.vehicle_ids.Add(int.Parse(vehicleId));
        }
        
    }
}