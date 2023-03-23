using System.Text.RegularExpressions;

namespace OOD_Project;

public class Bytebus : Vehicle
{
    public List<int> line_ids { get; private set; }
    public List<Line> lines { get; private set; }
    public engineClassEnum engineClass { get; private set; }
    public City _city { get; private set; }
    public BytebusString rep1 { get; private set; }

    public Bytebus(BytebusString bs, City city)
    {
        this._city = city;
        this.rep1 = bs;
        this.Init(bs);
        this.InitRefs();
        bs.rep0 = this;
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
        var bs = new BytebusString(this.ToRep1String())
        {
            rep0 = this
        };
        return bs;
    }
}

public enum engineClassEnum
{
    Byte5,
    bisel20,
    gibgaz
}

public class BytebusString : VehicleString
{
    private string value;
    public Bytebus rep0 { get; set; }
    private CityStrings _cityStrings;

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
        return Regex.IsMatch(s, "#\\d+\\^.+\\*(?:\\d+,)*\\d+");
    }
    
    public override string ToString()
    {
        return rep0 != null ? rep0.ToString() : value;
    }
}
