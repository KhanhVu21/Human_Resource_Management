using HR.Application.Dto.Dto;
using HR.Application.Utilities.Utils;

namespace HR.Application.Interface.Interfaces;

public interface ICategoryWardRepository : IRepository<CategoryWardDto>
{
    Task<TemplateApi<CategoryWardDto>> GetCategoryWardByDistrictCode(string districtCode, int pageNumber, int pageSize);
}