﻿// ----------------------------------------------------------------
// Copyright ©2022 ZhaiFanhua All Rights Reserved.
// FileName:UserAuthorityDto
// Guid:e063ddee-794e-4927-9617-5f0cc77815b9
// Author:zhaifanhua
// Email:me@zhaifanhua.com
// CreateTime:2022-05-15 下午 06:07:09
// ----------------------------------------------------------------

using ZhaiFanhuaBlog.Models.Bases;

namespace ZhaiFanhuaBlog.ViewModels.Users;

/// <summary>
/// 用户权限
/// </summary>
public class UserAuthorityDto
{
    /// <summary>
    /// 父级权限
    /// </summary>
    public Guid? ParentId { get; set; }

    /// <summary>
    /// 权限名称
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 权限类型
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// 权限描述
    /// </summary>
    public string? Description { get; set; }
}