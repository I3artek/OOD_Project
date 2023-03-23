namespace OOD_Project;

public abstract class Vehicle
{
    public int id { get; protected set; }

    public abstract string ToRep1String();
    public abstract VehicleString ToRep1();
    public abstract void UpdateRefs();
}

public abstract class VehicleString
{
    
}