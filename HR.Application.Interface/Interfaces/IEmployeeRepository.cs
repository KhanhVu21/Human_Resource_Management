using HR.Application.Dto.Custom;
using HR.Application.Dto.Dto;
using HR.Application.Utilities.Utils;

namespace HR.Application.Interface.Interfaces;

public interface IEmployeeRepository : IRepository<EmployeeDto>
{
    #region ===[ CRUD TABLE Employee ]=============================================================

    Task<TemplateApi<EmployeeAndBenefits>> GetEmployeeAndBenefits(Guid idEmployee);
    Task<TemplateApi<EmployeeAndAllowance>> GetEmployeeAndAllowance(Guid idEmployee);

    #endregion
}