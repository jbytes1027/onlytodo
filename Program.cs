using Microsoft.EntityFrameworkCore;
using OnlyTodo.Data;
using OnlyTodo.Services;

var builder = WebApplication.CreateBuilder(args);

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
if (connectionString is null) throw new Exception("No Connection String Found");
builder.Services.AddDbContext<OnlyTodoContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddScoped<TaskService>();

var app = builder.Build();
app.UseCors();
app.MapControllers();

app.Run();

public partial class Program { }