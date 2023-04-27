namespace OOD_Project;

public class FieldVisitor : Visitor
{
    private string fieldName;
    private string? value;
    private bool hasField;
    
    public string? Get(IVisitable o, string fieldName)
    {
        this.fieldName = fieldName;
        o.Accept(this);
        if (!hasField) throw new Exception();
        return value;
    }
    public override void VisitLine(ILine line)
    {
        switch (fieldName)
        {
            case "NumberHex":
                value = line.GetNumberHex();
                hasField = true;
                break;
            case "NumberDec":
                value = Convert.ToString(line.GetNumberDec());
                hasField = true;
                break;
            case "CommonName":
                value = line.GetCommonName();
                hasField = true;
                break;
            default:
                hasField = false;
                break;
        }
    }

    public override void VisitStop(IStop stop)
    {
        switch (fieldName)
        {
            case "Id":
                value = Convert.ToString(stop.GetId());
                hasField = true;
                break;
            case "Name":
                value = stop.GetName();
                hasField = true;
                break;
            case "Type":
                value = Convert.ToString(stop.GetType());
                hasField = true;
                break;
            default:
                hasField = false;
                break;
        }
    }

    public override void VisitBytebus(IBytebus bytebus)
    {
        switch (fieldName)
        {
            case "Id":
                value = Convert.ToString(bytebus.GetId());
                hasField = true;
                break;
            case "EngineClass":
                value = Convert.ToString(bytebus.GetEngineClass());
                hasField = true;
                break;
            default:
                hasField = false;
                break;
        }
    }

    public override void VisitTrambit(ITrambit trambit)
    {
        switch (fieldName)
        {
            case "Id":
                value = Convert.ToString(trambit.GetId());
                hasField = true;
                break;
            case "CarsNumber":
                value = Convert.ToString(trambit.GetCarsNumber());
                hasField = true;
                break;
            default:
                hasField = false;
                break;
        }
    }

    public override void VisitVehicle(IVehicle vehicle)
    {
        switch (fieldName)
        {
            case "Id":
                value = Convert.ToString(vehicle.GetId());
                hasField = true;
                break;
            default:
                hasField = false;
                break;
        }
    }

    public override void VisitDriver(IDriver driver)
    {
        switch (fieldName)
        {
            case "Name":
                value = driver.GetName();
                hasField = true;
                break;
            case "Surname":
                value = driver.GetSurname();
                hasField = true;
                break;
            case "Seniority":
                value = Convert.ToString(driver.GetSeniority());
                hasField = true;
                break;
            default:
                hasField = false;
                break;
        }
    }
}