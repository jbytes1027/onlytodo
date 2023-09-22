using OnlyTodo.Data;
using OnlyTodo.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<OnlyTodoContext>();
builder.Services.AddScoped<TaskService>();

var app = builder.Build();
app.MapControllers();

app.Run();
