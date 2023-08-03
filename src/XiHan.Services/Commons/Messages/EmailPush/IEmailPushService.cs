﻿#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2022 ZhaiFanhua All Rights Reserved.
// Licensed under the MulanPSL2 License. See LICENSE in the project root for license information.
// FileName:IEmailPushService
// Guid:a4bee990-b81b-4883-a722-908a55905543
// Author:zhaifanhua
// Email:me@zhaifanhua.com
// CreatedTime:2022-12-07 下午 01:32:01
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

using XiHan.Infrastructures.Apps.Logging;
using XiHan.Infrastructures.Responses.Results;
using XiHan.Models.Syses;
using XiHan.Services.Bases;

namespace XiHan.Services.Commons.Messages.EmailPush;

/// <summary>
/// IEmailPushService
/// </summary>
public interface IEmailPushService : IBaseService<SysEmail>
{
    /// <summary>
    /// 发送验证邮件
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="userEmail"></param>
    /// <param name="verificationCode"></param>
    /// <returns></returns>
    Task<CustomResult> SendVerificationCodeEmail(string userName, string userEmail, string verificationCode);
}