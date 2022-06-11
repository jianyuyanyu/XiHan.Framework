﻿// ----------------------------------------------------------------
// Copyright ©2022 ZhaiFanhua All Rights Reserved.
// FileName:BlogComment
// Guid:60383ed1-8cd3-43d1-85e8-8b3dc45cdc7e
// Author:zhaifanhua
// Email:me@zhaifanhua.com
// CreateTime:2022-05-08 下午 06:25:47
// ----------------------------------------------------------------

using SqlSugar;
using System.Net;
using ZhaiFanhuaBlog.Models.Bases;

namespace ZhaiFanhuaBlog.Models.Blogs;

/// <summary>
/// 文章评论表
/// </summary>
public class BlogComment : BaseEntity
{
    /// <summary>
    /// 评论者
    /// </summary>
    public Guid AccountId { get; set; }

    /// <summary>
    /// 父级评论
    /// </summary>
    [SugarColumn(IsNullable = true)]
    public Guid ParentId { get; set; }

    /// <summary>
    /// 所属文章
    /// </summary>
    public Guid ArticleId { get; set; }

    /// <summary>
    /// 评论内容
    /// </summary>
    [SugarColumn(ColumnDataType = "nvarchar(4000)")]
    public string? TheContent { get; set; } = null;

    /// <summary>
    /// 评论者IP(显示地区)
    /// </summary>
    [SugarColumn(ColumnDataType = "varbinary(16)", IsNullable = true)]
    public IPAddress? CommentIp { get; set; }

    /// <summary>
    /// 评论点赞数
    /// </summary>
    public int PollCount { get; set; } = 0;

    /// <summary>
    /// 是否置顶 是(true)否(false)
    /// </summary>
    public bool IsTop { get; set; } = false;
}