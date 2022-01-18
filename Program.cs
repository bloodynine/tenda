global using FastEndpoints;
global using FastEndpoints.Validation;
using FastEndpoints.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using MongoDB.Entities;
using Tenda.Shared.Hubs;
using Tenda.Shared.Models;
using Tenda.Shared.Services;
using Tenda.Users;
using Tenda.Utilities;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOptions();
// Add services to the container.

builder.Services.Configure<JsonOptions>(o => o.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter()));

builder.Services.AddControllers();
builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("Database")
);
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt")
);
builder.Services.AddScoped<IGetByMonthService, GetByMonthService>();
builder.Services.AddScoped<ITotalService, TotalService>();
builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
builder.Services.AddScoped<ILoginService, LoginService>();
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();
builder.Services.AddAuthenticationJWTBearer(jwtSettings.Key);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddFastEndpoints();
builder.Services.AddSpaStaticFiles(x => x.RootPath = "wwwroot");

builder.Services.AddSignalR();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment()) app.UseSpaStaticFiles();

Task.Run(async () =>
{
    var settings = app.Configuration.GetSection("Database").Get<DatabaseSettings>();
    await DB.InitAsync(
        settings.DatabaseName,
        settings.Host, settings.Port);
}).GetAwaiter().GetResult();


// app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseDefaultExceptionHandler();
app.UseFastEndpoints();
app.UseSwagger();
app.UseSwaggerUI();

app.MapHub<ResolvedTotal>("api/resolvedTotal");
app.MapWhen(y => !y.Request.Path.StartsWithSegments("/api"), x =>
{
    x.UseSpa(spa =>
    {
        // spa.Options.SourcePath = "ClientApp";
        if (app.Environment.IsDevelopment())
        {
            spa.Options.SourcePath = "ClientApp";
            spa.UseAngularCliServer("start");
        }
    });
});

app.Run();