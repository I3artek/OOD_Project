using System.Runtime.CompilerServices;

namespace OOD_Project;

public class City
{
    private List<Line> lines { get; set; }
    private List<Stop> stops { get; set; }
    private List<Vehicle> vehicles { get; set; }
    private List<Driver> drivers { get; set; }

    public City()
    {
        this.lines = new List<Line>();
        this.stops = new List<Stop>();
        this.vehicles = new List<Vehicle>();
        this.drivers = new List<Driver>();
    }
}

public class CityStrings
{
    private List<string> lines { get; set; }
    private List<string> stops { get; set; }
    private List<string> bytebuses { get; set; }
    private List<string> trambits { get; set; }
    private List<string> drivers { get; set; }

    public static void InitializeWithExampleData(CityStrings city)
    {
        city.lines = new List<string>
        {
            "10(16)`SIMD`@1,2,3,8!11,12,13",
            "17(23)`Isengard-Mordor`@4,5,6,7!11,14,15",
            "E(14)`Museum of Plant`@7,8,9!14,21,22,23"
        };
        city.stops = new List<string>
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
    }
}
