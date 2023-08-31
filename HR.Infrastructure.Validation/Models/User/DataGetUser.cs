using HR.Application.Dto.Dto;
using HR.Domain.Core.Entities;

namespace HR.Infrastructure.Validation.Models.User;

public class DataGetUser
{
    public UserDto? Data { get; set; }
    public IEnumerable<InformationRoleOfUser>? RoleList { get; set; }
    public bool IsAdmin { get; set; }
}