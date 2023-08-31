namespace HR.Infrastructure.Validation.Models.OnLeave;

public class OnLeaveModel
{
    public Guid? Id { get; set; }
    public string? Description { get; set; }
    public Guid IdEmployee { get; set; }
    public Guid IdUnit { get; set; }
    public string? UnitName { get; set; }
}