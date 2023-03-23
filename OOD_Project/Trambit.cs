using System.Text.RegularExpressions;

namespace OOD_Project;

public class Trambit : Vehicle
{
    public int carsNumber { get; private set; }
    public int line_id { get; private set; }
    public Line line { get; private set; }
    public City _city { get; private set; }
    public TrambitString rep1 { get; private set; }
    
    public Trambit(TrambitString ts, City city)
    {
        this._city = city;
        this.rep1 = ts;
        this.Init(ts);
        this.InitRefs();
        ts.rep0 = this;
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

    public void UpdateRefs()
    {
        foreach (var cityLine in _city.lines)
        {
            if (cityLine.numberDec == line_id)
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
}

public class TrambitString
{
    private string value;
    public Trambit rep0 { get; set; }
    private CityStrings _cityStrings;

    public TrambitString(string s)
    {
        this.value = s;
    }

    public string GetStringValue()
    {
        return this.value;
    }
    
    public override string ToString()
    {
        return rep0 != null ? rep0.ToString() : value;
    }
}
