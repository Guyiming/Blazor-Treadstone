using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using OpsMain.StorageLayer;
using OpsMain.StorageLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpsMain.IdsAuthentication.Extensions
{
    public class ProfileService : IProfileService
    {
        TreadstoneMainContext dbContext;
        IMemoryCache _cache;
        public ProfileService(TreadstoneMainContext dbContext,IMemoryCache cache)
        {
            this.dbContext = dbContext;
            _cache = cache;
        }

        //Get user profile date in terms of claims when calling /connect/userinfo
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            //sub里是用户id
            var userId = context.Subject.Claims.FirstOrDefault(x => x.Type == "sub");
            SysUser cachedUser = null;
            if (!string.IsNullOrEmpty(userId?.Value) && long.Parse(userId.Value) > 0)
            {

                long.TryParse(userId.Value, out long uid);
                var cacheKey = $"{CacheKeys.AuthorizedMenus}_{userId}";
                if(!_cache.TryGetValue(cacheKey, out cachedUser))
                {
                    //从数据库获取
                    cachedUser = await dbContext.Users.Where(u => u.Id == uid).Include(u => u.R_RoleUsers).ThenInclude(o => o.Role).ThenInclude(r => r.R_RoleMenus).ThenInclude(rm => rm.Menu).FirstOrDefaultAsync();
                    var option = new MemoryCacheEntryOptions();
                    option.SetSlidingExpiration(TimeSpan.FromSeconds(10));//10s滑动过期
                    _cache.Set(cacheKey, cachedUser,option);
                }
               

                if (cachedUser != null)
                {
                    var claims = Tools.SetUserClaims(cachedUser);

                    context.IssuedClaims = claims.Where(x => context.RequestedClaimTypes.Contains(x.Type)).ToList();
                }
            }
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            ////get subject from context (set in ResourceOwnerPasswordValidator.ValidateAsync),
            //var userId = context.Subject.Claims.FirstOrDefault(x => x.Type == "user_id");

            //if (!string.IsNullOrEmpty(userId?.Value) && long.Parse(userId.Value) > 0)
            //{
            //    var user = await _userRepository.FindAsync(long.Parse(userId.Value));

            //    if (user != null)
            //    {
            //        if (user.IsActive)
            //        {
            //            context.IsActive = user.IsActive;
            //        }
            //    }
            //}
            return Task.CompletedTask;
        }
    }
}
