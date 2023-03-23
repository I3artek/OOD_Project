namespace OOD_Project;

public class InvalidDataFormatException : Exception
{
    public InvalidDataFormatException(object o, string s)
    {
        switch (o)
        {
            case LineString or Line:
                Console.WriteLine("String is in the wrong format!");
                Console.WriteLine("Required: <numberHex>(<numberDec>)`<commonName>`@<stop id>,...!<vehicle id>,...");
                Console.WriteLine("Obtained: " + s);
                break;
            case StopString or Stop:
                Console.WriteLine("String is in the wrong format!");
                Console.WriteLine("Required: #<id>(<line id>,...)<name>/<type>");
                Console.WriteLine("Obtained: " + s);
                break;
            case BytebusString or Bytebus:
                Console.WriteLine("String is in the wrong format!");
                Console.WriteLine("Required: #<id>^<engineClass>*<line id>,...");
                Console.WriteLine("Obtained: " + s);
                break;
            case TrambitString or Trambit:
                Console.WriteLine("String is in the wrong format!");
                Console.WriteLine("Required: #<id>(<carsNumber>)<line id>,...");
                Console.WriteLine("Obtained: " + s);
                break;
            case DriverString or Driver:
                Console.WriteLine("String is in the wrong format!");
                Console.WriteLine("Required: <name> <surname>(<seniority>)@<vehicle id>,...");
                Console.WriteLine("Obtained: " + s);
                break;
        }
    }
}