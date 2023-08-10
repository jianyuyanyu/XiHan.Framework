﻿#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2023 ZhaiFanhua All Rights Reserved.
// Licensed under the MulanPSL2 License. See LICENSE in the project root for license information.
// FileName:GlobalConst
// Guid:6c5799dd-8d27-4fb0-9a12-59857ee66c84
// Author:zhaifanhua
// Email:me@zhaifanhua.com
// CreatedTime:2023-06-13 下午 10:40:48
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

using XiHan.Infrastructures.Infos;

namespace XiHan.Infrastructures.Consts;

/// <summary>
/// 全局静态常量
/// </summary>
public static class GlobalConst
{
    /// <summary>
    /// 是否演示模式
    /// </summary>
    public const string IsDemoMode = "IsDemoMode";

    /// <summary>
    /// 默认密码
    /// </summary>
    public const string DefaultPassword = "XH_password_123";

    /// <summary>
    /// 默认角色编码
    /// </summary>
    public const string DefaultRole = "sys_role_user";

    /// <summary>
    /// 系统管理员角色编码
    /// </summary>
    public const string SysAdminRole = "sys_role_admin";

    /// <summary>
    /// 上传根路径
    /// </summary>
    public static string RootUploadPath => Path.Combine(ApplicationInfoHelper.BaseDirectory, "Uploads");

    /// <summary>
    /// 导出根路径
    /// </summary>
    public static string RootExportPath => Path.Combine(ApplicationInfoHelper.BaseDirectory, "Exports");

    /// <summary>
    /// 导入根路径
    /// </summary>
    public static string RootImportPath => Path.Combine(ApplicationInfoHelper.BaseDirectory, "Imports");

    /// <summary>
    /// 导入模板根路径
    /// </summary>
    public static string RootImportTemplatePath => Path.Combine(ApplicationInfoHelper.BaseDirectory, "Imports", "Templates");
}