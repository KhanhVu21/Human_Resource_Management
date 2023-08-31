using HR.Domain.Core.Entities;

namespace HR.Application.Dto.Custom;

public class EmployeeAndBenefits
{
    public Employee? Employee { get; set; }
    public List<EmployeeBenefits>? EmployeeBenefits { get; set; }
}