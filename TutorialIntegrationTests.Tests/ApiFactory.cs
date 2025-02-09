using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.InteropServices;
using Testcontainers.MsSql;

using TutorialIntegrationTests.API;

namespace TutorialIntegrationTests.Tests;

public class ApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _msSqlContainer;
    public ApiFactory()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            _msSqlContainer = new MsSqlBuilder()
                .WithImage("mcr.microsoft.com/mssql/server:2019-latest")
                  .WithPassword("password(!)Strong")
                         .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(1433))
                         .Build();
        }
        else
        {
            _msSqlContainer = new MsSqlBuilder().Build();
        }
    }

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
            options.UseSqlServer(_msSqlContainer.GetConnectionString());
        });
    }

    public async Task InitializeAsync()
    {
        await _msSqlContainer.StartAsync();
        var x = _msSqlContainer.GetConnectionString();

        var aa = x;
    }

    public async new Task DisposeAsync()
    {
        await _msSqlContainer.StopAsync();
    }
}
