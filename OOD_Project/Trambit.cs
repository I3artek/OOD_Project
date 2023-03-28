using System.Text.RegularExpressions;

namespace OOD_Project;

public interface ITrambit
{
    public int GetCarsNumber();
    public int GetLineId();
}

public class Trambit : Vehicle, ITrambit
{
    private int carsNumber { get; set; }
    private int line_id { get; set; }
    private Line line { get; set; }
    public City _city { get; private set; }

    public Trambit(TrambitString ts, City city)
    {
        this._city = city;
        this.Init(ts);
        this.InitRefs();
    }
    
    public Trambit(TrambitString ts)
    {
        this.Init(ts);
    }

    public Trambit(string s) : this(new TrambitString(s))
    {
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
        foreach (var cityLine in _city.lines)
        {
            if (cityLine.GetNumberDec() == line_id)
            {
                this.line = cityLine;
            }
        }
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
    public int GetLineId() => new Trambit(this).GetLineId();
}
