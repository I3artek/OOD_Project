namespace OOD_Project;

public static class MyAlgorithms
{
    public static T? Find<T>(IMyCollection<T> collection, 
        Func<T, bool> predicate, bool searchDirection = true)
        where T : class
    {
        var it = searchDirection ? 
            collection.GetForwardEnumerator() : collection.GetReverseEnumerator();
        do
        {
            if (predicate(it.Current)) return it.Current;
        } while (it.MoveNext()) ;

        return null;
    }
    
    public static void Print<T>(IMyCollection<T> collection, 
        Func<T, bool> predicate, bool searchDirection = true)
        where T : class
    {
        var it = searchDirection ? 
            collection.GetForwardEnumerator() : collection.GetReverseEnumerator();

        do
        {
            if (predicate(it.Current)) Console.WriteLine(it.Current);
        } while (it.MoveNext()) ;
    }
    
    public static T? Find<T>(IEnumerator<T> it, Func<T, bool> predicate)
        where T : class
    {
        do
        {
            if (predicate(it.Current)) return it.Current;
        } while (it.MoveNext()) ;

        return null;
    }
    
    public static void ForEach<T>(IEnumerator<T> it, Action<T> func)
        where T : class
    {
        do
        {
            func(it.Current);
        } while (it.MoveNext()) ;
    }
    
    public static int CountIf<T>(IEnumerator<T> it, Func<T, bool> predicate)
        where T : class
    {
        var count = 0;
        do
        {
            if (predicate(it.Current)) count++;
        } while (it.MoveNext());

        return count;
    }
    
    public static T? DoIf<T>(IMyCollection<T> collection, 
        Func<T, bool> predicate, Action<T> func)
        where T : class
    {
        var it = collection.GetForwardEnumerator();
        do
        {
            if (predicate(it.Current)) func(it.Current);
        } while (it.MoveNext()) ;

        return null;
    }
}