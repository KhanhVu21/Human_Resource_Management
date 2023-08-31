namespace HR.Domain.Core.Entities;

public class CategoryCompensationBenefits
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public int? Status { get; set; }
    public DateTime? CreatedDate { get; set; }
    public double? AmountMoney { get; set; }
}