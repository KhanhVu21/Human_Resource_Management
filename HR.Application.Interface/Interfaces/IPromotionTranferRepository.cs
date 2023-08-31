using HR.Application.Dto.Dto;
using HR.Application.Utilities.Utils;

namespace HR.Application.Interface.Interfaces;

public interface IPromotionTranferRepository : IRepository<PromotionTranferDto>
{
    Task<TemplateApi<PromotionTranferDto>> GetPromotionTranferAndWorkFlow(Guid idUserCurrent, int pageNumber, int pageSize);
    Task<TemplateApi<PromotionTranferDto>> GetPromotionTranferAndWorkFlowByIdUserApproved(Guid idUserCurrent, int pageNumber, int pageSize);
}