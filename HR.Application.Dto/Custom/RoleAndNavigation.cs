using HR.Domain.Core.Entities;

namespace HR.Application.Dto.Custom;

public class RoleAndNavigation
{
    public Role? Role { get; set; }
    public List<Navigation>? Navigation { get; set; }
}