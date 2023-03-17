namespace OOD_Project;

public class InvalidDataFormatException : Exception
{
    public InvalidDataFormatException(Object o)
    {
        switch (o)
        {
            case Line:
                Console.WriteLine("Line");
                break;
            case Stop:
                Console.WriteLine("Stop");
                break;
        }
    }
}