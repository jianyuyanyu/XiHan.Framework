﻿// ----------------------------------------------------------------
// Copyright ©2022 ZhaiFanhua All Rights Reserved.
// FileName:CustomLogExtension
// Guid:17417bc3-81d3-4124-a1e6-efe266d535cb
// Author:zhaifanhua
// Email:me@zhaifanhua.com
// CreateTime:2022-06-13 上午 04:05:22
// ----------------------------------------------------------------

using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace ZhaiFanhuaBlog.WebApi.Common.Extensions.DependencyInjection;

/// <summary>
/// CustomLogExtension
/// </summary>
public static class CustomLogExtension
{
    /// <summary>
    /// Log服务扩展
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static ILoggingBuilder AddCustomLog(this ILoggingBuilder builder)
    {
        string infoTemplate = @"================{NewLine}记录时间：{Timestamp:yyyy-MM-dd HH:mm:ss.fff}{NewLine}日志级别：{Level}{NewLine}请求类名：{SourceContext}{NewLine}消息描述：{Message}{NewLine}================{NewLine}{NewLine}";
        string otherTemplate = @"================{NewLine}记录时间：{Timestamp:yyyy-MM-dd HH:mm:ss.fff}{NewLine}日志级别：{Level}{NewLine}请求类名：{SourceContext}{NewLine}消息描述：{Message}{NewLine}错误详情：{Exception}{NewLine}================{NewLine}{NewLine}";
        string infoPath = Directory.GetCurrentDirectory() + @"\Logs\Info\.log";
        string waringPath = Directory.GetCurrentDirectory() + @"\Logs\Waring\.log";
        string errorPath = Directory.GetCurrentDirectory() + @"\Logs\Error\.log";
        string fatalPath = Directory.GetCurrentDirectory() + @"\Logs\Fatal\.log";
        Serilog.Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                // 日志调用类命名空间如果以 Microsoft 开头，覆盖日志输出最小级别为 Information
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                // 输出到控制台
                .WriteTo.Console()
                .WriteTo.Logger(log => log.Filter.ByIncludingOnly(lev => lev.Level == LogEventLevel.Information)
                // 异步输出到文件
                .WriteTo.Async(congfig => congfig.File(
                    // 配置日志输出到文件，文件输出到当前项目的 Logs 目录下
                    infoPath,
                    // 日记的生成周期为每天
                    rollingInterval: RollingInterval.Day,
                    // 单位字节不配置时，默认1GB
                    fileSizeLimitBytes: 1024 * 1024 * 10,
                    // 保留最近多少个文件，默认31个
                    retainedFileCountLimit: 10,
                    // 超过文件大小时，自动创建新文件
                    rollOnFileSizeLimit: true,
                    shared: true,
                    outputTemplate: infoTemplate)
                ))
                .WriteTo.Logger(log => log.Filter.ByIncludingOnly(lev => lev.Level == LogEventLevel.Warning)
                .WriteTo.Async(congfig => congfig.File(
                    waringPath,
                    rollingInterval: RollingInterval.Day,
                    fileSizeLimitBytes: 1024 * 1024 * 10,
                    retainedFileCountLimit: 10,
                    rollOnFileSizeLimit: true,
                    shared: true,
                    outputTemplate: otherTemplate)
                ))
                .WriteTo.Logger(log => log.Filter.ByIncludingOnly(lev => lev.Level == LogEventLevel.Error)
                .WriteTo.Async(congfig => congfig.File(
                    errorPath,
                    rollingInterval: RollingInterval.Day,
                    fileSizeLimitBytes: 1024 * 1024 * 10,
                    retainedFileCountLimit: 10,
                    rollOnFileSizeLimit: true,
                    shared: true,
                    outputTemplate: otherTemplate)
                ))
                .WriteTo.Logger(log => log.Filter.ByIncludingOnly(lev => lev.Level == LogEventLevel.Fatal)
                .WriteTo.Async(congfig => congfig.File(
                    fatalPath,
                    rollingInterval: RollingInterval.Day,
                    fileSizeLimitBytes: 1024 * 1024 * 10,
                    retainedFileCountLimit: 10,
                    rollOnFileSizeLimit: true,
                    shared: true,
                    outputTemplate: otherTemplate)
                ))
                .CreateLogger();
        builder.AddSerilog();
        return builder;
    }
}