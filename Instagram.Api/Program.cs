using Instagram.Api.Infrastructure;
using Microsoft.AspNetCore.Mvc;

[assembly: ApiController]
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();

builder.Services.RegisterDatabase(builder.Configuration);
builder.Services.RegisterApplication();
builder.Services.RegisterProxies();
builder.Services.ConfigureSwagger();

builder.ConfigureAuthorization();

builder.Services.AddHostedService<BanCancellationService>();

var app = builder.Build().MigrateDatabase();

await app.CreateAdmin();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(opt =>
{
    opt.AllowAnyMethod()
        .AllowAnyHeader()
        .WithOrigins("http://localhost:3000")
        .AllowCredentials();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }