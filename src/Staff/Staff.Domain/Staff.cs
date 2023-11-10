namespace EMS.Staff.Domain;

public sealed class Staff
{
    public int Id { get; init; }
    public int ManagerId { get; init; }
    public int PositionId { get; init; }
    public History? History { get; init; }
}