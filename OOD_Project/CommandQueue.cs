using System.Xml.Serialization;

namespace OOD_Project;

public class CommandQueue
{
    private List<(string, string)> commands = new();

    public void Add(string commandName, string commandText)
    {
        commands.Add((commandName, commandText));
    }

    public (string, string)? Pop()
    {
        if (commands.Count == 0)
            return null;
        var temp = commands.First();
        commands.RemoveAt(0);
        return temp;
    }

    public void Clear()
    {
        commands.Clear();
    }

    public void ExportToPlainText(string path)
    {
        using var sw = File.CreateText(path);
        var c = Pop();
        while (c != null)
        {
            sw.WriteLine(c.Value.Item2);
            c = Pop();
        }
    }
    
    public void ExportToXML(string path)
    {
        var x = new XmlSerializer(typeof(List<(string,string)>));
        TextWriter writer = new StreamWriter(path);
        x.Serialize(writer, commands);
        commands.Clear();
    }

    public void LoadFromXML(string path)
    {
        if (!File.Exists(path))
        {
            TaskTesting.WriteLineWithColor("No file with such name!", ConsoleColor.Red);
            return;
        }
        var x = new XmlSerializer(typeof(List<(string,string)>));
        var reader = new StreamReader(path);
        var cd = x.Deserialize(reader);
        commands.AddRange(cd as IEnumerable<(string, string)>);
    }

    public void LoadFromPlainText(string path)
    {
        if (!File.Exists(path))
        {
            TaskTesting.WriteLineWithColor("No file with such name!", ConsoleColor.Red);
            return;
        }

        using var sr = File.OpenText(path);
        while (sr.ReadLine() is { } s)
        {
            Add(s.Split(" ")[0], s);
        }
    }

    public override string ToString()
    {
        var str = "Commands currently stored in the queue:\n";
        foreach (var (_, text) in commands)
        {
            str += text + "\n";
        }

        return str;
    }
}