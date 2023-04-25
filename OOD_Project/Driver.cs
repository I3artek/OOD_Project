using System.Text.RegularExpressions;

namespace OOD_Project;

public interface IDriver : IVisitable
{
    public int GetVehicleId(int index);
    public string GetName();
    public string GetSurname();
    public int GetSeniority();
    public int GetVehicleIdsCount();
}

public class Driver : IDriver
{
    private List<int> vehicle_ids { get; set; } = new();
    private List<Vehicle> vehicles { get; set; }
    private string name { get; set; }
    private string surname { get; set; }
    private int seniority { get; set; }
    public City _city { get; private set; }

    public Driver(DriverString ds, City city)
    {
        this._city = city;
        this.Init(ds);
        this.InitRefs();
    }
    
    public Driver(DriverString ds)
    {
        this.Init(ds);
    }

    public Driver(string s) : this(new DriverString(s))
    {
    }
    
    public Driver(IDriver d)
    {
        for (var i = 0; i < d.GetVehicleIdsCount(); i++)
        {
            this.vehicle_ids.Add(d.GetVehicleId(i));
        }

        this.name = d.GetName();
        this.surname = d.GetSurname();
        this.seniority = d.GetSeniority();
    }

    private void Init(DriverString ds)
    {
        var driverString = ds.GetStringValue();
        //driverString format:
        //"<name> <surname>(<seniority>)@<vehicle id>,..."

        this.name = Regex.Match(driverString, "[^ ]+").Value;
        this.surname = Regex.Match(driverString, " [^(]+").Value[1..];
        this.seniority = int.Parse(
            Regex.Match(driverString, "\\([^)]+").Value[1..]);

        this.vehicle_ids = new List<int>();
        var vehicle_ids = Regex.Match(driverString, "@.+")
            .Value[1..].Split(",");
        foreach (var vehicleId in vehicle_ids)
        {
            this.vehicle_ids.Add(int.Parse(vehicleId));
        }
    }
    
    private void InitRefs()
    {
        this.vehicles = new List<Vehicle>();
        
        this.UpdateRefs();
    }

    public void UpdateRefs()
    {
        foreach (var vehicleId in vehicle_ids)
        {
            this.vehicles.Add(_city.GetVehicle(vehicleId) as Vehicle);
            // foreach (var cityVehicle in _city.vehicles)
            // {
            //     if (cityVehicle.GetId() == vehicleId)
            //     {
            //         this.vehicles.Add(cityVehicle);
            //     }
            // }
        }
    }
    
    public override string ToString()
    {
        var s = "Driver" + Environment.NewLine;
        s += "- vehicles: ";
        foreach (var vehicleId in vehicle_ids)
        {
            s += vehicleId + " ";
        }
        s += Environment.NewLine;
        s += "- name: " + this.name + Environment.NewLine;
        s += "- surname: " + this.surname + Environment.NewLine;
        s += "- seniority: " + this.seniority + Environment.NewLine;
        return s;
    }
    
    public string ToRep1String()
    {
        //driverString format:
        //"<name> <surname>(<seniority>)@<vehicle id>,..."
        var driverString = this.name + " "
                                     + this.surname + "("
                                     + this.seniority + ")@";
        foreach (var vehicleId in vehicle_ids)
        {
            driverString += vehicleId + ",";
        }
        //remove unnecessary colon
        driverString = driverString.Remove(driverString.Length - 1);
        return driverString;
    }

    public DriverString ToRep1()
    {
        return new DriverString(this.ToRep1String());
    }

    public int GetVehicleId(int index) => this.vehicle_ids[index];
    public string GetName() => this.name;
    public string GetSurname() => this.surname;
    public int GetSeniority() => this.seniority;
    public int GetVehicleIdsCount() => this.vehicle_ids.Count;
}

public class DriverString : IDriver
{
    private string value;

    public DriverString(string s)
    {
        if (IsValid(s))
        {
            this.value = s;
        }
        else
        {
            throw new InvalidDataFormatException(this, s);
        }
    }

    public string GetStringValue()
    {
        return this.value;
    }

    public static bool IsValid(string s)
    {
        return Regex.IsMatch(s, ".+ .+\\(\\d+\\)@(?:\\d+,)*\\d+");
    }
    
    public override string ToString()
    {
        return new Driver(this).ToString();
    }

    public int GetVehicleId(int index) => new Driver(this).GetVehicleId(index);
    public string GetName() => new Driver(this).GetName();
    public string GetSurname() => new Driver(this).GetSurname();
    public int GetSeniority() => new Driver(this).GetSeniority();
    public int GetVehicleIdsCount() => new Driver(this).GetVehicleIdsCount();
}

public class DriverHashMap : IDriver
{
    private static readonly HashMap _hashMap = new();
    private readonly HashedList vehicle_ids = new(_hashMap);
    private readonly int name;
    private readonly int surname;
    private readonly int seniority;

    public DriverHashMap(IDriver d)
    {
        for (var i = 0; i < d.GetVehicleIdsCount(); i++)
        {
            this.vehicle_ids.Add(_hashMap.Add(d.GetVehicleId(i)));
        }

        this.name = _hashMap.Add(d.GetName());
        this.surname = _hashMap.Add(d.GetSurname());
        this.seniority = _hashMap.Add(d.GetSeniority());
    }

    public override string ToString()
    {
        return new Driver(this).ToString();
    }

    public int GetVehicleId(int index) => vehicle_ids[index];
    public string GetName() => _hashMap[this.name];
    public string GetSurname() => _hashMap[this.surname];
    public int GetSeniority() => Convert.ToInt32(_hashMap[this.seniority]);
    public int GetVehicleIdsCount() => this.vehicle_ids.Count;
}
