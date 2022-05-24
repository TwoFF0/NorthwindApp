#pragma warning disable SA1600
#pragma warning disable SA1210

namespace NorthwindWebApps
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Northwind.Serivces.EntityFrameworkCore;
    using NorthwindWebApps.Infrastructure;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            switch (this.Configuration["ServiceType"])
            {
                case "In-Memory":
                    services.ConfigureInMemory();
                    break;

                case "AdoNet":
                    services.ConfigureAdoNet(this.Configuration);
                    break;

                case "EFCore":
                    services.ConfigureEFCore(this.Configuration);
                    break;
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, NorthwindContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApplication1 v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            if (this.Configuration.GetConnectionString("MyDB") == "In-Memory")
            {
                SeedData.GenerateSeedData(context, 15);
            }
        }
    }
}
