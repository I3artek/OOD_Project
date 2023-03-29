using System.Text.RegularExpressions;

namespace OOD_Project;

public interface IStop
{
    public int GetId();
    public int GetLineId(int index);
    public string GetName();
    public typeEnum GetType();
    public int GetLineIdsCount();
}

public class Stop : IStop
{
    private int id { get; set; }
    private List<int> line_ids { get; set; }
    private List<Line> lines { get; set; } = new();
    private string name { get; set; }
    private typeEnum type { get; set; }
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

    public Stop(string s) : this(new StopString(s))
    {
    }
    
    public Stop(IStop s)
    {
        this.id = s.GetId();
        for (var i = 0; i < s.GetLineIdsCount(); i++)
        {
            this.line_ids.Add(s.GetLineId(i));
        }

        this.name = s.GetName();
        this.type = s.GetType();
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
            this.lines.Add(_city.GetLine(lineId) as Line);
            // foreach (var cityLine in _city.lines)
            // {
            //     if (cityLine.GetNumberDec() == lineId)
            //     {
            //         this.lines.Add(cityLine);
            //     }
            // }
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

    public int GetId() => this.id;
    public int GetLineId(int index) => this.line_ids[index];
    public string GetName() => this.name;
    public typeEnum GetType() => this.type;
    public int GetLineIdsCount() => this.line_ids.Count;
}

public enum typeEnum
{
    bus,
    tram,
    other
}

public class StopString : IStop
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
        var types = new List<string>(Enum.GetNames(typeof(typeEnum)));
        return Regex.IsMatch(s, "#\\d+\\((?:\\d+,)*\\d+\\).+/.+")
               && types.Any(s.Contains);
    }
    
    public override string ToString()
    {
        return new Stop(this).ToString();
    }

    public int GetId() => new Stop(this).GetId();
    public int GetLineId(int index) => new Stop(this).GetLineId(index);
    public string GetName() => new Stop(this).GetName();
    public typeEnum GetType() => new Stop(this).GetType();
    public int GetLineIdsCount() => new Stop(this).GetLineIdsCount();
}

public class StopHashMap : IStop
{
    private static readonly HashMap _hashMap = new();
    private readonly int id;
    private readonly HashedList line_ids = new(_hashMap);
    private readonly int name;
    private readonly int type;

    public StopHashMap(IStop s)
    {
        this.id = _hashMap.Add(s.GetId());
        for (var i = 0; i < s.GetLineIdsCount(); i++)
        {
            this.line_ids.Add(_hashMap.Add(s.GetLineId(i)));
        }

        this.name = _hashMap.Add(s.GetName());
        this.type = _hashMap.Add(s.GetType().ToString());
    }

    public override string ToString()
    {
        return new Stop(this).ToString();
    }

    public int GetId() => Convert.ToInt32(_hashMap[this.id]);
    public int GetLineId(int index) => this.line_ids[index];
    public string GetName() => _hashMap[this.name];
    public typeEnum GetType() => (typeEnum)Enum.Parse(typeof(typeEnum), _hashMap[this.type]);
    public int GetLineIdsCount() => this.line_ids.Count;
}
