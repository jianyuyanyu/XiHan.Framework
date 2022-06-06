﻿// ----------------------------------------------------------------
// Copyright ©2022 ZhaiFanhua All Rights Reserved.
// FileName:UserStatistic
// Guid:23d7c499-56e3-4d23-835b-80aab213bb38
// Author:zhaifanhua
// Email:me@zhaifanhua.com
// CreateTime:2022-05-08 下午 06:16:33
// ----------------------------------------------------------------

using ZhaiFanhuaBlog.Models.Bases;

namespace ZhaiFanhuaBlog.Models.Users;

/// <summary>
/// 用户统计表
/// </summary>
public class UserStatistic : BaseEntity
{
    /// <summary>
    /// 所属用户
    /// </summary>
    public Guid AccountId { get; set; }

    /// <summary>
    /// 关注数量
    /// </summary>
    public int FollowsCount { get; set; } = 0;

    /// <summary>
    /// 粉丝数量
    /// </summary>
    public int FocusCount { get; set; } = 0;

    /// <summary>
    /// 收藏数量
    /// </summary>
    public int CollectsCount { get; set; } = 0;

    /// <summary>
    /// 通知数量
    /// </summary>
    public int NoticesCount { get; set; } = 0;

    /// <summary>
    /// 登录次数
    /// </summary>
    public int LoginCount { get; set; } = 0;
}