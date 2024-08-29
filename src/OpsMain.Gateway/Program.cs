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
            builder.Services.AddOcelot();
            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseAuthorization();
            app.UseOcelot();

            app.MapControllers();

            app.Run();
        }
    }
}
