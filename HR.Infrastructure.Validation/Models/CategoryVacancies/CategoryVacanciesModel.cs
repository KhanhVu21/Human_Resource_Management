namespace HR.Infrastructure.Validation.Models.CategoryVacancies;

public class CategoryVacanciesModel
{
    public Guid? Id { get; set; }
    public string? PositionName { get; set; }
    public int? NumExp { get; set; }
    public string? Degree { get; set; }
    public string? JobRequirements { get; set; }
    public string? JobDescription { get; set; }
}