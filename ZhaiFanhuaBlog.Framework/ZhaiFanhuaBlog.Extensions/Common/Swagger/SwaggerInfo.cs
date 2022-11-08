﻿// ----------------------------------------------------------------
// Copyright ©2022 ZhaiFanhua All Rights Reserved.
// FileName:SwaggerInfo
// Guid:fb9017cd-48ca-4cbc-93cf-f9372a9606af
// Author:zhaifanhua
// Email:me@zhaifanhua.com
// CreateTime:2022-06-02 上午 11:48:18
// ----------------------------------------------------------------

using Microsoft.OpenApi.Models;

namespace ZhaiFanhuaBlog.Extensions.Common.Swagger;

/// <summary>
/// SwaggerInfo
/// </summary>
public class SwaggerInfo
{
    /// <summary>
    /// URL前缀
    /// </summary>
    public string? UrlPrefix { get; set; }

    /// <summary>
    /// 分组名称
    /// </summary>
    public string? GroupName { get; set; }

    /// <summary>
    /// <see cref="Microsoft.OpenApi.Models.OpenApiInfo"/>
    /// </summary>
    public OpenApiInfo? OpenApiInfo { get; set; }

    /// <summary>
    /// Swagger分组信息，将进行遍历使用
    /// </summary>
    public static readonly List<SwaggerInfo> SwaggerInfos = new()
    {
        new SwaggerInfo
        {
            UrlPrefix = SwaggerGroup.Reception,
            GroupName = SwaggerGroup.Reception,
            OpenApiInfo = new OpenApiInfo
            {
                Title = "前台接口",
                Description = "这是用于普通用户浏览的博客前台接口",
            }
        },
        new SwaggerInfo
        {
            UrlPrefix = SwaggerGroup.Backstage,
            GroupName = SwaggerGroup.Backstage,
            OpenApiInfo = new OpenApiInfo
            {
                Title = "后台接口",
                Description = "这是用于管理的博客后台接口"
            }
        },
        new SwaggerInfo
        {
            UrlPrefix = SwaggerGroup.Authorize,
            GroupName = SwaggerGroup.Authorize,
            OpenApiInfo = new OpenApiInfo
            {
                Title = "授权接口",
                Description = "这是用于登录的博客授权接口"
            }
        },
        new SwaggerInfo
        {
            UrlPrefix = SwaggerGroup.Common,
            GroupName = SwaggerGroup.Common,
            OpenApiInfo = new OpenApiInfo
            {
                Title = "公共接口",
                Description = "这是用于常用功能的公共接口"
            }
        },
        new SwaggerInfo
        {
            UrlPrefix = SwaggerGroup.Test,
            GroupName = SwaggerGroup.Test,
            OpenApiInfo = new OpenApiInfo
            {
                Title = "测试接口",
                Description = "这是用于测试的博客测试接口"
            }
        }
    };
}