using Duende.IdentityServer.Services;
using Duende.IdentityServer.Validation;
using IdentityServerHost.Quickstart.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpsMain.IdsAuthentication.Extensions;
using OpsMain.StorageLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace OpsMain.IdsAuthentication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSqlService(Configuration, "SysOpsConStr");

            services.AddCors(opt =>
            {
                opt.AddPolicy("allowAll", builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });

            var conStr = Configuration.GetConnectionString("SysIdsConStr");
            var migrationAsm = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            services.AddIdentityServer(opt =>
            {
                opt.Authentication.CookieSameSiteMode = Microsoft.AspNetCore.Http.SameSiteMode.Strict;
            })
                .AddConfigurationStore(opt =>
                {
                    opt.ConfigureDbContext = b => b.UseSqlServer(conStr, sql => sql.MigrationsAssembly(migrationAsm));
                })
                .AddOperationalStore(opt =>
                {
                    opt.ConfigureDbContext = b => b.UseSqlServer(conStr, sql => sql.MigrationsAssembly(migrationAsm));
                });

            //用户名密码模式，增加自定义claim
            services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
            //授权码模式，添加自定义claim
            services.AddTransient<IProfileService, ProfileService>();

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            IdsConfig.SeedData(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();
            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.Lax
            });
            app.UseRouting();
            app.UseCors("allowAll");
            app.UseIdentityServer();//使用ids
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
