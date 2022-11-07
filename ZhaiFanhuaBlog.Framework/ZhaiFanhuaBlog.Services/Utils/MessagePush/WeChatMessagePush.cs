﻿// ----------------------------------------------------------------
// Copyright ©2022 ZhaiFanhua All Rights Reserved.
// FileName:WeChatMessagePush
// Guid:a273c787-f81d-4c4b-875e-c20d8a04ab45
// Author:zhaifanhua
// Email:me@zhaifanhua.com
// CreateTime:2022-11-08 上午 02:44:27
// ----------------------------------------------------------------

using ZhaiFanhuaBlog.Core.AppSettings;
using ZhaiFanhuaBlog.Utils.Http;
using ZhaiFanhuaBlog.Utils.MessagePush.WeChat;
using ZhaiFanhuaBlog.ViewModels.Bases.Results;
using ZhaiFanhuaBlog.ViewModels.Response;

namespace ZhaiFanhuaBlog.Services.Utils.MessagePush;

/// <summary>
/// WeChatMessagePush
/// </summary>
public class WeChatMessagePush : IWeChatMessagePush
{
    private readonly IHttpHelper _IHttpHelper;
    private readonly WeChatRobot _WeChatRobot;

    /// <summary>
    /// 构造函数
    /// </summary>
    public WeChatMessagePush(IHttpHelper iHttpHelper)
    {
        string webHookUrl = AppSettings.WeChart.WebHookUrl;
        _IHttpHelper = iHttpHelper;
        _WeChatRobot = new WeChatRobot(_IHttpHelper, webHookUrl);
    }

    #region WeChat

    /// <summary>
    /// 微信推送文本消息
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public async Task<BaseResultDto> WeChatToText(Text text)
    {
        ResultInfo? result = await _WeChatRobot.TextMessage(text);
        return WeChatReturn(result);
    }

    /// <summary>
    /// 微信推送文档消息
    /// </summary>
    /// <param name="markdown"></param>
    /// <returns></returns>
    public async Task<BaseResultDto> WeChatToMarkdown(Markdown markdown)
    {
        ResultInfo? result = await _WeChatRobot.MarkdownMessage(markdown);
        return WeChatReturn(result);
    }

    /// <summary>
    /// 微信推送图片消息
    /// </summary>
    /// <param name="image"></param>
    /// <returns></returns>
    public async Task<BaseResultDto> WeChatToImage(Image image)
    {
        ResultInfo? result = await _WeChatRobot.ImageMessage(image);
        return WeChatReturn(result);
    }

    ///// <summary>
    ///// 微信推送卡片菜单消息
    ///// </summary>
    ///// <param name="feedCard"></param>
    ///// <returns></returns>
    //public async Task<BaseResultDto> WeChatToFeedCard(FeedCard feedCard)
    //{
    //    ResultInfo? result = await _WeChatRobot.FeedCardMessage(feedCard);
    //    return WeChatReturn(result);
    //}

    /// <summary>
    /// 统一格式返回
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    private static BaseResultDto WeChatReturn(ResultInfo? result)
    {
        if (result != null)
        {
            if (result.ErrCode == "0" || result?.ErrMsg == "ok")
                return BaseResponseDto.OK("发送成功");
            else
                return BaseResponseDto.BadRequest(result?.ErrMsg ?? "发送失败");
        }
        return BaseResponseDto.InternalServerError();
    }

    #endregion WeChat
}