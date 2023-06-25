﻿#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2022 ZhaiFanhua All Rights Reserved.
// FileName:FormatTimeExtensions
// Guid:4598f6e0-78b7-46d3-9eb5-834f6699d7c6
// Author:zhaifanhua
// Email:me@zhaifanhua.com
// CreateTime:2022-05-08 下午 03:36:28
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

namespace XiHan.Utils.Extensions;

/// <summary>
/// 时间格式化拓展类
/// </summary>
public static class FormatTimeExtension
{
    /// <summary>
    /// 获取当前时间的时间戳
    /// </summary>
    /// <param name="thisValue"></param>
    /// <returns></returns>
    public static string GetDateToTimeStamp(this DateTime thisValue)
    {
        var ts = thisValue - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return Convert.ToInt64(ts.TotalSeconds).ToString();
    }

    /// <summary>
    /// 时间转换字符串
    /// </summary>
    /// <param name="dateTimeBefore"></param>
    /// <param name="dateTimeAfter"></param>
    /// <returns></returns>
    public static string FormatDateTimeToString(this DateTime dateTimeBefore, DateTime dateTimeAfter)
    {
        if (dateTimeBefore < dateTimeAfter)
        {
            var timeSpan = dateTimeAfter - dateTimeBefore;
            return timeSpan.FormatTimeSpanToString();
        }

        throw new Exception("开始日期必须小于结束日期");
    }

    /// <summary>
    /// 毫秒转换字符串
    /// </summary>
    /// <param name="milliseconds"></param>
    /// <returns></returns>
    public static string FormatMilliSecondsToString(this long milliseconds)
    {
        var timeSpan = TimeSpan.FromMilliseconds(milliseconds);
        return timeSpan.FormatTimeSpanToString();
    }

    /// <summary>
    /// 时刻转换字符串
    /// </summary>
    /// <param name="ticks"></param>
    /// <returns></returns>
    public static string FormatTimeTicksToString(this long ticks)
    {
        var timeSpan = TimeSpan.FromTicks(ticks);
        return timeSpan.FormatTimeSpanToString();
    }

    /// <summary>
    /// 时间跨度转换字符串
    /// </summary>
    /// <param name="timeSpan"></param>
    /// <returns></returns>
    public static string FormatTimeSpanToString(this TimeSpan timeSpan)
    {
        var result = string.Empty;
        if (timeSpan.Days >= 1)
        {
            result = timeSpan.Days + "天";
        }
        if (timeSpan.Hours >= 1)
        {
            result += timeSpan.Hours + "时";
        }
        if (timeSpan.Minutes >= 1)
        {
            result += timeSpan.Minutes + "分";
        }
        if (timeSpan.Seconds >= 1)
        {
            result += timeSpan.Seconds + "秒";
        }
        if (timeSpan.Milliseconds >= 1)
        {
            result += timeSpan.Milliseconds + "毫秒";
        }
        return result;
    }

    /// <summary>
    /// 时间按天转换字符串
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string FormatDateTimeToEasyString(this DateTime value)
    {
        DateTime now = DateTime.Now;
        var strDate = value.ToString("yyyy-MM-dd");
        if (now < value) return strDate;
        TimeSpan dep = now - value;

        if (dep.TotalMinutes < 1)
        {
            return "刚刚";
        }
        else if (dep.TotalMinutes >= 1 && dep.TotalMinutes < 60)
        {
            return dep.TotalMinutes.ParseToInt() + "分钟前";
        }
        else if (dep.TotalHours < 24)
        {
            return dep.TotalHours.ParseToInt() + "小时前";
        }
        else if (dep.TotalDays < 7)
        {
            return dep.TotalDays.ParseToInt() + "天前";
        }
        else if (dep.TotalDays >= 7 && dep.TotalDays < 30)
        {
            int defautlWeek = dep.TotalDays.ParseToInt() / 7;
            return defautlWeek + "周前";
        }
        else if (dep.TotalDays.ParseToInt() >= 30 && dep.TotalDays.ParseToInt() < 365)
        {
            return value.Month.ParseToInt() + "个月前";
        }
        else return now.Year - value.Year + "年前";
    }

    /// <summary>
    /// 字符串转日期
    /// </summary>
    /// <param name="thisValue"></param>
    /// <returns></returns>
    public static DateTime FormatStringToDate(this string thisValue)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(thisValue))
            {
                return DateTime.MinValue;
            }
            if (thisValue.Contains("-") || thisValue.Contains("/"))
            {
                return DateTime.Parse(thisValue);
            }
            else
            {
                int length = thisValue.Length;
                return length switch
                {
                    4 => DateTime.ParseExact(thisValue, "yyyy", System.Globalization.CultureInfo.CurrentCulture),
                    6 => DateTime.ParseExact(thisValue, "yyyyMM", System.Globalization.CultureInfo.CurrentCulture),
                    8 => DateTime.ParseExact(thisValue, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture),
                    10 => DateTime.ParseExact(thisValue, "yyyyMMddHH", System.Globalization.CultureInfo.CurrentCulture),
                    12 => DateTime.ParseExact(thisValue, "yyyyMMddHHmm", System.Globalization.CultureInfo.CurrentCulture),
                    14 => DateTime.ParseExact(thisValue, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture),
                    _ => DateTime.ParseExact(thisValue, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture),
                };
            }
        }
        catch
        {
            return DateTime.MinValue;
        }
    }
}