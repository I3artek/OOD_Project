namespace OOD_Project;

public class StateVisitor : Visitor
{
    private IVisitable? stateObject = null;

    public IVisitable? CreateStateObject(IVisitable o)
    {
        o.Accept(this);
        return stateObject;
    }

    public void SetState(IVisitable o)
    {
        o.Accept(this);
    }

    public override void VisitLine(ILine line)
    {
        switch (stateObject)
        {
            case null:
                stateObject = new Line(line);
                break;
            case ILine lineState:
                line.SetCommonName(lineState.GetCommonName());
                line.SetNumberHex(lineState.GetNumberHex());
                line.SetNumberDec(lineState.GetNumberDec());
                break;
        }
    }

    public override void VisitStop(IStop stop)
    {
        switch (stateObject)
        {
            case null:
                stateObject = new Stop(stop);
                break;
            case IStop stopState:
                stop.SetId(stopState.GetId());
                stop.SetName(stopState.GetName());
                stop.SetType(stopState.GetType());
                break;
        }
    }

    public override void VisitBytebus(IBytebus bytebus)
    {
        switch (stateObject)
        {
            case null:
                stateObject = new Bytebus(bytebus);
                break;
            case IBytebus bytebusState:
                bytebus.SetId(bytebusState.GetId());
                bytebus.SetEngineClass(bytebusState.GetEngineClass());
                break;
        }
    }

    public override void VisitTrambit(ITrambit trambit)
    {
        switch (stateObject)
        {
            case null:
                stateObject = new Trambit(trambit);
                break;
            case ITrambit trambitState:
                trambit.SetId(trambitState.GetId());
                trambit.SetCarsNumber(trambitState.GetCarsNumber());
                break;
        }
    }

    public override void VisitVehicle(IVehicle vehicle)
    {
        throw new NotImplementedException();
    }

    public override void VisitDriver(IDriver driver)
    {
        switch (stateObject)
        {
            case null:
                stateObject = new Driver(driver);
                break;
            case IDriver driverState:
                driver.SetName(driverState.GetName());
                driver.SetSeniority(driverState.GetSeniority());
                driver.SetSurname(driverState.GetSurname());
                break;
        }
    }
}