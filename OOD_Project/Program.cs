using System;

namespace OOD_Project
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var BytezariaStrings = new CityStrings();
            var Bytezaria = new City(BytezariaStrings);
            var s = Bytezaria.lines[0];
            var x = new LineHashMap(s);
            var d = new Line(x);
            Console.WriteLine(x);
        }
    }
}