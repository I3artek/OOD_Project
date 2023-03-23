using System.Text.RegularExpressions;

namespace OOD_Project;

public class Stop
{
    public int id { get; private set; }
    public List<int> line_ids { get; private set; }
    public List<Line> lines { get; private set; }
    public string name { get; private set; }
    public typeEnum type { get; private set; }
    public City _city { get; private set; }

    public Stop(StopString ss, City city)
    {
        this._city = city;
        this.Init(ss);
        this.InitRefs();
    }
    
    public Stop(StopString ss)
    {
        this.Init(ss);
    }

    private void Init(StopString ss)
    {
        var stopString = ss.GetStringValue();
        //stopString format:
        //"#<id>(<line id>,...)<name>/<type>"
        
        this.id = int.Parse(
            Regex.Match(stopString, "#[^(]+").Value[1..]);

        this.line_ids = new List<int>();
        var line_ids = Regex.Match(stopString, "\\([^)]+")
            .Value[1..].Split(",");
        foreach (var lineId in line_ids)
        {
            this.line_ids.Add(int.Parse(lineId));
        }

        this.name = Regex.Match(stopString, "\\)[^/]+").Value[1..];
        var type = Regex.Match(stopString, "/.+").Value[1..];
        Enum.TryParse(type, out typeEnum _type);
        this.type = _type;
    }

    private void InitRefs()
    {
        this.lines = new List<Line>();
        
        this.UpdateRefs();
    }

    public void UpdateRefs()
    {
        foreach (var lineId in line_ids)
        {
            foreach (var cityLine in _city.lines)
            {
                if (cityLine.numberDec == lineId)
                {
                    this.lines.Add(cityLine);
                }
            }
        }
    }
    
    public override string ToString()
    {
        var s = "Stop" + Environment.NewLine;
        s += "- id: " + this.id + Environment.NewLine;
        s += "- lines: ";
        foreach (var lineId in line_ids)
        {
            s += lineId + " ";
        }
        s += Environment.NewLine;
        s += "- name: " + this.name + Environment.NewLine;
        s += "- type: " + this.type + Environment.NewLine;
        return s;
    }
    
    public string ToRep1String()
    {
        //stopString format:
        //"#<id>(<line id>,...)<name>/<type>"
        var stopString = "#" + this.id + "(";
        foreach (var lineId in line_ids)
        {
            stopString += lineId + ",";
        }
        //remove unnecessary colon
        stopString = stopString.Remove(stopString.Length - 1);
        stopString += ")"
                      + this.name + "/"
                      + this.type;
        return stopString;
    }

    public StopString ToRep1()
    {
        return new StopString(this.ToRep1String());
    }
}

public enum typeEnum
{
    bus,
    tram,
    other
}

public class StopString
{
    private string value;

    public StopString(string s)
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
        return Regex.IsMatch(s, "#\\d+\\((?:\\d+,)*\\d+\\).+/.+");
    }
    
    public override string ToString()
    {
        return new Stop(this).ToString();
    }
}
