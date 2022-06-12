﻿// ----------------------------------------------------------------
// Copyright ©2022 ZhaiFanhua All Rights Reserved.
// FileName:ResultMessage
// Guid:3874a6ae-1ebc-49ab-b4a3-55e76dc68945
// Author:zhaifanhua
// Email:me@zhaifanhua.com
// CreateTime:2022-05-08 下午 11:46:50
// ----------------------------------------------------------------

using ZhaiFanhuaBlog.Utils.Summaries;
using ZhaiFanhuaBlog.ViewModels.Response.Enum;
using ZhaiFanhuaBlog.ViewModels.Response.Model;

namespace ZhaiFanhuaBlog.ViewModels.Response;

/// <summary>
/// 返回信息
/// </summary>
public class ResponseResult
{
    /// <summary>
    /// 返回成功 200
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static MessageModel OK(object data)
    {
        return new MessageModel
        {
            Success = true,
            Code = ResultCode.OK,
            Message = EnumSummaryHelper.GetEnumSummary(ResultCode.OK),
            Data = data,
        };
    }

    /// <summary>
    /// 返回成功 200
    /// </summary>
    /// <param name="pageIndex"></param>
    /// <param name="totalCount"></param>
    /// <param name="dataCount"></param>
    /// <param name="pageSize"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static PageModel OK(int pageIndex, int totalCount, int dataCount, int pageSize, List<object> data)
    {
        return new PageModel
        {
            PageIndex = pageIndex,
            TotalCount = totalCount,
            DataCount = dataCount,
            PageSize = pageSize,
            Data = data,
        };
    }

    /// <summary>
    /// 返回成功 200
    /// </summary>
    /// <param name="dataCount"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static TableModel OK(int dataCount, List<object> data)
    {
        return new TableModel
        {
            Code = ResultCode.OK,
            Message = EnumSummaryHelper.GetEnumSummary(ResultCode.OK),
            DataCount = dataCount,
            Data = data,
        };
    }

    /// <summary>
    /// 返回失败，访问出错 400
    /// </summary>
    /// <returns></returns>
    public static MessageModel BadRequest()
    {
        return new MessageModel
        {
            Success = false,
            Code = ResultCode.BadRequest,
            Message = EnumSummaryHelper.GetEnumSummary(ResultCode.BadRequest),
            Data = null,
        };
    }

    /// <summary>
    /// 返回失败，访问未授权 401
    /// </summary>
    /// <returns></returns>
    public static MessageModel Unauthorized()
    {
        return new MessageModel
        {
            Success = false,
            Code = ResultCode.Unauthorized,
            Message = EnumSummaryHelper.GetEnumSummary(ResultCode.Unauthorized),
            Data = null,
        };
    }

    /// <summary>
    /// 返回失败，内容禁止访问 403
    /// </summary>
    /// <returns></returns>
    public static MessageModel Forbidden()
    {
        return new MessageModel
        {
            Success = false,
            Code = ResultCode.Forbidden,
            Message = EnumSummaryHelper.GetEnumSummary(ResultCode.Forbidden),
            Data = null,
        };
    }

    /// <summary>
    /// 返回失败，数据未找到 404
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static MessageModel NotFound()
    {
        return new MessageModel
        {
            Success = false,
            Code = ResultCode.NotFound,
            Message = EnumSummaryHelper.GetEnumSummary(ResultCode.NotFound),
            Data = null,
        };
    }

    /// <summary>
    /// 返回失败，服务器内部错误 500
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static MessageModel InternalServerError()
    {
        return new MessageModel
        {
            Success = false,
            Code = ResultCode.InternalServerError,
            Message = EnumSummaryHelper.GetEnumSummary(ResultCode.InternalServerError),
            Data = null,
        };
    }

    /// <summary>
    /// 返回失败，功能未实施 501
    /// </summary>
    /// <returns></returns>
    public static MessageModel NotImplemented()
    {
        return new MessageModel
        {
            Success = false,
            Code = ResultCode.NotImplemented,
            Message = EnumSummaryHelper.GetEnumSummary(ResultCode.NotImplemented),
            Data = null,
        };
    }
}