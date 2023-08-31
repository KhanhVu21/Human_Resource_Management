using HR.Application.Dto.Dto;
using HR.Application.Utilities.Utils;

namespace HR.Application.Interface.Interfaces;

public interface IBusinessTripEmployeeRepository : IRepository<BusinessTripEmployeeDto>
{
    Task<TemplateApi<BusinessTripEmployeeDto>> InsertBusinessTripEmployeeByList(
        List<BusinessTripEmployeeDto> businessTripEmployee, Guid idUserCurrent, string fullName);

    Task<TemplateApi<BusinessTripEmployeeDto>> GetListByIBusinessTrip(
        Guid idBusinessTrip, int pageNumber,
        int pageSize);
}