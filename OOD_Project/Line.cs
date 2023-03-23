using System.Text.RegularExpressions;

namespace OOD_Project;

public class Line
{
    public string numberHex { get; private set; }
    public int numberDec { get; private set; }
    //numberDec is what is referred to as line_id in other places
    public string commonName { get; private set; }
    public List<int> stop_ids { get; private set; }
    public List<Stop> stops { get; private set; }
    public List<int> vehicle_ids { get; private set; }
    public List<Vehicle> vehicles { get; private set; }
    public City _city { get; private set; }
    public LineString rep1 { get; private set; }

    public Line(LineString ls, City city)
    {
        this._city = city;
        this.rep1 = ls;
        this.Init(ls);
        this.InitRefs();
        ls.rep0 = this;
    }

    private void Init(LineString ls)
    {
        var lineString = ls.GetStringValue();
        //lineString format:
        //"<numberHex>(<numberDec>)`<commonName>`@<stop id>,...!<vehicle id>,..."
        
        this.numberHex = Regex.Match(lineString, "[^(]+").Value;
        this.numberDec = int.Parse(
            Regex.Match(lineString, "\\([^)]+")
                .Value[1..]);
        this.commonName = Regex.Match(lineString, "`[^`]+")
            .Value[1..];
        
        this.stop_ids = new List<int>();
        var stop_ids = Regex.Match(lineString, "@[^!]+")
            .Value[1..].Split(",").ToList();
        foreach (var stopId in stop_ids)
        {
            this.stop_ids.Add(int.Parse(stopId));
        }

        this.vehicle_ids = new List<int>();
        var vehicle_ids = Regex.Match(lineString, "!.+")
            .Value[1..].Split(",").ToList();
        foreach (var vehicleId in vehicle_ids)
        {
            this.vehicle_ids.Add(int.Parse(vehicleId));
        }
    }

    private void InitRefs()
    {
        this.stops = new List<Stop>();
        this.vehicles = new List<Vehicle>();
        
        this.UpdateRefs();
    }

    public void UpdateRefs()
    {
        foreach (var stopId in stop_ids)
        {
            foreach (var cityStop in _city.stops)
            {
                if (cityStop.id == stopId)
                {
                    this.stops.Add(cityStop);
                }
            }
        }
        foreach (var vehicleId in vehicle_ids)
        {
            foreach (var cityVehicle in _city.vehicles)
            {
                if (cityVehicle.id == vehicleId)
                {
                    this.vehicles.Add(cityVehicle);
                }
            }
        }
    }

    public override string ToString()
    {
        var s = "Line" + Environment.NewLine;
        s += "- numberHex: " + this.numberHex + Environment.NewLine;
        s += "- numberDec: " + this.numberDec + Environment.NewLine;
        s += "- commonName: " + this.commonName + Environment.NewLine;
        s += "- stops: ";
        foreach (var stopId in stop_ids)
        {
            s += stopId + " ";
        }
        s += Environment.NewLine;
        s += "- vehicles: ";
        foreach (var vehicleId in vehicle_ids)
        {
            s += vehicleId + " ";
        }
        s += Environment.NewLine;
        return s;
    }

    public string ToRep1String()
    {
        //lineString format:
        //"<numberHex>(<numberDec>)`<commonName>`@<stop id>,...!<vehicle id>,..."
        var lineString = this.numberHex;
        lineString += "(" + this.numberDec + ")"
                      + "`" + this.commonName + "`"
                      + "@";
        foreach (var stopId in stop_ids)
        {
            lineString += stopId + ",";
        }
        //remove unnecessary colon
        lineString = lineString.Remove(lineString.Length - 1);
        lineString += "!";
        foreach (var vehicleId in vehicle_ids)
        {
            lineString += vehicleId + ",";
        }
        //remove unnecessary colon
        lineString = lineString.Remove(lineString.Length - 1);
        return lineString;
    }

    public LineString ToRep1()
    {
        var ls = new LineString(this.ToRep1String())
        {
            rep0 = this
        };
        return ls;
    }
}

public class LineString
{
    private string value;
    public Line rep0 { get; set; }
    private CityStrings _cityStrings;

    public LineString(string s)
    {
        if (IsValid(s))
        {
            this.value = s;
        }
        else
        {
            throw new InvalidDataFormatException(this, s);
        }
    }

    public string GetStringValue()
    {
        return this.value;
    }

    public static bool IsValid(string s)
    {
        return Regex.IsMatch(s, ".+\\(\\d+\\)`.+`@(?:\\d+,)*\\d+!(?:\\d+,)*\\d+");
    }

    public override string ToString()
    {
        return rep0 != null ? rep0.ToString() : value;
    }
}
