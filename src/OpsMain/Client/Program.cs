using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpsMain.Client.Extensions;
using OpsMain.Client.Filters;
using OpsMain.Client.RestServices;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OpsMain.Client
{
    //授权参考文档：https://docs.microsoft.com/zh-cn/aspnet/core/security/authorization/views?view=aspnetcore-5.0
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddSingleton<IAuthorizationHandler, MenuAuthorizeHandler>();
            builder.Services.AddScoped<CustomAuthorizationMessageHandler>();
            builder.Services.AddScoped<RestRoleService>();
            builder.Services.AddScoped<RestUserService>();
            builder.Services.AddScoped<RestMenuService>();
            builder.Services.AddScoped<RestFreeService>();


            builder.Services.AddHttpClient("webapi", client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["3rdResource"]);
            }).AddHttpMessageHandler<CustomAuthorizationMessageHandler>();


            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("webapi"));


            builder.Services.AddMasaBlazor();

            builder.Services.AddOidcAuthentication(opt =>
            {
                //从wwwroot/appsettings.json读取
                builder.Configuration.Bind("LocalOidc", opt.ProviderOptions);

            });

            //设置策略
            builder.Services.AddAuthorizationCore(opt =>
            {
                opt.AddPolicy(PolicyString.MenuAuthorize, builder =>
                {
                    builder.RequireClaim("menus");
                    builder.Requirements.Add(new MenuRequirement());
                });
            });

            //builder.Services.AddApiAuthorization().AddAccountClaimsPrincipalFactory<CustomUserFactory>();



            await builder.Build().RunAsync();
        }
    }
}
