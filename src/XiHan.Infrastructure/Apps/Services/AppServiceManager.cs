﻿#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2022 ZhaiFanhua All Rights Reserved.
// FileName:AppServiceManager
// Guid:bded17a9-b219-467a-b2e8-f8e38a454a04
// Author:zhaifanhua
// Email:me@zhaifanhua.com
// CreateTime:2022-12-24 上午 01:57:42
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Reflection;
using XiHan.Infrastructure.Enums;
using XiHan.Utils.Consoles;
using XiHan.Utils.Infos.IpLocation;
using XiHan.Utils.Infos.IpLocation.Ip2region;

namespace XiHan.Infrastructure.Apps.Services;

/// <summary>
/// 服务管理器
/// </summary>
public static class AppServiceManager
{
    /// <summary>
    /// 注册服务
    /// </summary>
    /// <param name="services"></param>
    public static void RegisterService(IServiceCollection services)
    {
        RegisterBaseService(services);
        RegisterSelfService(services);
    }

    /// <summary>
    /// 注册基础服务
    /// </summary>
    /// <param name="services"></param>
    public static void RegisterBaseService(IServiceCollection services)
    {
        // Ip 查询服务
        services.AddSingleton<ISearcher, Searcher>();
        IpSearchHelper.IpDbPath = Path.Combine(AppContext.BaseDirectory, "ConfigData", "ip2region.xdb");
    }

    /// <summary>
    /// 注册自身服务
    /// </summary>
    /// <param name="services"></param>
    public static void RegisterSelfService(IServiceCollection services)
    {
        // 所有涉及服务的组件库
        string[] libraries = new string[] { "XiHan.Repositories", "XiHan.Services", "XiHan.Tasks" };
        // 根据程序路径反射出所有引用的程序集
        var referencedTypes = new List<Type>();
        foreach (var library in libraries)
        {
            try
            {
                var assemblyTypes = Assembly.Load(library).GetTypes()
                    .Where(type => type.GetCustomAttribute<AppServiceAttribute>() != null);
                referencedTypes.AddRange(assemblyTypes);
            }
            catch (Exception ex)
            {
                var errorMsg = $"找不到{library}组件库！";
                Log.Error(ex, errorMsg);
                errorMsg.WriteLineError();
            }
        }
        // 批量注入
        foreach (var type in referencedTypes)
        {
            // 服务周期
            var serviceAttribute = type.GetCustomAttribute<AppServiceAttribute>();
            if (serviceAttribute == null) continue;
            var serviceType = serviceAttribute.ServiceType;

            // 情况1 适用于依赖抽象编程，这里只获取第一个
            if (serviceType == null && serviceAttribute.IsInterfaceServiceType)
            {
                serviceType = type.GetInterfaces().FirstOrDefault();
            }
            // 情况2 不常见特殊情况下才会指定ServiceType，写起来麻烦
            if (serviceType == null)
            {
                serviceType = type;
            }
            // 判断是否实现了该接口，若是，则注入服务
            else if (serviceType != null && serviceType.IsAssignableFrom(type))
            {
                switch (serviceAttribute.ServiceLifetime)
                {
                    case ServiceLifeTimeEnum.Singleton:
                        services.AddSingleton(serviceType, type);
                        break;

                    case ServiceLifeTimeEnum.Scoped:
                        services.AddScoped(serviceType, type);
                        break;

                    case ServiceLifeTimeEnum.Transient:
                        services.AddTransient(serviceType, type);
                        break;

                    default:
                        services.AddTransient(serviceType, type);
                        break;
                }
                var infoMsg = $"服务注册：{serviceType.Name}-{type.Name}";
                Log.Information(infoMsg);
                infoMsg.WriteLineSuccess();
            }
        }
    }
}