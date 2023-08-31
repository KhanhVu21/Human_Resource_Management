namespace HR.Domain.Core.Entities;

public class BusinessTrip
{
    public Guid Id { get; set; }
    public DateTime? CreatedDate { get; set; }
    public int? Status { get; set; }
    public string? Description { get; set; }
    public Guid IdUserRequest { get; set; }
    public Guid IdUnit { get; set; }
    public string? UnitName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}