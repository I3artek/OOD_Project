using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace OOD_Project;

public interface ILine
{
    public string GetNumberHex();
    public int GetNumberDec();
    public string GetCommonName();
    public int GetStopId(int index);
    public int GetVehicleId(int index);
    public int GetStopIdsCount();
    public int GetVehicleIdsCount();
}

public class Line : ILine
{
    private string numberHex { get; set; }
    private int numberDec { get; set; }
    //numberDec is what is referred to as line_id in other places
    private string commonName { get; set; }
    private List<int> stop_ids { get; set; } = new();
    private List<Stop> stops { get; set; } = new();
    private List<int> vehicle_ids { get; set; } = new();
    private List<Vehicle> vehicles { get; set; } = new();
    private City _city { get; set; }

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

    private void Init(ILine l)
    {
        this.numberHex = l.GetNumberHex();
        this.numberDec = l.GetNumberDec();
        this.commonName = l.GetCommonName();
        for (var i = 0; i < l.GetStopIdsCount(); i++)
        {
            this.stop_ids.Add(l.GetStopId(i));
        }
        for (var i = 0; i < l.GetVehicleIdsCount(); i++)
        {
            this.vehicle_ids.Add(l.GetVehicleId(i));
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

    public string GetNumberHex() => this.numberHex;
    public int GetNumberDec() => this.numberDec;
    public string GetCommonName() => this.commonName;
    public int GetStopId(int index) => this.stop_ids[index];
    public int GetVehicleId(int index) => this.vehicle_ids[index];
    public int GetStopIdsCount() => this.stop_ids.Count;
    public int GetVehicleIdsCount() => this.vehicle_ids.Count;
}

public class LineString : ILine
{
    private readonly string value;

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
    
    public string GetNumberHex() => new Line(this).GetNumberHex();
    public int GetNumberDec() => new Line(this).GetNumberDec();
    public string GetCommonName() => new Line(this).GetCommonName();
    public int GetStopId(int index) => new Line(this).GetStopId(index);
    public int GetVehicleId(int index) => new Line(this).GetVehicleId(index);
    public int GetStopIdsCount() => new Line(this).GetStopIdsCount();
    public int GetVehicleIdsCount() => new Line(this).GetVehicleIdsCount();
}

public class LineHashMap : ILine
{
    private static readonly HashMap _hashMap = new();
    private readonly int _numberHex;
    private string numberHex => _hashMap[_numberHex];
    private readonly int _numberDec;
    private int numberDec => Convert.ToInt32(_hashMap[_numberDec]);
    private readonly int _commonName;
    private string commonName => _hashMap[_commonName];
    private readonly HashedList stop_ids = new(_hashMap);
    private readonly HashedList vehicle_ids = new(_hashMap);

    public LineHashMap(Line l)
    {
        this._numberHex = _hashMap.Add(l.GetNumberHex());
        this._numberDec = _hashMap.Add(l.GetNumberDec());
        this._commonName = _hashMap.Add(l.GetCommonName());
        for (var i = 0; i < l.GetStopIdsCount(); i++)
        {
            this.stop_ids.Add(_hashMap.Add(l.GetStopId(i)));
        }
        for (var i = 0; i < l.GetVehicleIdsCount(); i++)
        {
            this.vehicle_ids.Add(_hashMap.Add(l.GetVehicleId(i)));
        }
    }

    public override string ToString()
    {
        return new Line(this).ToString();
    }
    
    public string GetNumberHex() => this.numberHex;
    public int GetNumberDec() => this.numberDec;
    public string GetCommonName() => this.commonName;
    public int GetStopId(int index) => this.stop_ids[index];
    public int GetVehicleId(int index) => this.vehicle_ids[index];
    public int GetStopIdsCount() => this.stop_ids.Count;
    public int GetVehicleIdsCount() => this.vehicle_ids.Count;
}
