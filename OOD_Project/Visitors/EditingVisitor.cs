namespace OOD_Project;

public class EditingVisitor : Visitor
{
    private string fieldName;
    private string value;

    public void Set(IVisitable o, string fieldName, string value)
    {
        this.fieldName = fieldName;
        this.value = value;
        o.Accept(this);
    }
    public override void VisitLine(ILine line)
    {
        switch (fieldName)
        {
            case "NumberHex":
                line.SetNumberHex(value);
                break;
            case "NumberDec":
                line.SetNumberDec(Convert.ToInt32(value));
                break;
            case "CommonName":
                line.SetCommonName(value);
                break;
        }
    }

    public override void VisitStop(IStop stop)
    {
        switch (fieldName)
        {
            case "Id":
                stop.SetId(Convert.ToInt32(value));
                break;
            case "Name":
                stop.SetName(value);
                break;
            case "Type":
                stop.SetType(value);
                break;
        }
    }

    public override void VisitBytebus(IBytebus bytebus)
    {
        switch (fieldName)
        {
            case "Id":
                bytebus.SetId(Convert.ToInt32(value));
                break;
            case "EngineClass":
                bytebus.SetEngineClass(value);
                break;
        }
    }

    public override void VisitTrambit(ITrambit trambit)
    {
        switch (fieldName)
        {
            case "Id":
                trambit.SetId(Convert.ToInt32(value));
                break;
            case "CarsNumber": 
                trambit.SetCarsNumber(Convert.ToInt32(value));
                break;
        }
    }

    public override void VisitVehicle(IVehicle vehicle)
    {
        switch (fieldName)
        {
            case "Id": 
                vehicle.SetId(Convert.ToInt32(value));
                break;
        }
    }

    public override void VisitDriver(IDriver driver)
    {
        switch (fieldName)
        {
            case "Name":
                driver.SetName(value);
                break;
            case "Surname":
                driver.SetSurname(value);
                break;
            case "Seniority":
                driver.SetSeniority(Convert.ToInt32(value));
                break;
        }
    }
}