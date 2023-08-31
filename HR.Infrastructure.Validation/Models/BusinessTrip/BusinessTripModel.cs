namespace HR.Infrastructure.Validation.Models.BusinessTrip;

public class BusinessTripModel
{
    public Guid? Id { get; set; }
    public string? Description { get; set; }
    public Guid IdUnit { get; set; }
    public string? UnitName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}