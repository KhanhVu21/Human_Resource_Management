using HR.Application.Dto.Dto;
using HR.Application.Utilities.Utils;

namespace HR.Application.Interface.Interfaces;

public interface ICategoryVacanciesRepository : IRepository<CategoryVacanciesDto>
{
    Task<TemplateApi<CategoryVacanciesDto>> UpdateStatusVacancy(int status, Guid idCategoryVacancy, Guid idUserCurrent,
        string fullName);
    Task<TemplateApi<CategoryVacanciesDto>> GetVacancyApproved(int pageNumber, int pageSize);
}