using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using StudentApi.Data;
using System.Security.Claims;

namespace StudentApi.Authorization
{

    public class PermissionBasedAuthorizationFilter(AppDbcontext appDbcontext) : IAuthorizationFilter
    {


        public void OnAuthorization(AuthorizationFilterContext context)
        {
            CheckPermissionAttribute? attribute = (CheckPermissionAttribute?)context.ActionDescriptor.EndpointMetadata
                .FirstOrDefault(x => x is CheckPermissionAttribute);

            if (attribute != null)
            {

                var claimIdentity = context.HttpContext.User.Identity as ClaimsIdentity;

                if (claimIdentity == null || !claimIdentity.IsAuthenticated)
                {
                    context.Result = new ForbidResult();
                }
                else
                {
                    var userId = int.Parse(claimIdentity.FindFirst(ClaimTypes.NameIdentifier)!.Value);

                    var hasPermissions = appDbcontext.UserPermissions.Any(x => x.UserId == userId &&
                    x.PermissionId == attribute.Permission);


                    if (!hasPermissions)
                    {
                        context.Result = new ForbidResult();
                    }
                }
            }
        }
    }
}
