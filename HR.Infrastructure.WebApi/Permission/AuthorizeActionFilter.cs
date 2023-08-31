using HR.Application.Dto.Dto;
using HR.Application.Interface.Interfaces;
using HR.Application.Utilities.Utils;
using HR.Domain.Kernel.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HR.Infrastructure.WebApi.Permission
{
    public class AuthorizeActionFilter : IAuthorizationFilter
    {
        private readonly string _permission;
        private readonly IRoleRepository _roleRepository;

        public AuthorizeActionFilter(string permission, IRoleRepository roleRepository)
        {
            _permission = permission;
            _roleRepository = roleRepository;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var idUserCurrent = context.HttpContext.Items["UserId"] as Guid? ?? Guid.Empty;

            var rolesOfUser = _roleRepository.GetRoleByIdUser(idUserCurrent);

            if (!rolesOfUser.Result.Any())
            {
                context.Result =
                    new JsonResult(new TemplateApi<UserDto>(null, null, "Bạn cần đăng nhập tài khoản !",
                            false, 0, 0, 0, 0))
                    { StatusCode = StatusCodes.Status401Unauthorized };
            }

            switch (_permission)
            {
                case ListRole.Admin:
                    {
                        if (!rolesOfUser.Result.Any(e => e.IsAdmin))
                        {
                            context.Result =
                                new JsonResult(new TemplateApi<UserDto>(null, null, "Bạn cần đăng nhập tài khoản Admin !",
                                        false, 0, 0, 0, 0))
                                { StatusCode = StatusCodes.Status401Unauthorized };
                        }

                        break;
                    }
                case ListRole.User:
                    {
                        if (!rolesOfUser.Result.Any())
                        {
                            context.Result =
                                new JsonResult(new TemplateApi<UserDto>(null, null,
                                        "Bạn cần đăng nhập tài khoản của người dùng hoặc Admin !",
                                        false, 0, 0, 0, 0))
                                { StatusCode = StatusCodes.Status401Unauthorized };
                        }

                        break;
                    }
                    // case ListRole.HumanResource:
                    // {
                    //     if (!rolesOfUser.Result.Any(e => e.NumberRole == 1 || e.IsAdmin))
                    //     {
                    //         context.Result =
                    //             new JsonResult(new TemplateApi<UserDto>(null, null,
                    //                     "Bạn cần đăng nhập tài khoản của phòng nhân sự hoặc Admin!",
                    //                     false, 0, 0, 0, 0))
                    //                 { StatusCode = StatusCodes.Status401Unauthorized };
                    //     }
                    //
                    //     break;
                    // }
                    // case ListRole.Director:
                    // {
                    //     if (!rolesOfUser.Result.Any(e => e.NumberRole == 2 || e.IsAdmin))
                    //     {
                    //         context.Result =
                    //             new JsonResult(new TemplateApi<UserDto>(null, null,
                    //                     "Bạn cần đăng nhập tài khoản của ban giám đốc hoặc Admin!",
                    //                     false, 0, 0, 0, 0))
                    //                 { StatusCode = StatusCodes.Status401Unauthorized };
                    //     }
                    //
                    //     break;
                    // }
                    // case ListRole.Unit + ListRole.HumanResource:
                    // {
                    //     if (!rolesOfUser.Result.Any(e => e.NumberRole == 0 || e.NumberRole == 1 || e.IsAdmin))
                    //     {
                    //         context.Result =
                    //             new JsonResult(new TemplateApi<UserDto>(null, null,
                    //                     "Bạn cần đăng nhập tài khoản của phòng ban và nhân sự!",
                    //                     false, 0, 0, 0, 0))
                    //                 { StatusCode = StatusCodes.Status401Unauthorized };
                    //     }
                    //
                    //     break;
                    // }
                    // case ListRole.Unit + ListRole.Director:
                    // {
                    //     if (!rolesOfUser.Result.Any(e => e.NumberRole == 0 || e.NumberRole == 2 || e.IsAdmin))
                    //     {
                    //         context.Result =
                    //             new JsonResult(new TemplateApi<UserDto>(null, null,
                    //                     "Bạn cần đăng nhập tài khoản của phòng ban và giám đốc !",
                    //                     false, 0, 0, 0, 0))
                    //                 { StatusCode = StatusCodes.Status401Unauthorized };
                    //     }
                    //
                    //     break;
                    // }
                    // case ListRole.HumanResource + ListRole.Director:
                    // {
                    //     if (!rolesOfUser.Result.Any(e => e.NumberRole == 1 || e.NumberRole == 2 || e.IsAdmin))
                    //     {
                    //         context.Result =
                    //             new JsonResult(new TemplateApi<UserDto>(null, null,
                    //                     "Bạn cần đăng nhập tài khoản của nhân sự và giám đốc !",
                    //                     false, 0, 0, 0, 0))
                    //                 { StatusCode = StatusCodes.Status401Unauthorized };
                    //     }
                    //
                    //     break;
                    // }
                    // case ListRole.Unit + ListRole.HumanResource + ListRole.Director:
                    // {
                    //     if (!rolesOfUser.Result.Any(e =>
                    //             e.NumberRole == 0 || e.NumberRole == 1 || e.NumberRole == 2 || e.IsAdmin))
                    //     {
                    //         context.Result =
                    //             new JsonResult(new TemplateApi<UserDto>(null, null,
                    //                     "Bạn cần đăng nhập tài khoản của phòng ban và nhân sự và giám đốc!",
                    //                     false, 0, 0, 0, 0))
                    //                 { StatusCode = StatusCodes.Status401Unauthorized };
                    //     }
                    //
                    //     break;
                    // }
            }
        }
    }
}