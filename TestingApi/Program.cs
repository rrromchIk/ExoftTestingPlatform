using Microsoft.EntityFrameworkCore;
using TestingApi.Data;
using TestingApi.Extensions;
using TestingApi.Middlewares;


var builder = WebApplication.CreateBuilder(args);

builder.Logging
    .ClearProviders()
    .SetMinimumLevel(LogLevel.Trace)
    .AddSimpleConsole(options => { options.IncludeScopes = true; })
    .AddDebug();

builder.Services.RegisterCustomServices();
builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options => {
    options.AddPolicy("AllowAny", builder => {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseCors("AllowAny");
app.UseAuthorization();

app.MapControllers();

app.Run();