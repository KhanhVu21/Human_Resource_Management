using HR.Application.Dto.Dto;
using HR.Application.Utilities.Utils;

namespace HR.Application.Interface.Interfaces;

public interface IEmployeeBenefitsRepository : IRepository<EmployeeBenefitsDto>
{
    Task<TemplateApi<EmployeeBenefitsDto>> UpdateEmployeeBenefits(List<EmployeeBenefitsDto> listIdBenefits,
        Guid idUserCurrent, string fullName);
}