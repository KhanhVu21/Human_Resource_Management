using HR.Application.Dto.Dto;
using HR.Application.Utilities.Utils;

namespace HR.Application.Interface.Interfaces;

public interface IBusinessTripRepository : IRepository<BusinessTripDto>
{
    Task<TemplateApi<BusinessTripDto>> GetBusinessTripAndWorkFlow(Guid idUserCurrent, int pageNumber, int pageSize);
    Task<TemplateApi<BusinessTripDto>> GetBusinessTripAndWorkFlowByIdUserApproved(Guid idUserCurrent, int pageNumber, int pageSize);
}