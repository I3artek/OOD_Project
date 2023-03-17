namespace OOD_Project;

public class Bytebus : Vehicle
{
    //private int id;
    private List<Line> lines;
    private engineClassEnum engineClass;
}

enum engineClassEnum
{
    Byte5,
    bisel20,
    gibgaz
}