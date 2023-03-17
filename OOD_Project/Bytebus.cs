using System.Text.RegularExpressions;

namespace OOD_Project;

public class Bytebus : Vehicle
{
    private List<int> line_ids;
    private List<Line> lines;
    private engineClassEnum engineClass;
    
    public Bytebus(string BytebusString)
    {
        this.Init(BytebusString);
    }

    private void Init(string BytebusString)
    {
        //BytebusString format:
        //"#<id>^<engineClass>*<line id>,..."

        this.id = int.Parse(
            Regex.Match(BytebusString, "#[^\\^]+")
            .Value[1..]);

        this.line_ids = new List<int>();
        var line_ids = Regex.Match(BytebusString, "\\*.+")
            .Value[1..].Split(",");
        foreach (var lineId in line_ids)
        {
            this.line_ids.Add(int.Parse(lineId));
        }

        var engineClass = Regex.Match(BytebusString, "\\^[^*]+")
            .Value[1..];
        Enum.TryParse(engineClass, out this.engineClass);
    }
}

enum engineClassEnum
{
    Byte5,
    bisel20,
    gibgaz
}