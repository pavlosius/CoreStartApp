using CoreStartApp.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
var env = app.Environment;

if (env.IsDevelopment() || env.IsStaging())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();

app.UseStaticFiles();

app.UseMiddleware<LoggingMiddleware>();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", async context =>
    {
        await context.Response.WriteAsync($"Welcome to the {env.ApplicationName}!");
    });
});

app.Map("/about", About);

app.Map("/config", Config);

app.Run(async (context) =>
{
    await context.Response.WriteAsync($"Page not found");
});

app.Run();


static void About(IApplicationBuilder app)
{
    var env = app.ApplicationServices.GetService<IWebHostEnvironment>();

    app.Run(async context =>
    {
        await context.Response.WriteAsync($"{env.ApplicationName} - ASP.Net Core tutorial project");
    });
}

static void Config(IApplicationBuilder app)
{
    var env = app.ApplicationServices.GetService<IWebHostEnvironment>();

    app.Run(async context =>
    {
        await context.Response.WriteAsync($"App name: {env.ApplicationName}. App running configuration: {env.EnvironmentName}");
    });
}


/// <summary>
/// Статический метод, создающий и настраивающий IHostBuilder -
/// объект, который в свою очередь создает хост для развертывания Core-приложения
/// </summary>
static IHostBuilder CreateHostBuilder(string[] args) =>
   Host.CreateDefaultBuilder(args)
       .ConfigureWebHostDefaults(webBuilder =>
       {
           webBuilder.UseStartup<StartupBase>();
           // Переопределяем путь до статических файлов по умолчанию
           //webBuilder.UseWebRoot("Views");
       });