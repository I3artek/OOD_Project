using System.Text.RegularExpressions;

namespace OOD_Project;

public abstract class Visitor
{
    public abstract void VisitLine(ILine line);
    public abstract void VisitStop(IStop stop);
    public abstract void VisitBytebus(IBytebus bytebus);
    public abstract void VisitTrambit(ITrambit trambit);
    public abstract void VisitVehicle(IVehicle vehicle);
    public abstract void VisitDriver(IDriver driver);
}

public interface IVisitable
{
    public void Accept(Visitor visitor);
}
