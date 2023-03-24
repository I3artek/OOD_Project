using System.Dynamic;
using System.Security.Cryptography;
using System.Text;
namespace OOD_Project;

public class HashMap
{
    private Dictionary<int, string> _map;

    public void Add(string s)
    {
        this._map.Add(GetHash(s), s);
    }
    
    public void Add(int s)
    {
        Add(Convert.ToString(s));
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