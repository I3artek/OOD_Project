using System.Text.RegularExpressions;

namespace OOD_Project;

public interface IBytebus : IVehicle
{
    public int GetLineId(int index);
    public engineClassEnum GetEngineClass();
    public int GetLineIdsCount();
    
    void IVisitable.Accept(Visitor visitor)
    {
        visitor.VisitBytebus(this);
    }
}

public class Bytebus : Vehicle, IBytebus
{
    private List<int> line_ids { get; set; } = new();
    private List<Line> lines { get; set; }
    private engineClassEnum engineClass { get; set; }
    public City _city { get; private set; }

    public Bytebus(BytebusString bs, City city) : base(bs)
    {
        this._city = city;
        this.Init(bs);
        this.InitRefs();
    }
    
    public Bytebus(BytebusString bs) : base(bs)
    {
        this.Init(bs);
    }

    public Bytebus(string s) : this(new BytebusString(s))
    {
    }
    
    public Bytebus(IBytebus b) : base(b)
    {
        for (var i = 0; i < b.GetLineIdsCount(); i++)
        {
            this.line_ids.Add(b.GetLineId(i));
        }

        this.engineClass = b.GetEngineClass();
    }

    private void Init(BytebusString bs)
    {
        var bytebusString = bs.GetStringValue();
        //bytebusString format:
        //"#<id>^<engineClass>*<line id>,..."

        this.id = int.Parse(
            Regex.Match(bytebusString, "#[^\\^]+")
            .Value[1..]);

        this.line_ids = new List<int>();
        var line_ids = Regex.Match(bytebusString, "\\*.+")
            .Value[1..].Split(",");
        foreach (var lineId in line_ids)
        {
            this.line_ids.Add(int.Parse(lineId));
        }

        var engineClass = Regex.Match(bytebusString, "\\^[^*]+")
            .Value[1..];
        Enum.TryParse(engineClass, out engineClassEnum _engineClass);
        this.engineClass = _engineClass;
    }

    private void InitRefs()
    {
        this.lines = new List<Line>();
        
        this.UpdateRefs();
    }

    public override void UpdateRefs()
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
        var s = "Bytebus" + Environment.NewLine;
        s += "- id: " + this.id + Environment.NewLine;
        s += "- lines: ";
        foreach (var lineId in line_ids)
        {
            s += lineId + " ";
        }
        s += Environment.NewLine;
        s += "- engineClass: " + this.engineClass + Environment.NewLine;
        return s;
    }
    
    public override string ToRep1String()
    {
        //bytebusString format:
        //"#<id>^<engineClass>*<line id>,..."
        var bytebusString = "#" + this.id + "^"
                            + this.engineClass + "*";
        foreach (var lineId in line_ids)
        {
            bytebusString += lineId + ",";
        }
        //remove unnecessary colon
        bytebusString = bytebusString.Remove(bytebusString.Length - 1);
        return bytebusString;
    }

    public override BytebusString ToRep1()
    {
        return new BytebusString(this.ToRep1String());
    }

    public int GetLineId(int index) => this.line_ids[index];
    public engineClassEnum GetEngineClass() => this.engineClass;
    public int GetLineIdsCount() => this.line_ids.Count;
}

public enum engineClassEnum
{
    Byte5,
    bisel20,
    gibgaz
}

public class BytebusString : VehicleString, IBytebus
{
    private string value;

    public BytebusString(string s)
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
        var types = new List<string>(Enum.GetNames(typeof(engineClassEnum)));
        return Regex.IsMatch(s, "#\\d+\\^.+\\*(?:\\d+,)*\\d+")
               && types.Any(s.Contains);
    }
    
    public override string ToString()
    {
        return new Bytebus(this).ToString();
    }

    public int GetLineId(int index) => new Bytebus(this).GetLineId(index);
    public engineClassEnum GetEngineClass() => new Bytebus(this).GetEngineClass();
    public int GetLineIdsCount() => new Bytebus(this).GetLineIdsCount();
    public override int GetId() => new Bytebus(this).GetId();
}

public class BytebusHashMap : VehicleHashMap, IBytebus
{
    private readonly HashedList line_ids = new(_hashMap);
    private readonly int engineClass;

    public BytebusHashMap(IBytebus b) : base(b)
    {
        for (var i = 0; i < b.GetLineIdsCount(); i++)
        {
            this.line_ids.Add(_hashMap.Add(b.GetLineId(i)));
        }

        this.engineClass = _hashMap.Add(b.GetEngineClass().ToString());
    }

    public override string ToString()
    {
        return new Bytebus(this).ToString();
    }

    public int GetLineId(int index) => this.line_ids[index];
    public engineClassEnum GetEngineClass() => 
        (engineClassEnum)Enum.Parse(typeof(engineClassEnum), _hashMap[this.engineClass]);
    public int GetLineIdsCount() => this.line_ids.Count;
}
