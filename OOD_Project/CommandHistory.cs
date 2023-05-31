namespace OOD_Project;

public class CommandHistory
{
    private List<Command> to_undo = new();
    private List<Command> to_redo = new();

    private bool is_redo = false;

    public bool IsRedo()
    {
        var tmp = is_redo;
        is_redo = false;
        return tmp;
    }
    
    public void AddCmd(string cmdName, string cmdString)
    {
        to_undo.Add(new Command(cmdName, cmdString));
    }
    
    public void SetAffectedObjectBefore(IVisitable? o)
    {
        if (to_undo.Last().cmdName == "edit")
        {
            var sv = new StateVisitor();
            to_undo.Last().affectedObjectBefore = sv.CreateStateObject(o!);
        }
        else
        {
            to_undo.Last().affectedObjectBefore = o;
        }
    }
    public void SetAffectedObjectAfter(IVisitable? o)
    {
        to_undo.Last().affectedObjectAfter = o;
    }

    public void Undo(IMyCollection<IVisitable> collection)
    {
        if(to_undo.Count == 0)
            return;

        var cmdToUndo = to_undo.Last();
        switch (cmdToUndo.cmdName)
        {
            case "list":
                //do nothing
                break;
            case "find":
                //do nothing
                break;
            case "add":
                collection.Remove(to_undo.Last().affectedObjectAfter!);
                break;
            case "edit":
                //for implementing more advanced redo:
                var s = new StateVisitor();
                //save current state (after editing)
                var tmpState = s.CreateStateObject(to_undo.Last().affectedObjectAfter!);

                var sv = new StateVisitor();
                //load the saved state from before edit
                sv.SetState(to_undo.Last().affectedObjectBefore!);
                //apply the changes to the object that was edited
                sv.SetState(to_undo.Last().affectedObjectAfter!);
                
                //for implementing more advanced redo:
                to_undo.Last().affectedObjectBefore = tmpState;
                
                break;
            case "delete":
                collection.Add(to_undo.Last().affectedObjectBefore!);
                break;
            default:
                break;
        }

        var tmp = to_undo.Last();
        to_redo.Add(tmp);
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
        is_redo = true;
        return tmp;
    }
    
    public (string, string) Redo(IMyCollection<IVisitable> collection)
    {
        if(to_redo.Count == 0)
            return ("", "");
        var tmp = ("", "");
        switch (to_redo.Last().cmdName)
        {
            //if add and edit are supposed to be redone
            //with memory of what user typed
            case "add":
                collection.Add(to_redo.Last().affectedObjectAfter!);
                return tmp;
            case "edit":
            {
                var sv = new StateVisitor();
                //load the saved state from before edit
                sv.SetState(to_redo.Last().affectedObjectBefore!);
                //apply the changes to the object that was edited
                sv.SetState(to_redo.Last().affectedObjectAfter!);
                return tmp;
            }
        }

        tmp.Item1 = to_redo.Last().cmdName;
        tmp.Item2 = to_redo.Last().cmdString;
        to_redo.RemoveAt(to_redo.Count - 1);
        is_redo = true;
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