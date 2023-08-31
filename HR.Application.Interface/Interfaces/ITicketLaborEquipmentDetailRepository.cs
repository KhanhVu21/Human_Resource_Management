using HR.Application.Dto.Dto;
using HR.Application.Utilities.Utils;

namespace HR.Application.Interface.Interfaces;

public interface ITicketLaborEquipmentDetailRepository : IRepository<TicketLaborEquipmentDetailDto>
{
    Task<TemplateApi<TicketLaborEquipmentDetailDto>> UpdateTicketLaborEquipmentDetail(List<TicketLaborEquipmentDetailDto> ticketLaborEquipmentDetailDto,
        Guid idUserCurrent, string fullName);
}