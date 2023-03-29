namespace OOD_Project;

public abstract class City
{
    public List<ILine> lines = new();
    public List<IStop> stops = new();
    public List<IVehicle> vehicles = new();
    public List<IDriver> drivers = new();

    public abstract string GetRepresentationName();

    public ILine GetLine(int lineId)
    {
        return lines.Find(line => line.GetNumberDec() == lineId);
    }
    public IStop GetStop(int stopId)
    {
        return stops.Find(stop => stop.GetId() == stopId);
    }
    public IVehicle GetVehicle(int vehicleId)
    {
        return vehicles.Find(vehicle => vehicle.GetId() == vehicleId);
    }
    //There are no references to Drivers anywhere
    //and they have no unique field
}

public class BaseCity : City
{
    // public new List<Line> lines = new();
    // public new List<Stop> stops = new();
    // public new List<Vehicle> vehicles = new();
    // public new List<Driver> drivers = new();

    public override string GetRepresentationName() => "Base representation";
    
    public BaseCity(StringCity cs)
    {
        InitFromStringCity(cs);
    }

    private void InitFromStringCity(StringCity cs)
    {
        //contain references to stops and vehicles
        foreach (var csLine in cs.lines)
        {
            this.lines.Add(new Line(csLine as LineString, this));
        }
        
        //contain references to lines
        foreach (var csStop in cs.stops)
        {
            this.stops.Add(new Stop(csStop as StopString, this));
        }

        //contain references to lines
        foreach (var csVehicle in cs.vehicles)
        {
            switch (csVehicle)
            {
                case BytebusString vehicle:
                    this.vehicles.Add(new Bytebus(vehicle, this));
                    break;
                case TrambitString vehicle:
                    this.vehicles.Add(new Trambit(vehicle, this));
                    break;
            }
        }
        
        //old
        // //contain references to lines
        // foreach (var csBytebus in cs.bytebuses)
        // {
        //     this.vehicles.Add(new Bytebus(csBytebus, this));
        // }
        //
        // //contain references to lines
        // foreach (var csTrambit in cs.trambits)
        // {
        //     this.vehicles.Add(new Trambit(csTrambit, this));
        // }
        
        //contain references to vehicles
        foreach (var csDriver in cs.drivers)
        {
            this.drivers.Add(new Driver(csDriver as DriverString, this));
        }
        
        //we need to update references for lines
        //as they were initialized before stops and vehicles
        foreach (var line in lines)
        {
            (line as Line).UpdateRefs();
        }
    }
}

public class StringCity : City
{
    // public new List<LineString> lines;
    // public new List<StopString> stops;
    //public List<BytebusString> bytebuses;
    //public List<TrambitString> trambits;
    // public new List<DriverString> drivers;
    
    public override string GetRepresentationName() => "Text representation";


    public StringCity()
    {
        InitializeWithExampleData(this);
    }

    private static void InitializeWithExampleData(StringCity city)
    {
        city.lines = new List<ILine>
        {
            new LineString("10(16)`SIMD`@1,2,3,8!11,12,13"),
            new LineString("17(23)`Isengard-Mordor`@4,5,6,7!11,14,15"),
            new LineString("E(14)`Museum of Plant`@7,8,9!14,21,22,23")
        };
        city.stops = new List<IStop>
        {
            new StopString("#1(16)SPIR-V/bus"),
            new StopString("#2(16)GLSL/tram"),
            new StopString("#3(16)HLSL/other"),
            new StopString("#4(23)DolGuldur/bus"),
            new StopString("#5(23)AmonHen/bus"),
            new StopString("#6(23)Gondolin/bus"),
            new StopString("#7(23,14)Bitazon/tram"),
            new StopString("#8(16,14)Bytecroft/bus"),
            new StopString("#9(14)Maple/other")
        };
        city.vehicles = new List<IVehicle>
        {
            new BytebusString("#11^Byte5*16,23"),
            new BytebusString("#12^bisel20*16"),
            new BytebusString("#13^bisel20*16"),
            new BytebusString("#14^gibgaz*23,14"),
            new BytebusString("#15^gibgaz*23"),
            new TrambitString("#21(1)14"),
            new TrambitString("#22(2)14"),
            new TrambitString("#23(6)14")
        };
        city.drivers = new List<IDriver>
        {
            new DriverString("Tomas Chairman(20)@11,21,15"),
            new DriverString("Tomas Thetank(4)@12,13,14"),
            new DriverString("Oru Bii(55)@22,23")
        };
    }
    
    //old implementation
    /*
    private static void InitializeWithExampleDataOld(StringCity city)
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
    */
}

public class HashMapCity : City
{
    public override string GetRepresentationName() => "HashMap representation";

    public HashMapCity(City c)
    {
        foreach (var cLine in c.lines)
        {
            this.lines.Add(new LineHashMap(cLine));
        }
        foreach (var cStop in c.stops)
        {
            this.stops.Add(new StopHashMap(cStop));
        }
        foreach (var cVehicle in c.vehicles)
        {
            switch (cVehicle)
            {
                case IBytebus cBytebus:
                    this.vehicles.Add(new BytebusHashMap(cBytebus));
                    break;
                case ITrambit cTrambit:
                    this.vehicles.Add(new TrambitHashMap(cTrambit));
                    break;
            }
        }
        foreach (var cDriver in c.drivers)
        {
            this.drivers.Add(new DriverHashMap(cDriver));
        }
    }
}
