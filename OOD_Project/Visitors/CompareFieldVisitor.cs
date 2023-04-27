using System.Text.RegularExpressions;

namespace OOD_Project;

public class CompareFieldVisitor : Visitor
{
    private string fieldName;
    private string fieldValue;
    private int result;
    private bool hasField;

    private static int CompareInts(string s, int a)
    {
        var b = Convert.ToInt32(s);
        return b < a ? -1 : b > a ? 1 : 0;
    }
    
    private static int CompareStrings(string s1, string s2)
    {
        return string.Compare(s1, s2, StringComparison.OrdinalIgnoreCase) switch
        {
            > 0 => 1,
            < 0 => -1,
            _ => 0
        };
    }

    private static int CompareEnums(string s, Enum e)
    {
        return Convert.ToString(e) == s ? 0 : -2;
    }

    public bool Compare(IVisitable o, string comparison)
    {
        var compareSymbol = Regex.Match(comparison, "[=<>]").Value;
        var args = comparison.Split(compareSymbol);
        this.fieldName = args[0];
        this.fieldValue = args[1];
        o.Accept(this);
        if (!hasField) throw new Exception();
        switch (compareSymbol)
        {
            case "=":
                if (result == 0)
                    return true;
                break;
            case ">":
                if (result == -1)
                    return true;
                break;
            case "<":
                if (result == 1)
                    return true;
                break;
        }
        return false;
    }
    public override void VisitLine(ILine line)
    {
        switch (fieldName)
        {
            case "NumberHex":
                result = CompareStrings(fieldValue, line.GetNumberHex());
                hasField = true;
                break;
            case "NumberDec":
                result = CompareInts(fieldValue, line.GetNumberDec());
                hasField = true;
                break;
            case "CommonName":
                result = CompareStrings(fieldValue, line.GetCommonName());
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
                result = CompareInts(fieldValue, stop.GetId());
                hasField = true;
                break;
            case "Name":
                result = CompareStrings(fieldValue, stop.GetName());
                hasField = true;
                break;
            case "Type":
                result = CompareEnums(fieldValue, stop.GetType());
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
                result = CompareInts(fieldValue, bytebus.GetId());
                hasField = true;
                break;
            case "EngineClass":
                result = CompareEnums(fieldValue, bytebus.GetEngineClass());
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
                result = CompareInts(fieldValue, trambit.GetId());
                hasField = true;
                break;
            case "CarsNumber":
                result = CompareInts(fieldValue, trambit.GetCarsNumber());
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
                result = CompareInts(fieldValue, vehicle.GetId());
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
                result = CompareStrings(fieldValue, driver.GetName());
                hasField = true;
                break;
            case "Surname":
                result = CompareStrings(fieldValue, driver.GetSurname());
                hasField = true;
                break;
            case "Seniority":
                result = CompareInts(fieldValue, driver.GetSeniority());
                hasField = true;
                break;
            default:
                hasField = false;
                break;
        }
    }
}