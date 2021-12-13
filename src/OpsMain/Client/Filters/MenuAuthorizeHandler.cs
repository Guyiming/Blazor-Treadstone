using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using OpsMain.Client.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OpsMain.Client.Filters
{
    public class MenuRequirement : IAuthorizationRequirement
    {

    }
    public class MenuAuthorizeHandler : AuthorizationHandler<MenuRequirement>
    {
        NavigationManager _navi;

        public MenuAuthorizeHandler(NavigationManager navi)
        {
            _navi = navi;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MenuRequirement requirement)
        {
            var urls = context.User.Claims.FirstOrDefault(o => o.Type == "menus")?.Value?.Split(',');
            var u = new Uri(_navi.Uri);
            var curUrl = u.AbsolutePath.Substring(1);
            if (urls?.Contains(curUrl) != true)
            {
                context.Fail();
            }
            else
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

}
