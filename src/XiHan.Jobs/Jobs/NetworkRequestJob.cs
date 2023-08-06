﻿#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2023 ZhaiFanhua All Rights Reserved.
// Licensed under the MulanPSL2 License. See LICENSE in the project root for license information.
// FileName:NetworkRequestJob
// Guid:2ab05c0c-71d1-4150-888e-0b788377944a
// Author:Administrator
// Email:me@zhaifanhua.com
// CreateTime:2023-07-21 下午 04:38:21
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;
using Serilog;
using XiHan.Infrastructures.Apps.Services;
using XiHan.Infrastructures.Requests.Https;
using XiHan.Jobs.Bases;
using XiHan.Services.Syses.Jobs;
using XiHan.Utils.Exceptions;
using XiHan.Utils.Extensions;

namespace XiHan.Jobs.Jobs;

/// <summary>
/// 网络请求任务
/// </summary>
[AppService(ServiceType = typeof(NetworkRequestJob), ServiceLifetime = ServiceLifeTimeEnum.Scoped)]
public class NetworkRequestJob : JobBase, IJob
{
    private static readonly ILogger Logger = Log.ForContext<SqlStatementJob>();

    private readonly IHttpPollyService _httpPollyService;
    private readonly ISysJobService _sysJobService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="httpPollyService"></param>
    /// <param name="sysJobService"></param>
    public NetworkRequestJob(IHttpPollyService httpPollyService, ISysJobService sysJobService)
    {
        _httpPollyService = httpPollyService;
        _sysJobService = sysJobService;
    }

    /// <summary>
    /// 执行
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task Execute(IJobExecutionContext context)
    {
        await ExecuteJob(context, async () => await NetworkRequest(context));
    }

    /// <summary>
    /// 网络请求
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    private async Task NetworkRequest(IJobExecutionContext context)
    {
        if (context is JobExecutionContextImpl { Trigger: AbstractTrigger trigger })
        {
            var info = await _sysJobService.GetByIdAsync(trigger.JobName) ?? throw new CustomException($"网络请求任务【{trigger?.JobName}】执行失败，任务不存在！");
            var url = info.ApiUrl;
            var paras = info.Params;

            if (url.IsNullOrEmpty() && paras.IsNullOrEmpty())
                throw new CustomException($"网络请求任务【{trigger.JobName}】执行失败，参数为空！");

            // POST请求
            string result;
            if (info.RequestMethod != null && info.RequestMethod == RequestMethodEnum.Post.GetEnumValueByKey())
            {
                result = await _httpPollyService.PostAsync(HttpGroupEnum.Remote, url!, paras!);
            }
            // GET请求
            else
            {
                if (url!.IndexOf("?", StringComparison.Ordinal) > -1)
                {
                    url += "&" + paras;
                }
                else
                {
                    url += "?" + paras;
                }

                result = await _httpPollyService.GetAsync(HttpGroupEnum.Remote, url);
            }

            Logger.Information($"网络请求任务【{info.Name}】执行成功，请求结果为：" + result);
        }
    }
}