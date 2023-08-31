using HR.Application.Dto.Dto;
using HR.Application.Utilities.Utils;
using HR.Domain.Core.Entities;

namespace HR.Application.Interface.Interfaces;

public interface ITicketLaborEquipmentRepository : IRepository<TicketLaborEquipmentDto>
{
    Task<TicketLaborEquipment> GetDataById(Guid id);
    Task<TemplateApi<TicketLaborEquipmentDto>> GetTicketLaborEquipmentAndWorkFlow(Guid idUserCurrent, int pageNumber, int pageSize);
    Task<TemplateApi<TicketLaborEquipmentDto>> GetTicketLaborEquipmentAndWorkFlowByIdUserApproved(Guid idUserCurrent, int pageNumber, int pageSize);
}