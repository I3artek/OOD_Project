using System.Text.RegularExpressions;

namespace OOD_Project;

public class Driver
{
    public List<int> vehicle_ids { get; private set; }
    public List<Vehicle> vehicles { get; private set; }
    public string name { get; private set; }
    public string surname { get; private set; }
    public int seniority { get; private set; }  
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
            foreach (var cityVehicle in _city.vehicles)
            {
                if (cityVehicle.id == vehicleId)
                {
                    this.vehicles.Add(cityVehicle);
                }
            }
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
}

public class DriverString
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
}
