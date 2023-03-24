using System.Security.Cryptography;
using System.Text;
namespace OOD_Project;

public class HashMap
{
    private readonly Dictionary<int, string> _map = new();

    /// <summary>
    /// Add string s to the hashmap
    /// </summary>
    /// <param name="s">string</param>
    /// <returns> Hash value </returns>
    public int Add(string s)
    {
        var hashed = GetHash(s);
        this._map.Add(hashed, s);
        return hashed;
    }
    
    /// <summary>
    /// Converts s to string and adds it to the hashmap
    /// </summary>
    /// <param name="s">int</param>
    /// <returns> Hash value </returns>
    public int Add(int s)
    { 
        return Add(Convert.ToString(s));
    }

    public string this[int key] => _map[key];
    
    public string Get(int key) => this[key];

    public static int GetHash(string s)
    {
        return BitConverter.ToInt32(
            SHA256.HashData(
                Encoding.ASCII.GetBytes(s)));
    }
    
    public static int GetHash(int s)
    {
        return GetHash(Convert.ToString(s));
    }
}