using System.Text.RegularExpressions;

namespace OOD_Project;

public interface ITrambit : IVehicle
{
    public int GetCarsNumber();
    public void SetCarsNumber(int value);
    public int GetLineId();
    
    void IVisitable.Accept(Visitor visitor)
    {
        visitor.VisitTrambit(this);
    }
}

public class Trambit : Vehicle, ITrambit
{
    private int carsNumber { get; set; }
    private int line_id { get; set; }
    private Line line { get; set; }
    public City _city { get; private set; }

    public Trambit(TrambitString ts, City city) : base(ts)
    {
        this._city = city;
        this.Init(ts);
        this.InitRefs();
    }

    public Trambit(int id, int carsNumber) : base(id)
    {
        this.carsNumber = carsNumber;
    }
    
    public Trambit(TrambitString ts) : base(ts)
    {
        this.Init(ts);
    }

    public Trambit(string s) : this(new TrambitString(s))
    {
    }
    
    public Trambit(ITrambit t) : base(t)
    {
        this.carsNumber = t.GetCarsNumber();
        this.line_id = t.GetLineId();
    }

    private void Init(TrambitString ts)
    {
        var trambitString = ts.GetStringValue();
        //trambitString format:
        //"#<id>(<carsNumber>)<line id>"

        this.id = int.Parse(
            Regex.Match(trambitString, "#[^(]+")
                .Value[1..]);

        this.carsNumber = int.Parse(
            Regex.Match(trambitString, "\\([^)]+")
                .Value[1..]);

        this.line_id = int.Parse(
            Regex.Match(trambitString, "\\).+")
                .Value[1..]);
    }
    
    private void InitRefs()
    {
        this.UpdateRefs();
    }

    public override void UpdateRefs()
    {
        this.line = _city.GetLine(line_id) as Line;
        // foreach (var cityLine in _city.lines)
        // {
        //     if (cityLine.GetNumberDec() == line_id)
        //     {
        //         this.line = cityLine;
        //     }
        // }
    }
    
    public override string ToString()
    {
        var s = "Trambit" + Environment.NewLine;
        s += "- id: " + this.id + Environment.NewLine;
        s += "- carsNumber: " + this.carsNumber + Environment.NewLine;
        s += "- line: " + this.line_id + Environment.NewLine;
        return s;
    }
    
    public override string ToRep1String()
    {
        //trambitString format:
        //"#<id>(<carsNumber>)<line id>"
        var trambitString = "#" + this.id + "("
                            + this.carsNumber + ")"
                            + this.line_id;
        return trambitString;
    }

    public override TrambitString ToRep1()
    {
        return new TrambitString(this.ToRep1String());
    }

    public int GetCarsNumber() => this.carsNumber;
    public void SetCarsNumber(int value) => carsNumber = value;

    public int GetLineId() => this.line_id;
}

public class TrambitString : VehicleString, ITrambit
{
    private string value;

    public TrambitString(string s)
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
        return Regex.IsMatch(s, "#\\d+\\(\\d+\\)\\d+");
    }
    
    public override string ToString()
    {
        return new Trambit(this).ToString();
    }

    public override int GetId() => new Trambit(this).GetId();
    public int GetCarsNumber() => new Trambit(this).GetCarsNumber();
    public void SetCarsNumber(int value)
    {
        throw new NotImplementedException();
    }

    public int GetLineId() => new Trambit(this).GetLineId();
}

public class TrambitHashMap : VehicleHashMap, ITrambit
{
    private int carsNumber;
    private readonly int lineId;
    
    public TrambitHashMap(ITrambit t) : base(t)
    {
        this.carsNumber = _hashMap.Add(t.GetCarsNumber());
        this.lineId = _hashMap.Add(t.GetLineId());
    }

    public override string ToString()
    {
        return new Trambit(this).ToString();
    }

    public int GetCarsNumber() => Convert.ToInt32(_hashMap[this.carsNumber]);
    public void SetCarsNumber(int value) => carsNumber = _hashMap.Add(value);

    public int GetLineId() => Convert.ToInt32(_hashMap[this.lineId]);
}
