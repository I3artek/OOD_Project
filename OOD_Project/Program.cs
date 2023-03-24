using System;

namespace OOD_Project
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var BytezariaStrings = new CityStrings();
            var Bytezaria = new City(BytezariaStrings);
            var x = new LineHashMap(Bytezaria.lines[0]);
            Console.WriteLine(x.commonName);
        }
    }
}