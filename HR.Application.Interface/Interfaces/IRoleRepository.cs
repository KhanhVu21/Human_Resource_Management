using HR.Application.Dto.Custom;
using HR.Application.Dto.Dto;
using HR.Application.Utilities.Utils;
using HR.Domain.Core.Entities;

namespace HR.Application.Interface.Interfaces;

public interface IRoleRepository : IRepository<RoleDto>
{
    #region ===[ CRUD TABLE ROLE ]=============================================================

    Task<IEnumerable<InformationRoleOfUser>> GetRoleByIdUser(Guid idUser);
    Task<RoleDto> GetUserRole(string roleType);
    Task<RoleDto> GetUserRoleById(Guid idRole);
    Task<List<RoleDto>> GetAllRole();
    Task<TemplateApi<RoleDto>> InsertRoleAndNavigationRole(RoleDto roleDto, List<Guid> idNavigationRole, Guid idUserCurrent,
        string fullName);
    Task<TemplateApi<RoleDto>> UpdateRoleAndNavigationRole(RoleDto roleDto, List<Guid> idNavigationRole, Guid idUserCurrent,
        string fullName);
    Task<TemplateApi<RoleDto>> DeleteRoleAndNavigationRole(List<Guid> roleIds, Guid idUserCurrent,
        string fullName);
    Task<TemplateApi<RoleAndNavigation>> GetRoleAndNavigation(Guid roleId);

    #endregion
}