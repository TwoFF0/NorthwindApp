namespace NorthwindWebApps.Infrastructure
{
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.OpenApi.Models;
    using Northwind.DataAccess;
    using Northwind.DataAccess.SqlServer;
    using Northwind.Serivces.EntityFrameworkCore;
    using Northwind.Serivces.EntityFrameworkCore.Customers;
    using Northwind.Serivces.EntityFrameworkCore.Employee;
    using Northwind.Serivces.EntityFrameworkCore.Products;
    using Northwind.Services;
    using Northwind.Services.Blogging;
    using Northwind.Services.Blogging.Services;
    using Northwind.Services.Customers;
    using Northwind.Services.DataAccess.Employee;
    using Northwind.Services.DataAccess.Products;
    using Northwind.Services.Employees;
    using Northwind.Services.EntityFrameworkCore.Blogging;
    using Northwind.Services.EntityFrameworkCore.Blogging.Context;
    using Northwind.Services.EntityFrameworkCore.Blogging.Services;
    using Northwind.Services.Products;

    /// <summary>
    /// Class to configure services.
    /// </summary>
    public static class ServiceConfigurations
    {
        /// <summary>
        /// Configure services in mode "EntityFramework.InMemory".
        /// </summary>
        /// <param name="services">Services to configure.</param>
        public static void ConfigureInMemory(this IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<NorthwindContext>(options => options.UseInMemoryDatabase("Northwind"))
            .AddTransient<IProductManagementService, ProductManagementService>()
            .AddTransient<IProductCategoryManagementService, ProductCategoryManagementService>()
            .AddTransient<IProductCategoryPictureService, ProductCategoryPictureService>()
            .AddTransient<IEmployeePictureService, EmployeePictureService>()
            .AddTransient<IEmployeeManagementService, EmployeeManagementService>()
            .AddAutoMapper(typeof(MappingProfile))

            .AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "NorthWindInMemory", Version = "v1" });
            });
        }

        /// <summary>
        /// Configure services in mode "Ado.Net (SqlClient)".
        /// </summary>
        /// <param name="services">Services to configure.</param>
        public static void ConfigureAdoNet(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddTransient<NorthwindDataAccessFactory, SqlServerDataAccessFactory>()
            .AddScoped(_ => new SqlConnection(configuration.GetConnectionString("SqlConnection")))
            .AddTransient<IProductManagementService, ProductManagementDataAccessService>()
            .AddTransient<IProductCategoryManagementService, ProductCategoriesManagementDataAccessService>()
            .AddTransient<IProductCategoryPictureService, ProductCategoryPictureService>()
            .AddTransient<IEmployeeManagementService, EmployeeManagementDataAccessService>()
            .AddTransient<IEmployeePictureService, EmployeePictureManagementDataAccessService>()
            .AddAutoMapper(typeof(MappingProfile))

            .AddDbContext<NorthwindContext>(options => options.UseInMemoryDatabase("Northwind"))
            .AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "NorthWindAdoNet", Version = "v1" });
            });
        }

        /// <summary>
        /// Configure services in mode "EFCore".
        /// </summary>
        /// <param name="services">Services to configure.</param>
        public static void ConfigureEFCore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddTransient<IDesignTimeDbContextFactory<BloggingContext>, DesignTimeBloggingContextFactory>()
            .AddTransient<NorthwindDataAccessFactory, SqlServerDataAccessFactory>()
            .AddScoped(_ => new SqlConnection(configuration.GetConnectionString("SqlConnection")))
            .AddTransient<IProductManagementService, ProductManagementDataAccessService>()
            .AddTransient<IProductCategoryManagementService, ProductCategoriesManagementDataAccessService>()
            .AddTransient<IProductCategoryPictureService, ProductCategoryPictureService>()
            .AddTransient<IEmployeeManagementService, EmployeeManagementDataAccessService>()
            .AddTransient<IEmployeePictureService, EmployeePictureManagementDataAccessService>()
            .AddAutoMapper(typeof(MappingProfile))
            .AddTransient<ICustomerService, CustomerService>()
            .AddTransient<IBloggingService, BloggingService>()
            .AddTransient<IBloggingCommentService, BloggingCommentService>()
            .AddTransient<IBloggingProductLinkService, BloggingProductLinkService>()

            .AddDbContext<NorthwindContext>(options => options.UseSqlServer(configuration.GetConnectionString("SqlConnection")))
            .AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "NorthWindEFCore", Version = "v1" });
            });
        }
    }
}
