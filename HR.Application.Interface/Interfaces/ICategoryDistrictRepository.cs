using HR.Application.Dto.Dto;
using HR.Application.Utilities.Utils;

namespace HR.Application.Interface.Interfaces;

public interface ICategoryDistrictRepository : IRepository<CategoryDistrictDto>
{
    Task<TemplateApi<CategoryDistrictDto>> GetByCityCode(string cityCode, int pageNumber, int pageSize);
}