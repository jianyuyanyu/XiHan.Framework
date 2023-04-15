﻿#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2022 ZhaiFanhua All Rights Reserved.
// FileName:MiniProfilerSetup
// Guid:895a4bbf-54a4-47ca-98d9-78c59cc6b91b
// Author:zhaifanhua
// Email:me@zhaifanhua.com
// CreateTime:2022-09-29 上午 01:49:21
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

using Microsoft.AspNetCore.Builder;
using XiHan.Infrastructure.Apps.Setting;

namespace XiHan.Extensions.Setups.Application;

/// <summary>
/// MiniProfilerSetup
/// </summary>
public static class MiniProfilerSetup
{
    /// <summary>
    /// 性能分析
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IApplicationBuilder UseMiniProfilerSetup(this IApplicationBuilder app)
    {
        if (app == null)
        {
            throw new ArgumentNullException(nameof(app));
        }

        var isEnabledMiniprofiler = AppSettings.Miniprofiler.IsEnabled.GetValue();
        if (!isEnabledMiniprofiler) return app;
        // 性能分析
        app.UseMiniProfiler();

        return app;
    }
}