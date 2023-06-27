﻿#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2023 ZhaiFanhua All Rights Reserved.
// FileName:HttpContextExtension
// Guid:61d55324-ab83-4df1-a500-e076d5b6cd89
// Author:zhaifanhua
// Email:me@zhaifanhua.com
// CreateTime:2023-06-13 下午 09:01:53
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

using Microsoft.AspNetCore.Http;
using System.Net;
using System.Security.Claims;
using UAParser;
using XiHan.Infrastructures.Infos;
using XiHan.Infrastructures.Infos.IpLocation;
using XiHan.Utils.Extensions;
using XiHan.Utils.Verifications;

namespace XiHan.Infrastructures.Apps.HttpContexts;

/// <summary>
/// 请求上下文拓展
/// </summary>
public static class HttpContextExtend
{
    #region 客户端信息

    /// <summary>
    /// 获取客户端信息
    /// </summary>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static UserClientInfo GetClientInfo(this HttpContext httpContext)
    {
        if (httpContext == null)
        {
            throw new ArgumentNullException(nameof(httpContext));
        }

        var clientModel = new UserClientInfo
        {
            RemoteIPv4 = httpContext.GetClientIpV4(),
            RemoteIPv6 = httpContext.GetClientIpV6(),
            RequestUrl = httpContext.GetRequestUrl(),
            QueryString = httpContext.GetQueryString()
        };

        var header = httpContext.Request.HttpContext.Request.Headers;

        if (header.TryGetValue("Accept-Language", out var value))
        {
            clientModel.Language = value.ToString().Split(';')[0];
        }
        if (header.TryGetValue("Referer", out var value1))
        {
            clientModel.Referer = value1.ToString();
        }
        if (!header.TryGetValue("User-Agent", out var value2)) return clientModel;
        var agent = value2.ToString();
        var clientInfo = Parser.GetDefault().Parse(agent);
        clientModel.Agent = agent;
        clientModel.OsName = clientInfo.OS.Family;
        if (!string.IsNullOrWhiteSpace(clientInfo.OS.Major))
        {
            clientModel.OsVersion = clientInfo.OS.Major;
            if (!string.IsNullOrWhiteSpace(clientInfo.OS.Minor))
            {
                clientModel.OsVersion += "." + clientInfo.OS.Minor;
            }
        }
        clientModel.UaName = clientInfo.UA.Family;
        if (string.IsNullOrWhiteSpace(clientInfo.UA.Major)) return clientModel;
        clientModel.UaVersion = clientInfo.UA.Major;
        if (!string.IsNullOrWhiteSpace(clientInfo.UA.Minor))
        {
            clientModel.UaVersion += "." + clientInfo.UA.Minor;
        }
        return clientModel;
    }

    /// <summary>
    /// 是否是 ajax 请求
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static bool IsAjaxRequest(this HttpContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }
        var request = context.Request;
        return request.Headers["X-Requested-With"] == "XMLHttpRequest" || request.Headers["X-Requested-With"] == "XMLHttpRequest";
    }

    /// <summary>
    /// 判断是否IP
    /// </summary>
    /// <param name="ip"></param>
    /// <returns></returns>
    public static bool IsIp(string ip)
    {
        return RegexHelper.IsIpRegex(ip);
    }

    /// <summary>
    /// 取得客户端 IP4
    /// </summary>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    public static string GetClientIpV4(this HttpContext httpContext)
    {
        return httpContext.GetClientIpAddressInfo().MapToIPv4().ToString();
    }

    /// <summary>
    /// 取得客户端 IP6
    /// </summary>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    public static string GetClientIpV6(this HttpContext httpContext)
    {
        return httpContext.GetClientIpAddressInfo().MapToIPv6().ToString();
    }

    /// <summary>
    /// 取得客户端 IP
    /// </summary>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IPAddress GetClientIpAddressInfo(this HttpContext httpContext)
    {
        if (httpContext == null)
        {
            throw new ArgumentNullException(nameof(httpContext));
        }

        var result = "0.0.0.0";
        var request = httpContext.Request;
        var context = request.HttpContext;
        var header = request.Headers;

        if (context.Connection.RemoteIpAddress != null)
        {
            result = context.Connection.RemoteIpAddress.ToString();
        }
        else
        {
            // 取代理 IP
            if (header.ContainsKey("X-Real-IP") | header.ContainsKey("X-Forwarded-For"))
            {
                result = header["X-Real-IP"].FirstOrDefault() ?? header["X-Forwarded-For"].FirstOrDefault();
            }
        }
        if (string.IsNullOrEmpty(result))
        {
            result = "0.0.0.0";
        }
        return result.FormatStringToIpAddress();
    }

    /// <summary>
    /// 获取请求Url
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static string? GetRequestUrl(this HttpContext? context)
    {
        return context != null ? context.Request.Path.Value : "";
    }

    /// <summary>
    /// 获取请求参数
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static string? GetQueryString(this HttpContext? context)
    {
        return context != null ? context.Request.QueryString.Value : "";
    }

    #endregion

    #region 地址信息

    /// <summary>
    /// 获取地址信息
    /// </summary>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static UserAddressInfo GetAddressInfo(this HttpContext httpContext)
    {
        var addressInfo = new UserAddressInfo();
        var addressInfoResult = IpSearchHelper.Search(httpContext.GetClientIpV4());
        if (addressInfoResult != null)
        {
            addressInfo = addressInfoResult;
        }
        return addressInfo;
    }

    #endregion

    #region 权限信息

    /// <summary>
    /// 获取登录用户权限信息
    /// </summary>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    public static UserAuthInfo GetUserAuthInfo(this HttpContext httpContext)
    {
        var userAuthInfo = new UserAuthInfo
        {
            UserId = httpContext.GetUserId(),
            UserName = httpContext.GetUserName(),
            UserRole = httpContext.GetUserRole(),
            UserToken = httpContext.GetUserToken(),
            IsAdmin = httpContext.IsAdmin(),
            Claims = httpContext.GetClaims()
        };
        return userAuthInfo;
    }

    /// <summary>
    /// 获取登录用户 Id
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static long? GetUserId(this HttpContext context)
    {
        var uid = context.User.FindFirstValue(ClaimTypes.PrimarySid);
        return uid.ParseToLong();
    }

    /// <summary>
    /// 获取登录用户名
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static string? GetUserName(this HttpContext context)
    {
        var uname = context.User.Identity?.Name;
        return uname;
    }

    /// <summary>
    /// 获取登录用户权限
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static string GetUserRole(this HttpContext context)
    {
        var roleId = context.User.FindFirstValue(ClaimTypes.Role);
        return roleId.ParseToString();
    }

    /// <summary>
    /// 获取请求令牌
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static string GetUserToken(this HttpContext context)
    {
        return context.Request.Headers["Authorization"].ToString();
    }

    /// <summary>
    /// 判断是否是管理员
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static bool IsAdmin(this HttpContext context)
    {
        var userName = context.GetUserName();
        return userName == AppGlobalConstant.AdminRole;
    }

    /// <summary>
    /// ClaimsIdentity
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static IEnumerable<ClaimsIdentity> GetClaims(this HttpContext context)
    {
        return context.User.Identities;
    }

    #endregion
}