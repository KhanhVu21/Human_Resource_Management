using HR.Application.Dto.Dto;
using HR.Application.Utilities.Utils;

namespace HR.Application.Interface.Interfaces;

public interface IUserTypeRepository : IRepository<UserTypeDto>
{
    #region CRUD TABLE USER_TYPE
    Task<UserTypeDto> GetTypeUser(string typeCode);
    Task<UserTypeDto> GetUserType(Guid id);
    #endregion
}