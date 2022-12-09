﻿#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2022 ZhaiFanhua All Rights Reserved.
// FileName:WeChatMessagePushService
// Guid:a273c787-f81d-4c4b-875e-c20d8a04ab45
// Author:zhaifanhua
// Email:me@zhaifanhua.com
// CreateTime:2022-11-08 上午 02:44:27
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

using ZhaiFanhuaBlog.Extensions.Bases.Response.Results;
using ZhaiFanhuaBlog.Extensions.Response;
using ZhaiFanhuaBlog.Infrastructure.AppService;
using ZhaiFanhuaBlog.Infrastructure.AppSetting;
using ZhaiFanhuaBlog.Utils.Http;
using ZhaiFanhuaBlog.Utils.Message.WeChat;
using File = ZhaiFanhuaBlog.Utils.Message.WeChat.File;

namespace ZhaiFanhuaBlog.Services.Utils.Message;

/// <summary>
/// WeChatMessagePushService
/// </summary>
[AppService(ServiceType = typeof(IWeChatPushService), ServiceLifetime = LifeTime.Scoped)]
public class WeChatPushService : IWeChatPushService
{
    /// <summary>
    /// 机器人实例
    /// </summary>
    private readonly WeChatRobotHelper _WeChatRobot;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="iHttpHelper"></param>
    public WeChatPushService(IHttpHelper iHttpHelper)
    {
        WeChatConnection conn = new()
        {
            WebHookUrl = AppSettings.Message.WeChart.WebHookUrl,
            UploadkUrl = AppSettings.Message.WeChart.UploadkUrl,
            Key = AppSettings.Message.WeChart.Key
        };
        _WeChatRobot = new WeChatRobotHelper(iHttpHelper, conn);
    }

    /// <summary>
    /// 微信推送文本消息
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public async Task<BaseResultDto> WeChatToText(Text text)
    {
        WeChatResultInfo? result = await _WeChatRobot.TextMessage(text);
        return WeChatMessageReturn(result);
    }

    /// <summary>
    /// 微信推送文档消息
    /// </summary>
    /// <param name="markdown"></param>
    /// <returns></returns>
    public async Task<BaseResultDto> WeChatToMarkdown(Markdown markdown)
    {
        WeChatResultInfo? result = await _WeChatRobot.MarkdownMessage(markdown);
        return WeChatMessageReturn(result);
    }

    /// <summary>
    /// 微信推送图片消息
    /// </summary>
    /// <param name="image"></param>
    /// <returns></returns>
    public async Task<BaseResultDto> WeChatToImage(Image image)
    {
        WeChatResultInfo? result = await _WeChatRobot.ImageMessage(image);
        return WeChatMessageReturn(result);
    }

    /// <summary>
    /// 微信推送图文消息
    /// </summary>
    /// <param name="news">图文</param>
    /// <returns></returns>
    public async Task<BaseResultDto> WeChatToNews(News news)
    {
        WeChatResultInfo? result = await _WeChatRobot.NewsMessage(news);
        return WeChatMessageReturn(result);
    }

    /// <summary>
    /// 微信推送文件消息
    /// </summary>
    /// <param name="file">文件</param>
    /// <returns></returns>
    public async Task<BaseResultDto> WeChatToFile(File file)
    {
        WeChatResultInfo? result = await _WeChatRobot.FileMessage(file);
        return WeChatMessageReturn(result);
    }

    /// <summary>
    /// 微信推送文本通知消息
    /// </summary>
    /// <param name="templateCard">文本通知-模版卡片</param>
    /// <returns></returns>
    public async Task<BaseResultDto> WeChatToTextNotice(TemplateCardTextNotice templateCard)
    {
        templateCard.CardType = TemplateCardType.text_notice.ToString();
        WeChatResultInfo? result = await _WeChatRobot.TextNoticeMessage(templateCard);
        return WeChatMessageReturn(result);
    }

    /// <summary>
    /// 微信推送图文展示消息
    /// </summary>
    /// <param name="templateCard">图文展示-模版卡片</param>
    /// <returns></returns>
    public async Task<BaseResultDto> WeChatToNewsNotice(TemplateCardNewsNotice templateCard)
    {
        templateCard.CardType = TemplateCardType.news_notice.ToString();
        WeChatResultInfo? result = await _WeChatRobot.NewsNoticeMessage(templateCard);
        return WeChatMessageReturn(result);
    }

    /// <summary>
    /// 微信上传文件
    /// </summary>
    /// <param name="fileStream">文件</param>
    /// <returns></returns>
    public async Task<BaseResultDto> WeChatToUploadkFile(FileStream fileStream)
    {
        WeChatResultInfo? result = await _WeChatRobot.UploadkFile(fileStream);
        return WeChatUploadReturn(result);
    }

    /// <summary>
    /// 消息统一格式返回
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    private static BaseResultDto WeChatMessageReturn(WeChatResultInfo? result)
    {
        if (result != null)
        {
            if (result.ErrCode == 0 || result?.ErrMsg == "ok")
                return BaseResponseDto.OK("发送成功");
            else
                return BaseResponseDto.BadRequest(result?.ErrMsg ?? "发送失败");
        }
        return BaseResponseDto.InternalServerError();
    }

    /// <summary>
    /// 上传文件统一格式返回
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    private static BaseResultDto WeChatUploadReturn(WeChatResultInfo? result)
    {
        if (result != null)
        {
            if (result.ErrCode == 0 || result?.ErrMsg == "ok")
            {
                WeChatUploadResult uploadResult = new("上传成功", result.MediaId);
                return BaseResponseDto.OK(uploadResult);
            }
            else
                return BaseResponseDto.BadRequest(result?.ErrMsg ?? "上传失败");
        }
        return BaseResponseDto.InternalServerError();
    }
}