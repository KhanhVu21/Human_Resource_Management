using HR.Application.Dto.Dto;
using HR.Application.Utilities.Utils;
namespace HR.Application.Interface.Interfaces;

public interface IInternRequestRepository : IRepository<InternRequestDto>
{
    Task<TemplateApi<InternRequestDto>> GetInternRequestAndWorkFlow(Guid idUserCurrent, int pageNumber, int pageSize);
    Task<TemplateApi<InternRequestDto>> GetInternRequestAndWorkFlowByIdUserApproved(Guid idUserCurrent, int pageNumber, int pageSize);
}