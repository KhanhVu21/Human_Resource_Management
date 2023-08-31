using HR.Application.Dto.Dto;
using HR.Application.Utilities.Utils;

namespace HR.Application.Interface.Interfaces;

public interface IEmployeeAllowanceRepository : IRepository<EmployeeAllowanceDto>
{
    Task<TemplateApi<EmployeeAllowanceDto>> GetByIdEmployee(Guid employeeId, int pageNumber,
        int pageSize);

    Task<TemplateApi<EmployeeAllowanceDto>> InsertEmployeeAndAllowance(Guid employeeId, List<Guid> idAllowance,
        Guid idUserCurrent,
        string fullName);
}