using Instagram.Api.Hubs;
using Instagram.Api.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Serilog;

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

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Services.AddSerilog();

builder.Services.AddSignalR();

builder.Services.AddHostedService<BanCancellationService>();
builder.Services.AddHostedService<StoryDeletionService>();

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

app.UseSerilogRequestLogging();
app.MapControllers();

app.MapHub<NotificationHub>("ws/notification");
app.MapHub<DirectMessageHub>("ws/direct-message");

app.Run();

public partial class Program { }