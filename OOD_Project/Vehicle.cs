namespace OOD_Project;

public interface IVehicle : IVisitable
{
    public int GetId();
    public void SetId(int value);
    
    void IVisitable.Accept(Visitor visitor)
    {
        visitor.VisitVehicle(this);
    }
}

public abstract class Vehicle : IVehicle
{
    protected int id { get; set; }

    protected Vehicle(IVehicle v)
    {
        this.id = v.GetId();
    }

    protected Vehicle(int id)
    {
        this.id = id;
    }

    protected Vehicle(VehicleString vs)
    {
    }

    public abstract string ToRep1String();
    public abstract VehicleString ToRep1();
    public abstract void UpdateRefs();
    public int GetId() => this.id;
    public void SetId(int value) => id = value;
}

public abstract class VehicleString : IVehicle
{
    public abstract int GetId();
    public void SetId(int value)
    {
        throw new NotImplementedException();
    }
}

public abstract class VehicleHashMap : IVehicle
{
    protected static readonly HashMap _hashMap = new();
    protected int id;
    protected VehicleHashMap(IVehicle v)
    {
        this.id = _hashMap.Add(v.GetId());
    }
    public int GetId() => Convert.ToInt32(_hashMap[id]);
    public void SetId(int value) => id = _hashMap.Add(value);
}