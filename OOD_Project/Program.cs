using System;

namespace OOD_Project
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var StringBytezaria = new StringCity();
            var BaseBytezaria = new BaseCity(StringBytezaria);
            var HashMapBytezaria = new HashMapCity(BaseBytezaria);

            var Cities = new List<City>
            {
                BaseBytezaria,
                StringBytezaria,
                HashMapBytezaria
            };

            //TaskTesting.PerformOnAll(Cities, TaskTesting.Task2, "Task 2", true);
            TaskTesting.PerformOnAll(Cities, TaskTesting.Task4, "Task4", true);
        }
    }
}