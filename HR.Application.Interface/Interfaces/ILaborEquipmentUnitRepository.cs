using HR.Application.Dto.Dto;
using HR.Application.Utilities.Utils;

namespace HR.Application.Interface.Interfaces;

public interface ILaborEquipmentUnitRepository : IRepository<LaborEquipmentUnitDto>
{
    Task<TemplateApi<LaborEquipmentUnitDto>> GetLaborEquipmentUnitByUnit(Guid idUnit, int pageNumber, int pageSize);

    Task<TemplateApi<LaborEquipmentUnitDto>> GetLaborEquipmentUnitByIdTicketLaborEquipment(Guid idTicketLaborEquipment,
        int pageNumber, int pageSize);

    Task<TemplateApi<LaborEquipmentUnitDto>> InsertLaborEquipmentUnitTypeInsert(Guid idTicketLaborEquipment,
        Guid idUserCurrent, string fullName);

    Task<TemplateApi<LaborEquipmentUnitDto>> UpdateStatusLaborEquipmentUnit(Guid idLaborEquipmentUnit, int status,
        Guid idUserCurrent, string fullName);

    Task<TemplateApi<LaborEquipmentUnitDto>> GetLaborEquipmentUnitByUnitAndEmployee(Guid idUnit, Guid idEmployee,
        int pageNumber, int pageSize);

    Task<TemplateApi<LaborEquipmentUnitDto>> GetLaborEquipmentUnitByStatus(int status, int pageNumber, int pageSize);

    Task<TemplateApi<LaborEquipmentUnitDto>> UpdateLaborEquipmentUnitByListIdAndStatus(Guid id, int status,
        Guid idUserCurrent, string fullName);
}