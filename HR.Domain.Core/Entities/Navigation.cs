namespace HR.Domain.Core.Entities;

public class Navigation
{
    public Guid Id { get; set; }
    public string MenuName { get; set; }
    public Guid? IdParent { get; set; }
    public int? Status { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string? Path { get; set; }
    public string? IconLink { get; set; }
    public string? MenuCode { get; set; }
    public int? Sort { get; set; }
}