﻿#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2022 ZhaiFanhua All Rights Reserved.
// FileName:EmailModel
// Guid:86234008-5ebd-4723-9b94-98781e32e6ba
// Author:Administrator
// Email:me@zhaifanhua.com
// CreateTime:2022-12-03 下午 03:56:54
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

using System.Text;

namespace ZhaiFanhuaBlog.Utils.MessagePush.Email;

/// <summary>
/// EmailModel
/// </summary>
public class EmailModel
{
    /// <summary>
    /// 服务
    /// </summary>
    public string Host { get; set; } = string.Empty;

    /// <summary>
    /// 端口
    /// </summary>
    public int Port { get; set; } = 587;

    /// <summary>
    /// 发送者邮箱
    /// </summary>
    public string FromMail { get; set; } = string.Empty;

    /// <summary>
    /// 发送者名称
    /// </summary>
    public string FromName { get; set; } = string.Empty;

    /// <summary>
    /// 发送者邮箱密码
    /// </summary>
    public string FromPassword { get; set; } = string.Empty;

    /// <summary>
    /// 发送主题
    /// </summary>
    public string Subject { get; set; } = string.Empty;

    /// <summary>
    /// 发送内容
    /// </summary>
    public string Body { get; set; } = string.Empty;

    /// <summary>
    /// 内容编码
    /// </summary>
    public Encoding Coding { get; set; } = Encoding.UTF8;

    /// <summary>
    /// 接收者邮箱
    /// </summary>
    public List<string> ToMail { get; set; } = new List<string>();

    /// <summary>
    /// 抄送给邮箱
    /// </summary>
    public List<string> CcMail { get; set; } = new List<string>();

    /// <summary>
    /// 附件
    /// </summary>
    public List<string> AttachmentsPath { get; set; } = new List<string>();
}