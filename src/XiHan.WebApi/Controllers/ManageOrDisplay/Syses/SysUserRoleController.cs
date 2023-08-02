﻿#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2023 ZhaiFanhua All Rights Reserved.
// Licensed under the MulanPSL2 License. See LICENSE in the project root for license information.
// FileName:SysUserRoleController
// Guid:a5768a4e-d914-4d5d-9bda-8844a96638c7
// Author:Administrator
// Email:me@zhaifanhua.com
// CreateTime:2023-07-19 下午 05:50:43
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

using Microsoft.AspNetCore.Authorization;
using XiHan.Application.Common.Swagger;
using XiHan.WebApi.Controllers.Bases;

namespace XiHan.WebApi.Controllers.ManageOrDisplay.Syses;

/// <summary>
/// 系统用户角色管理
/// </summary>
[Authorize]
[ApiGroup(ApiGroupNames.Manage)]
public class SysUserRoleController : BaseApiController
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public SysUserRoleController()
    {
    }
}