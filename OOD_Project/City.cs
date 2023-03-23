using System.Runtime.CompilerServices;

namespace OOD_Project;

public class City
{
    public List<Line> lines { get; private set; }
    public List<Stop> stops { get; private set; }
    public List<Vehicle> vehicles { get; private set; }
    public List<Driver> drivers { get; private set; }

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
        //contain references to stops and vehicles
        foreach (var csLine in cs.lines)
        {
            this.lines.Add(new Line(csLine, this));
        }
        
        //contain references to lines
        foreach (var csStop in cs.stops)
        {
            this.stops.Add(new Stop(csStop, this));
        }
        
        //contain references to lines
        foreach (var csBytebus in cs.bytebuses)
        {
            this.vehicles.Add(new Bytebus(csBytebus, this));
        }
        
        //contain references to lines
        foreach (var csTrambit in cs.trambits)
        {
            this.vehicles.Add(new Trambit(csTrambit, this));
        }
        
        //contain references to vehicles
        foreach (var csDriver in cs.drivers)
        {
            this.drivers.Add(new Driver(csDriver, this));
        }
        
        //we need to update references for lines
        //as they were initialized before stops and vehicles
        foreach (var line in lines)
        {
            line.UpdateRefs();
        }
    }
}

public class CityStrings
{
    public List<LineString> lines { get; private set; }
    public List<StopString> stops { get; private set; }
    public List<BytebusString> bytebuses { get; private set; }
    public List<TrambitString> trambits { get; private set; }
    public List<DriverString> drivers { get; private set; }

    public CityStrings()
    {
        InitializeWithExampleData(this);
    }

    private static void InitializeWithExampleData(CityStrings city)
    {
        city.lines = new List<LineString>
        {
            new("10(16)`SIMD`@1,2,3,8!11,12,13"),
            new("17(23)`Isengard-Mordor`@4,5,6,7!11,14,15"),
            new("E(14)`Museum of Plant`@7,8,9!14,21,22,23")
        };
        city.stops = new List<StopString>
        {
            new("#1(16)SPIR-V/bus"),
            new("#2(16)GLSL/tram"),
            new("#3(16)HLSL/other"),
            new("#4(23)DolGuldur/bus"),
            new("#5(23)AmonHen/bus"),
            new("#6(23)Gondolin/bus"),
            new("#7(23,14)Bitazon/tram"),
            new("#8(16,14)Bytecroft/bus"),
            new("#9(14)Maple/other")
        };
        city.bytebuses = new List<BytebusString>
        {
            new("#11^Byte5*16,23"),
            new("#12^bisel20*16"),
            new("#13^bisel20*16"),
            new("#14^gibgaz*23,14"),
            new("#15^gibgaz*23")
        };
        city.trambits = new List<TrambitString>
        {
            new("#21(1)14"),
            new("#22(2)14"),
            new("#23(6)14")
        };
        city.drivers = new List<DriverString>
        {
            new("Tomas Chairman(20)@11,21,15"),
            new("Tomas Thetank(4)@12,13,14"),
            new("Oru Bii(55)@22,23")
        };
    }
}
