using System.Text.RegularExpressions;

namespace OOD_Project;

public interface IStop : IVisitable
{
    public int GetId();
    public void SetId(int value);
    public int GetLineId(int index);
    public string GetName();
    public void SetName(string value);
    public typeEnum GetType();
    public void SetType(typeEnum value);
    public void SetType(string value)
    {
        Enum.TryParse(value, out typeEnum _type);
        SetType(_type);
    }
    public int GetLineIdsCount();
    
    void IVisitable.Accept(Visitor visitor)
    {
        visitor.VisitStop(this);
    }
}

public class Stop : IStop
{
    private int id { get; set; }
    private List<int> line_ids { get; set; } = new();
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

    public Stop(int id, string name, typeEnum type)
    {
        this.id = id;
        this.name = name;
        this.type = type;
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
    public void SetId(int value) => id = value;

    public int GetLineId(int index) => this.line_ids[index];
    public string GetName() => this.name;
    public void SetName(string value) => name = value;

    public typeEnum GetType() => this.type;
    public void SetType(typeEnum value) => type = value;

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
    public void SetId(int value)
    {
        throw new NotImplementedException();
    }

    public int GetLineId(int index) => new Stop(this).GetLineId(index);
    public string GetName() => new Stop(this).GetName();
    public void SetName(string value)
    {
        throw new NotImplementedException();
    }

    public typeEnum GetType() => new Stop(this).GetType();
    public void SetType(typeEnum value)
    {
        throw new NotImplementedException();
    }

    public int GetLineIdsCount() => new Stop(this).GetLineIdsCount();
}

public class StopHashMap : IStop
{
    private static readonly HashMap _hashMap = new();
    private int id;
    private readonly HashedList line_ids = new(_hashMap);
    private int name;
    private int type;

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
    public void SetId(int value) => id = _hashMap.Add(value);

    public int GetLineId(int index) => this.line_ids[index];
    public string GetName() => _hashMap[this.name];
    public void SetName(string value) => name = _hashMap.Add(value);

    public typeEnum GetType() => (typeEnum)Enum.Parse(typeof(typeEnum), _hashMap[this.type]);
    public void SetType(typeEnum value) => type = _hashMap.Add(value.ToString());

    public int GetLineIdsCount() => this.line_ids.Count;
}
