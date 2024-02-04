using CoreStartApp.Middlewares;
using Microsoft.AspNetCore.Builder;
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

//app.Use(async (context, next) =>
//{
//    // Строка для публикации в лог
//    string logMessage = $"[{DateTime.Now}]: New request to http://{context.Request.Host.Value + context.Request.Path}{Environment.NewLine}";

//    // Путь до лога (опять-таки, используем свойства IWebHostEnvironment)
//    string logFilePath = Path.Combine(env.ContentRootPath, "Logs", "RequestLog.txt");

//    // Используем асинхронную запись в файл
//    await File.AppendAllTextAsync(logFilePath, logMessage);

//    Console.WriteLine(logMessage);

//    await next.Invoke();
//});

////Добавляем компонент для логирования запросов с использованием метода Use.
//app.Use(async (context, next) =>
//{
//    // Для логирования данных о запросе используем свойства объекта HttpContext
//    Console.WriteLine($"[{DateTime.Now}]: New request to http://{context.Request.Host.Value + context.Request.Path}");
//    await next.Invoke();
//});

//app.MapGet("/", () => "Hello World!");

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

//app.Run(async (context) =>
//{
//    await context.Response.WriteAsync($"Welcome to the {env.ApplicationName}!");
//});


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
    // Получаем сервис IWebHostEnvironment из IApplicationBuilder
    var env = app.ApplicationServices.GetService<IWebHostEnvironment>();

    app.Run(async context =>
    {
        await context.Response.WriteAsync($"App name: {env.ApplicationName}. App running configuration: {env.EnvironmentName}");
    });
}