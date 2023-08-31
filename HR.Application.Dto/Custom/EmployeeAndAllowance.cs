using HR.Domain.Core.Entities;

namespace HR.Application.Dto.Custom;

public class EmployeeAndAllowance
{
    public Employee? Employee { get; set; }
    public List<Allowance>? Allowances { get; set; }
}