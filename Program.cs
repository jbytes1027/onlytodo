using OnlyTodo.Data;
using OnlyTodo.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
{
    policy.AllowAnyOrigin(); policy.AllowAnyHeader();
}));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<OnlyTodoContext>();
builder.Services.AddScoped<TaskService>();

var app = builder.Build();
app.UseCors();
app.MapControllers();

app.Run();

public partial class Program { }