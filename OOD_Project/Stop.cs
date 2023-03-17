using System.Text.RegularExpressions;

namespace OOD_Project;

public class Stop
{
    private int id;
    private List<int> line_ids;
    private List<Line> lines;
    private string name;
    private typeEnum type;

    public Stop(string StopString)
    {
        this.Init(StopString);
    }

    private void Init(string StopString)
    {
        this.id = int.Parse(
            Regex.Match(StopString, "#[^(]+").Value[1..]);

        this.line_ids = new List<int>();
        var line_ids = Regex.Match(StopString, "\\([^)]+")
            .Value[1..].Split(",");
        foreach (var s in line_ids)
        {
            this.line_ids.Add(int.Parse(s));
        }

        this.name = Regex.Match(StopString, "\\)[^/]+").Value[1..];
        var type = Regex.Match(StopString, "/.+").Value[1..];
        Enum.TryParse<typeEnum>(type, out this.type);
    }
}

enum typeEnum
{
    bus,
    tram,
    other
}
