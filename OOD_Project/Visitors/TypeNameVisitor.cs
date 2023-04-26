namespace OOD_Project;

public class TypeNameVisitor : Visitor
{
    private string TypeName;

    public string GetTypeName(IVisitable o)
    {
        if (o == null)
        {
            return null;
        }
        o.Accept(this);
        return TypeName;
    }
    public override void VisitLine(ILine line)
    {
        TypeName = "Line";
    }

    public override void VisitStop(IStop stop)
    {
        TypeName = "Stop";
    }

    public override void VisitBytebus(IBytebus bytebus)
    {
        TypeName = "Bytebus";
    }

    public override void VisitTrambit(ITrambit trambit)
    {
        TypeName = "Trambit";
    }

    public override void VisitVehicle(IVehicle vehicle)
    {
        TypeName = "Vehicle";
    }

    public override void VisitDriver(IDriver driver)
    {
        TypeName = "Driver";
    }
}