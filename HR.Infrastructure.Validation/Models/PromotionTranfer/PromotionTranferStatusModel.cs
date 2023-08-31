namespace HR.Infrastructure.Validation.Models.PromotionTranfer;

public class PromotionTranferStatusModel
{
    public Guid Id { get; set; }
    public int Status { get; set; }
    public string? HrNote { get; set; }
    public string? DirectorNote { get; set; }
}