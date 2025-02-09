using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;

using TutorialIntegrationTests.API;

namespace TutorialIntegrationTests.Tests;

public class ApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    //private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder().WithUsername("postgres")
    //                                                                        .WithPassword("postgres")
    //                                                                        .Build();

    private readonly MsSqlContainer _sql = new MsSqlBuilder().Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            ConfigureDbContext(services);
        });

        builder.UseEnvironment("Development");
        base.ConfigureWebHost(builder);
    }

    private void ConfigureDbContext(IServiceCollection services)
    {
        var context = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(TodoContext));
        if (context != null)
        {
            services.Remove(context);
            var options = services.Where(r => (r.ServiceType == typeof(DbContextOptions))
              || (r.ServiceType.IsGenericType && r.ServiceType.GetGenericTypeDefinition() == typeof(DbContextOptions<>))).ToArray();
            foreach (var option in options)
            {
                services.Remove(option);
            }
        }

        services.AddDbContext<TodoContext>(options =>
        {
            options.UseSqlServer(_sql.GetConnectionString());
        });
    }

    public async Task InitializeAsync()
    {
        await _sql.StartAsync();
        var x = _sql.GetConnectionString();

        var aa = x;
    }

    public async new Task DisposeAsync()
    {
        await _sql.StopAsync();
    }
}
