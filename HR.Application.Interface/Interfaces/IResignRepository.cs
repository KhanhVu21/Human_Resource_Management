using HR.Application.Dto.Dto;
using HR.Application.Utilities.Utils;

namespace HR.Application.Interface.Interfaces;

public interface IResignRepository : IRepository<ResignDto>
{
    Task<ResignDto> GetDataById(Guid idResign);
    Task<TemplateApi<ResignDto>> GetResignAndWorkFlow(Guid idUserCurrent, int pageNumber, int pageSize);
    Task<TemplateApi<ResignDto>> GetResignAndWorkFlowByIdUserApproved(Guid idUserCurrent, int pageNumber, int pageSize);
}