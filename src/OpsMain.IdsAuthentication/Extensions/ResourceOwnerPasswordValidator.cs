using Duende.IdentityServer.Validation;
using IdentityModel;
using Microsoft.EntityFrameworkCore;
using OpsMain.Shared.Toolkit;
using OpsMain.StorageLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OpsMain.IdsAuthentication.Extensions
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        TreadstoneMainContext dbContext;

        public ResourceOwnerPasswordValidator(TreadstoneMainContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            try
            {
                var user = await dbContext.Users.FirstOrDefaultAsync(u => u.UserName == context.UserName);
                if (user != null)
                {
                    if (user.Password == context.Password.ToSHA256())
                    {
                        context.Result = new GrantValidationResult(subject: user.Id.ToString(), authenticationMethod: "custom", claims:Tools.SetUserClaims(user));
                        return;
                    }
                    context.Result = new GrantValidationResult(Duende.IdentityServer.Models.TokenRequestErrors.InvalidGrant, "密码错误");
                }
                context.Result = new GrantValidationResult(Duende.IdentityServer.Models.TokenRequestErrors.InvalidGrant, "用户不存在");

            }
            catch (Exception ex)
            {
                context.Result = new GrantValidationResult(Duende.IdentityServer.Models.TokenRequestErrors.InvalidGrant, $"出现异常：{ex.Message}");
            }
        }
    }
}
