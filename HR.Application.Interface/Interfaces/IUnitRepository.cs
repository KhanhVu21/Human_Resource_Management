using HR.Application.Dto.Dto;
using HR.Application.Utilities.Utils;

namespace HR.Application.Interface.Interfaces
{
    public interface IUnitRepository : IRepository<UnitDto>
    {
        #region ===[ CRUD TABLE UNIT ]==================================================
        Task<TemplateApi<UnitDto>> GetAllUnitByIdParent(Guid idUnitParent, int pageNumber, int pageSize);
        Task<UnitDto> GetUnitByUnitCode(string unitCode);

        #endregion
    }
}