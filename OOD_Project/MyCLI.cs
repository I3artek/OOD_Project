using System.Collections;

namespace OOD_Project;

public static class MyCLI
{
    public static Dictionary<string, Delegate> Commands = new();
    public static City AllObjects;

    static MyCLI()
    {
        Commands.Add("list", List);
    }

    public static void List(string command)
    {
        
    }
}
