using IdentityModel;
using OpsMain.StorageLayer.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace OpsMain.IdsAuthentication.Extensions
{
    public static class Tools
    {
        public static IEnumerable<Claim> SetUserClaims(SysUser user)
        {
            var menus = user.R_RoleUsers.SelectMany(o => o.Role.R_RoleMenus).Select(o => o.Menu.Href).Where(o=>!string.IsNullOrEmpty(o)).Distinct();
            return new Claim[]
            {
                new Claim("user_id", user.Id.ToString() ?? ""),
                new Claim(JwtClaimTypes.Name, user.UserName),
                new Claim("menus",string.Join(',',menus))

            };
        }

    }
}
