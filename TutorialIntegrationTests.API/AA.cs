using Microsoft.EntityFrameworkCore;

namespace TutorialIntegrationTests.API
{
    public static class AA
    {
        public static void AddInfraestructure(this IServiceCollection services, IConfiguration configuration)
        {
            ConfigureContext(services, configuration);

        }

        public static void ConfigureContext(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TodoContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            //using (var serviceProvider = services.BuildServiceProvider())
            //{
            //    var dbContext = serviceProvider.GetRequiredService<TodoContext>();
            //    dbContext.Database.Migrate();

            //}
        }
    }
}
