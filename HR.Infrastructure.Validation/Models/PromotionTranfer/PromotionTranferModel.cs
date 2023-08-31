namespace HR.Infrastructure.Validation.Models.PromotionTranfer;

public class PromotionTranferModel
{
    public Guid? Id { get; set; }
    public string? Description { get; set; }
    public Guid IdUnit { get; set; }
    public Guid IdEmployee { get; set; }
    public string? UnitName { get; set; }
    public Guid IdPosition { get; set; }
    public string? PositionName { get; set; }
}