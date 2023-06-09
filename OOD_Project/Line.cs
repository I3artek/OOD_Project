using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace OOD_Project;

public interface ILine : IVisitable
{
    public string GetNumberHex();
    public void SetNumberHex(string value);
    public int GetNumberDec();
    public void SetNumberDec(int value);
    public string GetCommonName();
    public void SetCommonName(string value);
    public int GetStopId(int index);
    public int GetVehicleId(int index);
    public int GetStopIdsCount();
    public int GetVehicleIdsCount();

    void IVisitable.Accept(Visitor visitor)
    {
        visitor.VisitLine(this);
    }
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
    public City _city { get; set; }

    public Line(LineString ls, City city)
    {
        this._city = city;
        this.Init(ls);
        this.InitRefs();
    }

    public Line(string numberHex, int numberDec, string commonName)
    {
        this.numberHex = numberHex;
        this.numberDec = numberDec;
        this.commonName = commonName;
    }

    public Line(LineString ls)
    {
        this.Init(ls);
    }

    public Line(ILine l)
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

    private void InitRefs()
    {
        this.UpdateRefs();
    }

    public void UpdateRefs()
    {
        foreach (var stopId in stop_ids)
        {
            if (_city.GetStop(stopId) is Stop)
            {
                this.stops.Add(_city.GetStop(stopId) as Stop);
            }
            // foreach (var cityStop in _city.stops)
            // {
            //     if (cityStop.GetId() == stopId)
            //     {
            //         this.stops.Add(cityStop);
            //     }
            // }
        }
        foreach (var vehicleId in vehicle_ids)
        {
            if (_city.GetVehicle(vehicleId) is Vehicle)
            {
                this.vehicles.Add(_city.GetVehicle(vehicleId) as Vehicle);
            }
            // foreach (var cityVehicle in _city.vehicles)
            // {
            //     if (cityVehicle.GetId() == vehicleId)
            //     {
            //         this.vehicles.Add(cityVehicle);
            //     }
            // }
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
    public void SetNumberHex(string value) => this.numberHex = value;

    public int GetNumberDec() => this.numberDec;
    public void SetNumberDec(int value) => this.numberDec = value;

    public string GetCommonName() => this.commonName;
    public void SetCommonName(string value) => this.commonName = value;

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
    public void SetNumberHex(string value)
    {
        throw new NotImplementedException();
    }

    public int GetNumberDec() => new Line(this).GetNumberDec();
    public void SetNumberDec(int value)
    {
        throw new NotImplementedException();
    }

    public string GetCommonName() => new Line(this).GetCommonName();
    public void SetCommonName(string value)
    {
        throw new NotImplementedException();
    }

    public int GetStopId(int index) => new Line(this).GetStopId(index);
    public int GetVehicleId(int index) => new Line(this).GetVehicleId(index);
    public int GetStopIdsCount() => new Line(this).GetStopIdsCount();
    public int GetVehicleIdsCount() => new Line(this).GetVehicleIdsCount();
}

public class LineHashMap : ILine
{
    private static readonly HashMap _hashMap = new();
    private int numberHex;
    private int numberDec;
    private int commonName;
    private readonly HashedList stop_ids = new(_hashMap);
    private readonly HashedList vehicle_ids = new(_hashMap);

    public LineHashMap(ILine l)
    {
        this.numberHex = _hashMap.Add(l.GetNumberHex());
        this.numberDec = _hashMap.Add(l.GetNumberDec());
        this.commonName = _hashMap.Add(l.GetCommonName());
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

    public string GetNumberHex() => _hashMap[numberHex];
    public void SetNumberHex(string value) => numberHex = _hashMap.Add(value);

    public int GetNumberDec() => Convert.ToInt32(_hashMap[numberDec]);
    public void SetNumberDec(int value) => numberDec = _hashMap.Add(value);

    public string GetCommonName() => _hashMap[commonName];
    public void SetCommonName(string value) => commonName = _hashMap.Add(value);

    public int GetStopId(int index) => this.stop_ids[index];
    public int GetVehicleId(int index) => this.vehicle_ids[index];
    public int GetStopIdsCount() => this.stop_ids.Count;
    public int GetVehicleIdsCount() => this.vehicle_ids.Count;
}
