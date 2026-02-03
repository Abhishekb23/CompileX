using CompileX.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<CodeExecutionService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", p =>
        p.AllowAnyOrigin()
         .AllowAnyHeader()
         .AllowAnyMethod());
});

var app = builder.Build();

app.UseCors("AllowAngular");

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.Run();
