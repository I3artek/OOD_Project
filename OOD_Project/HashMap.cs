namespace OOD_Project;

public class HashMap
{
    public Dictionary<int, string> hashMap;

    protected void AddToHashMap(string s)
    {
        this.hashMap.Add(HashThings.StringToInt(s), s);
    }
    
    protected void AddToHashMap(int s)
    {
        this.hashMap.Add(HashThings.IntToInt(s), 
            Convert.ToString(s));
    }

    protected void InitApplicableFields(object o)
    {
        var xd  = o.GetType().GetProperties();
        foreach (var x in xd)
        {
            Console.WriteLine(x.Name);
        }
    }
}