namespace OOD_Project;

public interface IVehicle
{
    public int GetId();
}

public abstract class Vehicle : IVehicle
{
    protected int id { get; set; }

    public abstract string ToRep1String();
    public abstract VehicleString ToRep1();
    public abstract void UpdateRefs();
    public int GetId() => this.id;
}

public abstract class VehicleString : IVehicle
{
    public abstract int GetId();
}