using System.Security.Cryptography;
using System.Text;

namespace OOD_Project;

public static class HashThings
{
    public static int StringToInt(string s)
    {
        return BitConverter.ToInt32(
            SHA256.HashData(
                Encoding.ASCII.GetBytes(s)));
    }

    public static int IntToInt(int s)
    {
        return StringToInt(Convert.ToString(s));
    }
}