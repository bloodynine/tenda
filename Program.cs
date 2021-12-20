using System.Text.Json;
using FastEndpoints;
using FastEndpoints.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Options;
using MongoDB.Entities;
using Tenda.Shared.Hubs;
using Tenda.Shared.Services;
using Tenda.Users;
using Tenda.Utilities;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<JsonOptions>(o => o.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter()));

builder.Services.AddControllers();
builder.Services.AddScoped<IGetByMonthService, GetByMonthService>();
builder.Services.AddScoped<ITotalService, TotalService>();
builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddAuthenticationJWTBearer("FDJSKLfdjsklw93nfugh84hrew99");

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddFastEndpoints();
builder.Services.AddSpaStaticFiles(x => x.RootPath = "ClientApp/dist");
builder.Services.AddSignalR();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    app.UseSpaStaticFiles();
}

Task.Run(async () =>
{
    await DB.InitAsync("KW", "localhost", 27017);
}).GetAwaiter().GetResult();


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseFastEndpoints();
app.UseSwagger();
app.UseSwaggerUI();

app.MapHub<ResolvedTotal>("api/resolvedTotal");
app.MapWhen(y => !y.Request.Path.StartsWithSegments("/api"), x =>
{
    x.UseSpa(spa =>
    {
        spa.Options.SourcePath = "ClientApp";
        if (app.Environment.IsDevelopment())
        {
            spa.UseAngularCliServer(npmScript: "start");
        }
    });
});

app.Run();
