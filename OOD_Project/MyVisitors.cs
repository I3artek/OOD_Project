using System.Text.RegularExpressions;

namespace OOD_Project;

public abstract class Visitor
{
    public abstract void Visit(IVisitable someObject);
}

public interface IVisitable
{
    public void Accept(Visitor visitor)
    {
        visitor.Visit(this);
    }

    public string? GetParameter(string name);
}

public static class TypeInfo
{
    public static Dictionary<string, List<string>> Fields = new();

    static TypeInfo()
    {
        Fields.Add("Line", new List<string>
        {
            
        });
    }
}

public class TypeNameVisitor : Visitor
{
    public string TypeName;
    public override void Visit(IVisitable someObject)
    {
        TypeName = Regex.Match(someObject.ToString(), ".+\n").Value[0..^1];
    }
}