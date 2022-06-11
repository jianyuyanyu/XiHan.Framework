﻿// ----------------------------------------------------------------
// Copyright ©2022 ZhaiFanhua All Rights Reserved.
// FileName:SiteConfiguration
// Guid:826c51d5-2ef4-43cb-baf3-fc08fd843c19
// Author:zhaifanhua
// Email:me@zhaifanhua.com
// CreateTime:2022-05-08 下午 06:35:58
// ----------------------------------------------------------------

using SqlSugar;
using ZhaiFanhuaBlog.Models.Bases;

namespace ZhaiFanhuaBlog.Models.Sites;

/// <summary>
/// 网站配置表
/// </summary>
public class SiteConfiguration : BaseDeleteEntity<Guid>
{
    /// <summary>
    /// 网站名称
    /// </summary>
    [SugarColumn(ColumnDataType = "nvarchar(20)")]
    public string? Name { get; set; } = null;

    /// <summary>
    /// 网站描述
    /// </summary>
    [SugarColumn(ColumnDataType = "nvarchar(200)", IsNullable = true)]
    public string? Description { get; set; } = null;

    /// <summary>
    /// 网站关键字
    /// </summary>
    [SugarColumn(ColumnDataType = "nvarchar(200)", IsNullable = true)]
    public string? KeyWord { get; set; } = null;

    /// <summary>
    /// 网站域名
    /// </summary>
    [SugarColumn(ColumnDataType = "nvarchar(50)")]
    public string? Domain { get; set; } = null;

    /// <summary>
    /// 站长名称
    /// </summary>
    [SugarColumn(ColumnDataType = "nvarchar(20)")]
    public string? AdminName { get; set; } = null;

    /// <summary>
    /// 升级时间
    /// </summary>
    [SugarColumn(IsNullable = true)]
    public DateTime? UpdateTime { get; set; }
}