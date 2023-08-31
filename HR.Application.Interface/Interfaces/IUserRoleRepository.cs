using HR.Application.Dto.Dto;
using HR.Application.Utilities.Utils;

namespace HR.Application.Interface.Interfaces;

public interface IUserRoleRepository
{
    #region CURD TABLE USER_ROLE
    Task<TemplateApi<UserRoleDto>> InsertListUserRole(List<Guid> idRole, Guid idUser, Guid idUserCurrent, string fullName);

    #endregion
}