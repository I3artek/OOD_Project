using System;

namespace OOD_Project
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var BytezariaStrings = new CityStrings();
            var Bytezaria = new City(BytezariaStrings);
            Console.WriteLine(new Line("E(14)`Museum of Plant`@7,8,9!14,21,22,23"));
        }
    }
}