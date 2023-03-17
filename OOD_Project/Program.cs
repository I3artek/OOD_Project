using System;

namespace OOD_Project
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var Lines_rep2 = new List<string>
            {
                "10(16)`SIMD`@1,2,3,8!11,12,13",
                "17(23)`Isengard-Mordor`@4,5,6,7!11,14,15",
                "E(14)`Museum of Plant`@7,8,9!14,21,22,23"
            };
            var ExampleLines = new List<Line>();
            var stops = new List<string>
            {
                "#1(16)SPIR-V/bus",
                "#2(16)GLSL/tram",
                "#3(16)HLSL/other",
                "#4(23)DolGuldur/bus",
                "#5(23)AmonHen/bus",
                "#6(23)Gondolin/bus",
                "#7(23,14)Bitazon/tram",
                "#8(16,14)Bytecroft/bus",
                "#9(14)Maple/other"
            };
            var xd = new List<Stop>();
            foreach (var s in stops)
            {
                xd.Add(new Stop(s));
            }
        }
    }
}