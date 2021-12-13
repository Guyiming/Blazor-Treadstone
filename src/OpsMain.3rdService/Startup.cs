using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using OpsMain._3rdService.Mapper;
using OpsMain.Shared.Toolkit;
using OpsMain.StorageLayer;
using OpsMain.StorageLayer.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpsMain._3rdService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public object SYsUser { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSqlService(Configuration, "SysOpsConStr");
            services.AddMyDI();

            services.AddCors(opt =>
            {
                opt.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });

            services
                .AddControllers(opt =>
                {
                    opt.AllowEmptyInputInBodyModelBinding = true;
                })
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull | JsonIgnoreCondition.WhenWritingDefault;
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "OpsMain._3rdService", Version = "v1" });

                //设置接口说明文件的存位置（可以让接口描述更丰富）
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddAutoMapper(config => { config.AllowNullCollections = true; }, typeof(MapProfile));


            services.AddAuthentication("Bearer")
               .AddJwtBearer("Bearer", opt =>
               {
                   opt.Authority = Configuration["Authority"];
                   opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                   {
                       ValidateAudience = false
                   };
                   opt.RequireHttpsMetadata = false;
               });

            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("Policy1", builder =>
                {
                    builder.RequireAuthenticatedUser();//首先需要通过身份验证
                    //builder.RequireClaim("scope", "Authenticated");
                });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OpsMain._3rdService v1"));
                SeedData(app);
            }

            app.UseRouting();
            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                var builder = endpoints.MapControllers();
                //if(!env.IsDevelopment())
                //{
                builder.RequireAuthorization("Policy1");
                //}
            });
        }

        private void SeedData(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<TreadstoneMainContext>();

                if (!context.Users.Any())
                {
                    SysUser admin = new SysUser { UserName = "admin", Password = "admin".ToSHA256() };
                    context.Users.Add(admin);
                    context.SaveChanges();

                    SysRole administrators = new SysRole { RoleName = "Administrators" };
                    context.Roles.Add(administrators);
                    context.SaveChanges();

                    List<SysMenu> menus = new List<SysMenu>();
                    
                    menus.Add(new SysMenu { MenuName = "Nginx Config", Href = "/", Icon = "icon-nginx1", OrderNo = 0, ParentId = null });
                    menus.Add(new SysMenu { MenuName = "Linux Service", Href = "systemctl", Icon = "icon-linux", OrderNo = 1, ParentId = null });
                    menus.Add(new SysMenu { MenuName = "C# Formatter", Href = "codeformat", Icon = "icon-c-sharp-l", OrderNo = 2, ParentId = null });
                    var parentMenu = new SysMenu { MenuName = "System Management", Href = null, Icon = "icon-guanli", OrderNo = 4, ParentId = null };
                    menus.Add(parentMenu);
                    context.Menus.AddRange(menus);
                    context.SaveChanges();

                    List<SysMenu> subMenus = new List<SysMenu>();
                    subMenus.Add(new SysMenu { MenuName = "User Management", Href = "management/user", Icon = "user", OrderNo = 0, ParentId = parentMenu.Id });
                    subMenus.Add(new SysMenu { MenuName = "Role Management", Href = "management/role", Icon = "team", OrderNo = 1, ParentId = parentMenu.Id });
                    subMenus.Add(new SysMenu { MenuName = "Menu & Permission", Href = "management/menu", Icon = "idcard", OrderNo = 2, ParentId = parentMenu.Id });
                    
                    context.Menus.AddRange(subMenus);
                    context.SaveChanges();

                    context.R_RoleUsers.Add(new R_RoleUser { UserId = admin.Id, RoleId = administrators.Id });
                    menus.AddRange(subMenus);
                    menus.ForEach(m =>
                    {
                        context.R_RoleMenus.Add(new R_RoleMenu { RoleId = administrators.Id, MenuId = m.Id });
                    });

                    context.SaveChanges();

                }
            }
        }
    }
}
