using System;

namespace OOD_Project
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var BytezariaStrings = new CityStrings();
            var Bytezaria = new City(BytezariaStrings);
            
            Console.WriteLine(new DriverString(Bytezaria.lines.Last().ToRep1String()));
            Console.WriteLine(Bytezaria.lines.Last().ToRep1());
            Console.WriteLine(DriverString.IsValid("Tomas Thetank(4)@12,13,14"));
        }
    }
}