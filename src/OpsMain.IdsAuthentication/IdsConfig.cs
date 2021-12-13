using Duende.IdentityServer;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;
using IdentityModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;

namespace IdentityServerHost.Quickstart.UI
{
    public class IdsConfig
    {
        //1. 配置可以访问的user（授权码模式和资源拥有者凭证模式会用到）
        public static List<TestUser> Users
        {
            get
            {
                var address = new
                {
                    street_address = "One Hacker Way",
                    locality = "Heidelberg",
                    postal_code = 69118,
                    country = "Germany"
                };

                return new List<TestUser>
                {
                    new TestUser
                    {
                        SubjectId = "818727",
                        Username = "alice",
                        Password = "alice",
                        Claims =
                        {
                            new Claim(JwtClaimTypes.Name, "Alice Smith"),
                            new Claim(JwtClaimTypes.GivenName, "Alice"),
                            new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            new Claim(JwtClaimTypes.Email, "AliceSmith@email.com"),
                            new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                            new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                            new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(address), IdentityServerConstants.ClaimValueTypes.Json),
                            new Claim("test","testvalue")
                        }
                    },
                    new TestUser
                    {
                        SubjectId = "88421113",
                        Username = "bob",
                        Password = "bob",
                        Claims =
                        {
                            new Claim(JwtClaimTypes.Name, "Bob Smith"),
                            new Claim(JwtClaimTypes.GivenName, "Bob"),
                            new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            new Claim(JwtClaimTypes.Email, "BobSmith@email.com"),
                            new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                            new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                            new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(address), IdentityServerConstants.ClaimValueTypes.Json)
                        }
                    }
                };
            }
        }

        //2. 配置要保护的身份资源
        public static List<IdentityResource> IdentityResources
        {
            get
            {
                IdentityResources.OpenId oid = new IdentityResources.OpenId();
                oid.UserClaims.Add("menus");
                return new List<IdentityResource>()
                {
                    oid,//必须有
                    new IdentityResources.Profile(),
                };
            }
        }


        //3.配置要保护的API资源
        public static List<ApiScope> ApiScopes => new List<ApiScope>
        {
            new ApiScope
            {
                Name="Authenticated",
                DisplayName="认证通过即可访问受保护的资源",
                UserClaims =
                {
                    JwtClaimTypes.Name,
                    "menus"
                },

            }
        };

        //4. 配置请求端
        public static List<Client> Clients => new List<Client>
        {
            new Client
            {
                ClientId="blazor",
                ClientSecrets =
                {
                    new Secret("1234".Sha256())
                },
                AllowedGrantTypes=GrantTypes.ResourceOwnerPassword,//客户端凭证模式
                AllowedScopes={ "Authenticated" }
            },
            new Client
            {
                ClientId="blazor_code2",
                AllowedGrantTypes=GrantTypes.Code,//授权码模式
                AllowedScopes={
                    "Authenticated",
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                },
                RedirectUris=
                {
                    "http://localhost:5000/authentication/login-callback",
                    "http://tools.guyiming.shop/authentication/login-callback"
                },
                PostLogoutRedirectUris={
                    "http://localhost:5000/authentication/logout-callback",
                    "http://tools.guyiming.shop/authentication/logout-callback"
                },
                RequireClientSecret=false,//不需要设置secret
                AllowedCorsOrigins={ "http://localhost:5000"}
            },
            new Client
            {
                ClientId="MVC_Client",
                ClientName="MVC_Client ClientName",
                ClientSecrets =
                {
                    new Secret("1234".Sha256())
                },
                //AllowOfflineAccess=true,//启用refresh token
                AllowedGrantTypes=GrantTypes.Code,//授权码模式
                RedirectUris={"https://localhost:4001/signin-oidc"},//登录完之后跳转
                PostLogoutRedirectUris={ "https://localhost:4001/signout-callback-oidc" },//退出后跳转

                AllowedScopes=
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "Authenticated"
                },
                RequireConsent=true,//是否需要用户同意
                AllowedCorsOrigins={ "https://localhost:4001"}
            },
        };


        public static void SeedData(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                #region 数据库迁移
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();
                #endregion

                //设置Clients
                context.Clients.RemoveRange(context.Clients.ToArray());//清空所有
                IdsConfig.Clients.ForEach(c => context.Clients.Add(c.ToEntity()));
                context.SaveChanges();

                //设置IdentityResource
                context.IdentityResources.RemoveRange(context.IdentityResources.ToArray());
                IdsConfig.IdentityResources.ForEach(r => context.IdentityResources.Add(r.ToEntity()));
                context.SaveChanges();

                //设置ApiScope

                context.ApiScopes.RemoveRange(context.ApiScopes.ToArray());
                IdsConfig.ApiScopes.ForEach(a => context.ApiScopes.Add(a.ToEntity()));
                context.SaveChanges();

            }
        }
    }
}