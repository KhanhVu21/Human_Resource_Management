using HR.Application.Dto.Dto;
using HR.Domain.Core.Entities;

namespace HR.Application.Dto.Custom;

public class UserAndRole
{
    public UserDto? User { get; set; }
    public List<Role>? Roles { get; set; }
}