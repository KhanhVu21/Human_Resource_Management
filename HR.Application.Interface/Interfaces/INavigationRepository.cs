using HR.Application.Dto.Custom;
using HR.Application.Dto.Dto;
using HR.Application.Utilities.Utils;

namespace HR.Application.Interface.Interfaces;

public interface INavigationRepository : IRepository<NavigationDto>
{
    Task<TemplateApi<NavigationAndChild>> GetNavigationByIdUser(Guid idUser, int pageNumber, int pageSize);
}