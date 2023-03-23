namespace OOD_Project;

public class InvalidDataFormatException : Exception
{
    public InvalidDataFormatException(object o, string s)
    {
        switch (o)
        {
            case Line:
                Console.WriteLine("String is in the wrong format!");
                Console.WriteLine("Required: <numberHex>(<numberDec>)`<commonName>`@<stop id>,...!<vehicle id>,...");
                Console.WriteLine("Obtained: " + s);
                break;
            case Stop:
                Console.WriteLine("String is in the wrong format!");
                Console.WriteLine("Required: #<id>(<line id>,...)<name>/<type>");
                Console.WriteLine("Obtained: " + s);
                break;
            case Bytebus:
                Console.WriteLine("String is in the wrong format!");
                Console.WriteLine("Required: #<id>^<engineClass>*<line id>,...");
                Console.WriteLine("Obtained: " + s);
                break;
            case Trambit:
                Console.WriteLine("String is in the wrong format!");
                Console.WriteLine("Required: #<id>(<carsNumber>)<line id>,...");
                Console.WriteLine("Obtained: " + s);
                break;
            case Driver:
                Console.WriteLine("String is in the wrong format!");
                Console.WriteLine("Required: <name> <surname>(<seniority>)@<vehicle id>,...");
                Console.WriteLine("Obtained: " + s);
                break;
        }
    }
}