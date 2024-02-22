using Microsoft.EntityFrameworkCore;
using OnlyTodo.Data;
using OnlyTodo.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
{
    policy.AllowAnyOrigin();
    policy.AllowAnyHeader();
    policy.AllowAnyMethod();
}));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

DotNetEnv.Env.TraversePath().Load();
builder.Configuration.AddEnvironmentVariables();

string? connectionString = builder.Configuration["CONNECTION_STRING"];

if (builder.Environment.EnvironmentName.Equals("Testing") || connectionString is null)
{
    builder.Services.AddDbContext<OnlyTodoContext>(options => options.UseInMemoryDatabase("TestingDb"));
}
else
{
    builder.Services.AddDbContext<OnlyTodoContext>(options => options.UseNpgsql(connectionString));
}

builder.Services.AddScoped<TaskService>();

WebApplication app = builder.Build();
app.UseCors();
app.MapControllers();

app.Run();

// Exposes class to testing
public partial class Program { }
