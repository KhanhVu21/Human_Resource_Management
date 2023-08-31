using HR.Application.Dto.Dto;
using HR.Application.Utilities.Utils;

namespace HR.Application.Interface.Interfaces;

public interface IOvertimeRepository : IRepository<OvertimeDto>
{
    Task<TemplateApi<OvertimeDto>> GetOvertimeAndWorkFlow(Guid idUserCurrent, int pageNumber, int pageSize);
    Task<TemplateApi<OvertimeDto>> GetOvertimeDAndWorkFlowByIdUserApproved(Guid idUserCurrent, int pageNumber, int pageSize);
}