using DevIO.Api.Extensions;
using Elmah.Io.Extensions.Logging;

namespace DevIO.Api.Configuration
{
    public static class LoggerConfig
    {
        public static IServiceCollection AddLoggingConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddElmahIo(o =>
            {
                o.ApiKey = "5d1bfda7f95d4d978a8fd983f312f31e";
                o.LogId = new Guid("70622ee3-da7a-45ef-b952-22a01dde4b81");
            });

            services.AddLogging(builder =>
            {
                builder.AddElmahIo(options =>
                {
                    options.ApiKey = "5d1bfda7f95d4d978a8fd983f312f31e";
                    options.LogId = new Guid("70622ee3-da7a-45ef-b952-22a01dde4b81");
                });
                builder.AddFilter<ElmahIoLoggerProvider>(null, LogLevel.Warning);
            });

            services.AddHealthChecks()
                .AddElmahIoPublisher(options =>
                {
                    options.ApiKey = "5d1bfda7f95d4d978a8fd983f312f31e";
                    options.LogId = new Guid("70622ee3-da7a-45ef-b952-22a01dde4b81");
                    options.HeartbeatId = "API Fornecedores";
                    options.Application = "API Fornecedor";

                })
                .AddCheck("Produtos", new SqlServerHealthCheck(configuration.GetConnectionString("DefaultConnection")))
                .AddSqlServer(configuration.GetConnectionString("DefaultConnection"), name: "BancoSQL");

            services.AddHealthChecksUI()
                .AddSqlServerStorage(configuration.GetConnectionString("DefaultConnection"));

            return services;
        }

        public static IApplicationBuilder UseLoggingConfiguration(this IApplicationBuilder app)
        {
            app.UseElmahIo();

            return app;
        }
    }
}