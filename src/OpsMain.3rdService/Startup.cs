using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using OpsMain._3rdService.Mapper;
using OpsMain.StorageLayer;
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
    }
}
