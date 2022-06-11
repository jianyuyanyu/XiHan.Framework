﻿// ----------------------------------------------------------------
// Copyright ©2022 ZhaiFanhua All Rights Reserved.
// FileName:Program
// Guid:fccfeb28-624c-41cb-9c5c-0b0652648a6b
// Author:zhaifanhua
// Email:me@zhaifanhua.com
// CreateTime:2022-05-17 下午 04:01:21
// ----------------------------------------------------------------

using ZhaiFanhuaBlog.Utils.Console;
using ZhaiFanhuaBlog.WebApi.Common.Extensions.Console;
using ZhaiFanhuaBlog.WebApi.Common.Extensions.DependencyInjection;

namespace ZhaiFanhuaBlog.WebApi;

/// <summary>
/// Program
/// </summary>
public class Program
{
    /// <summary>
    /// 入口
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        ConsoleHelper.WriteInfoLine("Log Start……");
        var log = builder.Logging;
        // log.AddLog4Net(@"Common/Config/log4net.config");
        ConsoleHelper.WriteSuccessLine("Log Started Successfully！");
        ConsoleHelper.WriteInfoLine("Configuration Start……");
        var config = builder.Configuration;
        ConsoleHelper.WriteSuccessLine("Configuration Started Successfully！");
        ConsoleHelper.WriteInfoLine("Services Start……");

        var services = builder.Services;
        // Controllers
        services.AddCustomControllers(config);
        // Swagger
        services.AddCustomSwagger(config);
        // SqlSugar
        services.AddCustomSqlSugar(config);
        // IOC
        services.AddCustomIOC(config);
        // JWT
        services.AddCustomJWT(config);
        // AutoMapper
        services.AddCustomAutoMapper(config);
        // Route
        services.AddCustomRoute(config);
        // Cors
        services.AddCustomCors(config);
        ConsoleHelper.WriteSuccessLine("Services Started Successfully！");
        ConsoleHelper.WriteInfoLine("ZhaiFanhuaBlog Application Start……");

        var app = builder.Build();
        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        // Swagger
        app.UseCustomSwagger(config);
        // 跨域
        app.UseCors();
        // 跳转https
        app.UseHttpsRedirection();
        // 使用静态文件
        app.UseStaticFiles();
        // 路由
        app.UseRouting();
        // 鉴权
        app.UseAuthentication();
        // 授权
        app.UseAuthorization();

        app.MapControllers();
        // 打印信息
        ConsoleInfo.ConsoleInfos();
        app.Run();
    }
}