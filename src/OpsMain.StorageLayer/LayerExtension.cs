using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpsMain.StorageLayer.Entity;
using OpsMain.StorageLayer.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpsMain.StorageLayer
{
    public static class LayerExtension
    {
        public static void AddSqlService(this IServiceCollection services, IConfiguration configuation, string sectionName)
        {
            string constr = configuation.GetValue<string>("ASPNETCORE_MY_MAIN_CONSTR");
            if (string.IsNullOrEmpty(constr))
            {
                constr = configuation.GetConnectionString(sectionName);
            }

            services.AddDbContext<TreadstoneMainContext>(opt =>
            {
                opt.UseMySql(constr, ServerVersion.AutoDetect(constr));
                //opt.UseSqlServer(constr, o => o.EnableRetryOnFailure());
            });
        }

        /// <summary>
        /// 依赖服务的汇总
        /// </summary>
        /// <param name="services"></param>
        public static void AddMyDI(this IServiceCollection services)
        {
            services.AddScoped<CommonRepoService<SysUser>>();
            services.AddScoped<CommonRepoService<SysRole>>();
            services.AddScoped<CommonRepoService<SysMenu>>();
            services.AddScoped<CommonRepoService<R_RoleMenu>>();
            services.AddScoped<CommonRepoService<R_RoleUser>>();
        }
    }
}
