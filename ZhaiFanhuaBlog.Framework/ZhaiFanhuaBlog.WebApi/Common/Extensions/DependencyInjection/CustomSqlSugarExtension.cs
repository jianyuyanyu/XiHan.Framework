﻿// ----------------------------------------------------------------
// Copyright ©2022 ZhaiFanhua All Rights Reserved.
// FileName:CustomSqlSugarExtension
// Guid:49736281-4a15-48db-ba3e-b66124a931d4
// Author:zhaifanhua
// Email:me@zhaifanhua.com
// CreateTime:2022-05-26 下午 06:24:14
// ----------------------------------------------------------------

using SqlSugar.IOC;

namespace ZhaiFanhuaBlog.WebApi.Common.Extensions.DependencyInjection;

/// <summary>
/// CustomSqlSugarExtension
/// </summary>
public static class CustomSqlSugarExtension
{
    /// <summary>
    /// SqlSugar扩展
    /// </summary>
    /// <param name="services"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static IServiceCollection AddCustomSqlSugar(this IServiceCollection services, IConfiguration config)
    {
        services.AddSqlSugar(config["Database:Type"] switch
        {
            "MySql" => new IocConfig()
            {
                DbType = IocDbType.MySql,
                ConnectionString = config["Database:ConnectionString:MySql"],
                IsAutoCloseConnection = true
            },
            "SqlServer" => new IocConfig()
            {
                DbType = IocDbType.SqlServer,
                ConnectionString = config["Database:ConnectionString:SqlServer"],
                IsAutoCloseConnection = true
            },
            "Sqlite" => new IocConfig()
            {
                DbType = IocDbType.Sqlite,
                ConnectionString = config["Database:ConnectionString:Sqlite"],
                IsAutoCloseConnection = true
            },
            "Oracle" => new IocConfig()
            {
                DbType = IocDbType.Oracle,
                ConnectionString = config["Database:ConnectionString:Oracle"],
                IsAutoCloseConnection = true
            },
            "PostgreSQL" => new IocConfig()
            {
                DbType = IocDbType.PostgreSQL,
                ConnectionString = config["Database:ConnectionString:PostgreSQL"],
                IsAutoCloseConnection = true
            },
            _ => new IocConfig()
            {
                DbType = IocDbType.SqlServer,
                ConnectionString = config["Database:ConnectionString:SqlServer"],
                IsAutoCloseConnection = true
            }
        });
        return services;
    }
}