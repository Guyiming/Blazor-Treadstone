using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace OpsMain.Gateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            if(builder.Environment.IsDevelopment())
            {
                builder.Configuration.AddJsonFile("ocelot.Development.json", optional: false, reloadOnChange: true);
            }
            else
            {
                builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
            }
            
            builder.Services.AddOcelot();
            builder.Services.AddControllers();
            builder.Services.AddCors(opt =>
            {
                opt.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseAuthorization();
            app.UseOcelot();
            app.UseCors();

            app.MapControllers();

            app.Run();
        }
    }
}
