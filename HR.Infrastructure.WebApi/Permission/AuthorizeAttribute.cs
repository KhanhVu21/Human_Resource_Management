using Microsoft.AspNetCore.Mvc;

namespace HR.Infrastructure.WebApi.Permission
{
    public class AuthorizeAttribute : TypeFilterAttribute
    {
        public AuthorizeAttribute(string permission)
        : base(typeof(AuthorizeActionFilter))
        {
            Arguments = new object[] { permission };
        }
    }
}
