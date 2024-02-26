using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

namespace BackendChallengeTechFullStackN5
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Obtener el valor de FrontendURL de la configuración
            string frontendUrl = Configuration.GetValue<string>("Environment:FrontendURL");

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder =>
                    {
                        builder
                            .WithOrigins(frontendUrl) // Permitir solo el origen específico de tu frontend
                            .AllowAnyMethod() // Permitir cualquier método
                            .AllowAnyHeader(); // Permitir cualquier encabezado
                    });
            });

            // Configura la conexión a la base de datos
            services.AddDbContext<Data.ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DbConnection")));

            // Agrega servicios MVC
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Habilita CORS
            app.UseCors("AllowSpecificOrigin");

            // Middleware de enrutamiento
            app.UseRouting();

            // Middleware de autorización
            app.UseAuthorization();

            // Middleware para servir archivos estáticos desde wwwroot
            app.UseStaticFiles();

            // Define las rutas de los endpoints
            app.UseEndpoints(endpoints =>
            {
                // Mapea las rutas a los controladores
                endpoints.MapControllers();
            });
        }
    }
}
