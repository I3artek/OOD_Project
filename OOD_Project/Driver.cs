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
    public DriverString rep1 { get; private set; }

    public Driver(DriverString ds, City city)
    {
        this._city = city;
        this.rep1 = ds;
        this.Init(ds);
        this.InitRefs();
        ds.rep0 = this;
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
}

public class DriverString
{
    private string value;
    public Driver rep0 { get; set; }
    private CityStrings _cityStrings;

    public DriverString(string s)
    {
        this.value = s;
    }

    public string GetStringValue()
    {
        return this.value;
    }
    
    public override string ToString()
    {
        return rep0 != null ? rep0.ToString() : value;
    }
}
