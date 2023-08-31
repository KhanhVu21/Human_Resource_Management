using HR.Application.Dto.Dto;
using HR.Application.Utilities.Utils;
using HR.Domain.Core.Entities;

namespace HR.Application.Interface.Interfaces;

public interface IRequestToHiredRepository : IRepository<RequestToHiredDto>
{
    Task<RequestToHired> GetDataById(Guid idRequestToHire);
    Task<TemplateApi<RequestToHiredDto>> GetRequestToHireAndWorkFlow(Guid idUserCurrent, int pageNumber, int pageSize);
    Task<TemplateApi<RequestToHiredDto>> GetRequestToHireAndWorkFlowByIdUserApproved(Guid idUserCurrent, int pageNumber, int pageSize);
}