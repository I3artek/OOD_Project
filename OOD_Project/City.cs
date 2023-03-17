using System.Runtime.CompilerServices;

namespace OOD_Project;

public class City
{
    private List<Line> lines { get; set; }
    private List<Stop> stops { get; set; }
    private List<Vehicle> vehicles { get; set; }
    private List<Driver> drivers { get; set; }

    public City(CityStrings cs)
    {
        this.lines = new List<Line>();
        this.stops = new List<Stop>();
        this.vehicles = new List<Vehicle>();
        this.drivers = new List<Driver>();
        
        InitFromStringCity(cs);
    }

    private void InitFromStringCity(CityStrings cs)
    {
        foreach (var csLine in cs.lines)
        {
            this.lines.Add(new Line(csLine));
        }

        foreach (var csStop in cs.stops)
        {
            this.stops.Add(new Stop(csStop));
        }
        
        foreach (var csBytebus in cs.bytebuses)
        {
            this.vehicles.Add(new Bytebus(csBytebus));
        }
        
        foreach (var csTrambit in cs.trambits)
        {
            this.vehicles.Add(new Trambit(csTrambit));
        }
        
        foreach (var csDriver in cs.drivers)
        {
            this.drivers.Add(new Driver(csDriver));
        }
    }
}

public class CityStrings
{
    public List<string> lines { get; private set; }
    public List<string> stops { get; private set; }
    public List<string> bytebuses { get; private set; }
    public List<string> trambits { get; private set; }
    public List<string> drivers { get; private set; }

    public CityStrings()
    {
        InitializeWithExampleData(this);
    }

    private static void InitializeWithExampleData(CityStrings city)
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
        city.bytebuses = new List<string>
        {
            "#11^Byte5*16,23",
            "#12^bisel20*16",
            "#13^bisel20*16",
            "#14^gibgaz*23,14",
            "#15^gibgaz*23"
        };
        city.trambits = new List<string>
        {
            "#21(1)14",
            "#22(2)14",
            "#23(6)14"
        };
        city.drivers = new List<string>
        {
            "Tomas Chairman(20)@11,21,15",
            "Tomas Thetank(4)@12,13,14",
            "Oru Bii(55)@22,23"
        };
    }
}
