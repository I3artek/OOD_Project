namespace OOD_Project;

public class GetFieldVisitor : Visitor
{
    private string fieldName;
    private string value;
    
    public string Get(IVisitable o, string fieldName)
    {
        this.fieldName = fieldName;
        o.Accept(this);
        return value;
    }
    public override void VisitLine(ILine line)
    {
        value = fieldName switch
        {
            "NumberHex" => line.GetNumberHex(),
            "NumberDec" => Convert.ToString(line.GetNumberDec()),
            "CommonName" => line.GetCommonName()
        };
    }

    public override void VisitStop(IStop stop)
    {
        value = fieldName switch
        {
            "Id" => Convert.ToString(stop.GetId()),
            "Name" => stop.GetName(),
            "Type" => Convert.ToString(stop.GetType())
        };
    }

    public override void VisitBytebus(IBytebus bytebus)
    {
        value = fieldName switch
        {
            "Id" => Convert.ToString(bytebus.GetId()),
            "EngineClass" => Convert.ToString(bytebus.GetEngineClass())
        };
    }

    public override void VisitTrambit(ITrambit trambit)
    {
        value = fieldName switch
        {
            "Id" => Convert.ToString(trambit.GetId()),
            "CarsNumber" => Convert.ToString(trambit.GetCarsNumber())
        };
    }

    public override void VisitVehicle(IVehicle vehicle)
    {
        value = fieldName switch
        {
            "Id" => Convert.ToString(vehicle.GetId())
        };
    }

    public override void VisitDriver(IDriver driver)
    {
        value = fieldName switch
        {
            "Name" => driver.GetName(),
            "Surname" => driver.GetSurname(),
            "Seniority" => Convert.ToString(driver.GetSeniority())
        };
    }
}