namespace OOD_Project;

public class CommandHistory
{
    private List<Command> to_undo = new();
    private List<Command> to_redo = new();

    
    public void AddCmd(string cmdName, string cmdString)
    {
        to_undo.Add(new Command(cmdName, cmdString));
    }
    
    public void SetAffectedObjectBefore(IVisitable? o)
    {
        to_undo.Last().affectedObjectBefore = o;
    }
    public void SetAffectedObjectAfter(IVisitable? o)
    {
        to_undo.Last().affectedObjectAfter = o;
    }

    public void Undo(IMyCollection<IVisitable> collection)
    {
        if(to_undo.Count == 0)
            return;
        if (to_undo.Last().affectedObjectAfter == to_undo.Last().affectedObjectBefore)
        {
            //no modification or both null
        }
        else if(to_undo.Last().affectedObjectAfter == null && to_undo.Last().affectedObjectBefore != null)
        {
            //was object, is null
            collection.Add(to_undo.Last().affectedObjectBefore);
        }
        else if(to_undo.Last().affectedObjectBefore == null && to_undo.Last().affectedObjectAfter != null)
        {
            //was null, is object
            collection.Remove(to_undo.Last().affectedObjectAfter);
        }
        else
        {
            //both are not null and different
            collection.Remove(to_undo.Last().affectedObjectAfter);
            collection.Add(to_undo.Last().affectedObjectBefore);
        }
        
        to_undo.RemoveAt(to_undo.Count - 1);
    }

    public (string, string) Redo()
    {
        if(to_redo.Count == 0)
            return ("", "");
        var tmp = ("", "");
        tmp.Item1 = to_redo.Last().cmdName;
        tmp.Item2 = to_redo.Last().cmdString;
        to_redo.RemoveAt(to_redo.Count - 1);
        return tmp;
    }
}

public class Command
{
    public string cmdName;
    public string cmdString;
    public IVisitable? affectedObjectBefore;
    public IVisitable? affectedObjectAfter;

    public Command(string cmdName, string cmdString)
    {
        this.cmdName = cmdName;
        this.cmdString = cmdString;
    }
}