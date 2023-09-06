﻿#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2022 ZhaiFanhua All Rights Reserved.
// Licensed under the MulanPSL2 License. See LICENSE in the project root for license information.
// FileName:BlogFollow
// Guid:196d9961-eb5f-4e8d-807d-a29b87a0a4f9
// Author:zhaifanhua
// Email:me@zhaifanhua.com
// CreatedTime:2022-05-08 下午 06:05:37
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

using SqlSugar;
using XiHan.Models.Bases.Attributes;
using XiHan.Models.Bases.Entities;

namespace XiHan.Models.Blogs;

/// <summary>
/// 博客关注表
/// </summary>
/// <remarks>记录新增信息</remarks>
[SystemTable]
[SugarTable(TableName = "Blog_Follow")]
public class BlogFollow : BaseCreateEntity
{
    /// <summary>
    /// 关注用户
    /// </summary>
    public long FollowedId { get; set; }

    /// <summary>
    /// 备注名称
    /// </summary>
    [SugarColumn(Length = 64, IsNullable = true)]
    public string? RemarkName { get; set; }
}