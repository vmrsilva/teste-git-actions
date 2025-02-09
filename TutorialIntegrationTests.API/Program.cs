
using Microsoft.EntityFrameworkCore;

namespace TutorialIntegrationTests.API;

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

        builder.Services.AddDbContext<TodoContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });


        //using (ServiceProvider serviceProvider = builder.Services.BuildServiceProvider())
        //{
        //    var dbContext = serviceProvider.GetRequiredService<TodoContext>();
        //    dbContext.Database.Migrate();
        //}

        //AA.AddInfraestructure(builder.Services, builder.Configuration);

        builder.Services.AddScoped<ITodoService, TodoService>();

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

        app.Run();
    }
}
