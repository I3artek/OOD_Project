namespace OOD_Project;

public static class MyAlgorithms
{
    public static T? Find<T>(IMyCollection<T> collection, Func<T, bool> predicate, bool searchDirection = true)
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
    
    public static void Print<T>(IMyCollection<T> collection, Func<T, bool> predicate, bool searchDirection = true)
        where T : class
    {
        var it = searchDirection ? 
            collection.GetForwardEnumerator() : collection.GetReverseEnumerator();

        do
        {
            if (predicate(it.Current)) Console.WriteLine(it.Current);
        } while (it.MoveNext()) ;
    }
}