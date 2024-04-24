﻿#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2024 ZhaiFanhua All Rights Reserved.
// Licensed under the MulanPSL2 License. See LICENSE in the project root for license information.
// FileName:ApplicationWithExternalServiceProvider
// Guid:3f21f488-c1cc-4fd3-b658-5671f163c222
// Author:Administrator
// Email:me@zhaifanhua.com
// CreateTime:2024-04-24 上午 11:44:05
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using XiHan.Core.Application.Abstracts;
using XiHan.Core.Verification;

namespace XiHan.Core.Application;

/// <summary>
/// 具有外部服务的应用程序提供器
/// </summary>
internal class ApplicationWithExternalServiceProvider : ApplicationBase, IApplicationWithExternalServiceProvider
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="startupModuleType"></param>
    /// <param name="services"></param>
    /// <param name="optionsAction"></param>
    public ApplicationWithExternalServiceProvider(
        [NotNull] Type startupModuleType,
        [NotNull] IServiceCollection services,
        Action<ApplicationCreationOptions>? optionsAction) : base(startupModuleType, services, optionsAction)
    {
        services.AddSingleton<IApplicationWithExternalServiceProvider>(this);
    }

    /// <summary>
    /// 设置服务提供器，但不初始化模块
    /// </summary>
    void IApplicationWithExternalServiceProvider.SetServiceProvider([NotNull] IServiceProvider serviceProvider)
    {
        CheckHelper.NotNull(serviceProvider, nameof(serviceProvider));

        if (ServiceProvider != null)
        {
            if (ServiceProvider != serviceProvider)
            {
                throw new Exception("服务提供器之前已设置为另一个服务提供器实例！");
            }

            return;
        }

        SetServiceProvider(serviceProvider);
    }

    /// <summary>
    /// 设置服务提供器并初始化所有模块，异步
    /// 如果之前调用过 <see cref="IApplicationWithExternalServiceProvider.SetServiceProvider"/>，则应将相同的 <paramref name="serviceProvider"/> 实例传递给此方法
    /// </summary>
    public async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        CheckHelper.NotNull(serviceProvider, nameof(serviceProvider));

        SetServiceProvider(serviceProvider);

        await InitializeModulesAsync();
    }

    /// <summary>
    /// 设置服务提供器并初始化所有模块，异步
    /// 如果之前调用过 <see cref="IApplicationWithExternalServiceProvider.SetServiceProvider"/>，则应将相同的 <paramref name="serviceProvider"/> 实例传递给此方法
    /// </summary>
    public void Initialize([NotNull] IServiceProvider serviceProvider)
    {
        CheckHelper.NotNull(serviceProvider, nameof(serviceProvider));

        SetServiceProvider(serviceProvider);

        InitializeModules();
    }

    /// <summary>
    /// 释放
    /// </summary>
    public override void Dispose()
    {
        base.Dispose();

        if (ServiceProvider is IDisposable disposableServiceProvider)
        {
            disposableServiceProvider.Dispose();
        }
    }
}