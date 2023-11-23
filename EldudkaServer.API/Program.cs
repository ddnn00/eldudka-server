using Hangfire;
using Hangfire.PostgreSql;
using EldudkaServer.Repositories;
using EldudkaServer.Jobs;
using EldudkaServer.Context;
using EldudkaServer.Services;

namespace EldudkaServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add Hangfire services.
            builder.Services.AddHangfire(
                configuration =>
                    configuration
                        .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                        .UseSimpleAssemblyNameTypeSerializer()
                        .UseRecommendedSerializerSettings()
                        .UsePostgreSqlStorage(builder.Configuration["pgSqlConnectionString"])
            );

            // Add the processing server as IHostedService
            builder.Services.AddHangfireServer();

            builder.Services.AddSingleton<DapperContext>();

            // Repositories registration
            builder.Services.AddTransient<IProductRepository, ProductRepository>();
            builder.Services.AddTransient<IOrderRepository, OrderRepository>();

            // Services registration
            builder.Services.AddTransient<ICloudShopService, CloudShopService>();
            builder.Services.AddTransient<IProductService, ProductService>();

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.UseHangfireDashboard();

            // Jobs registration
            if (bool.Parse(builder.Configuration["jobsIsActive"]))
            {
                RecurringJob.AddOrUpdate<ProductSync>(
                    "ProductSync",
                    (x) => x.Start(),
                    "*/2 * * * *"
                );
            }

            app.UseCors();

            app.Run();
        }
    }
}
