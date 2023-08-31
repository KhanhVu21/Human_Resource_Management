﻿namespace HR.Application.Dto.Dto;

public class CategoryVacanciesDto
{
    public Guid Id { get; set; }
    public int? Status { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string? PositionName { get; set; }
    public int? NumExp { get; set; }
    public string? Degree { get; set; }
    public string? JobRequirements { get; set; }
    public string? JobDescription { get; set; }
}