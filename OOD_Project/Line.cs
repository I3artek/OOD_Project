using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace OOD_Project;

public class Line
{
    public string numberHex { get; private set; }
    public int numberDec { get; private set; }
    //numberDec is what is referred to as line_id in other places
    public string commonName { get; private set; }
    public List<int> stop_ids { get; private set; } = new();
    public List<Stop> stops { get; private set; } = new();
    public List<int> vehicle_ids { get; private set; } = new();
    public List<Vehicle> vehicles { get; private set; } = new();
    public City _city { get; private set; }

    public Line(LineString ls, City city)
    {
        this._city = city;
        this.Init(ls);
        this.InitRefs();
    }

    public Line(LineString ls)
    {
        this.Init(ls);
    }

    public Line(LineHashMap lhm)
    {
        this.Init(lhm);
    }

    public Line(string s) : this(new LineString(s))
    {
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
        
        var stop_ids = Regex.Match(lineString, "@[^!]+")
            .Value[1..].Split(",").ToList();
        foreach (var stopId in stop_ids)
        {
            this.stop_ids.Add(int.Parse(stopId));
        }
        
        var vehicle_ids = Regex.Match(lineString, "!.+")
            .Value[1..].Split(",").ToList();
        foreach (var vehicleId in vehicle_ids)
        {
            this.vehicle_ids.Add(int.Parse(vehicleId));
        }
    }

    private void Init(LineHashMap lhm)
    {
        this.numberHex = lhm.numberHex;
        this.numberDec = lhm.numberDec;
        this.commonName = lhm.commonName;
        for (var i = 0; i < lhm.stop_ids.Count; i++)
        {
            this.stop_ids.Add(lhm.stop_ids[i]);
        }

        for (var i = 0; i < lhm.vehicle_ids.Count; i++)
        {
            this.vehicle_ids.Add(lhm.vehicle_ids[i]);
        }
    }

    private void InitRefs()
    {
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
        return new LineString(this.ToRep1String());
    }
}

public class LineString : ILine
{
    private string value;

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
        return new Line(this).ToString();
    }
}

public class LineHashMap : HashMapRepresentation, ILine
{
    private static readonly HashMap _hashMap = new();
    private readonly int _numberHex;
    public string numberHex => _hashMap[_numberHex];
    private readonly int _numberDec;
    public int numberDec => Convert.ToInt32(_hashMap[_numberDec]);
    private readonly int _commonName;
    public string commonName => _hashMap[_commonName];
    public readonly HashedList stop_ids = new(_hashMap);
    public readonly HashedList vehicle_ids = new(_hashMap);

    public LineHashMap(Line l)
    {
        this._numberHex = _hashMap.Add(l.numberHex);
        this._numberDec = _hashMap.Add(l.numberDec);
        this._commonName = _hashMap.Add(l.commonName);
        foreach (var lStopId in l.stop_ids)
        {
            this.stop_ids.Add(_hashMap.Add(lStopId));
        }
        foreach (var lVehicleId in l.vehicle_ids)
        {
            this.vehicle_ids.Add(_hashMap.Add(lVehicleId));
        }
    }

    public override string ToString()
    {
        return new Line(this).ToString();
    }
}

public interface ILine
{
}
