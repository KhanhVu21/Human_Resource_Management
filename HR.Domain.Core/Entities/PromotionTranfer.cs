namespace HR.Domain.Core.Entities;

public class PromotionTranfer
{
    public Guid Id { get; set; }
    public DateTime? CreatedDate { get; set; }
    public int? Status { get; set; }
    public string? Description { get; set; }
    public Guid IdUserRequest { get; set; }
    public Guid IdUnit { get; set; }
    public Guid IdEmployee { get; set; }
    public string? UnitName { get; set; }
    public Guid IdPosition { get; set; }
    public string? PositionName { get; set; }
}