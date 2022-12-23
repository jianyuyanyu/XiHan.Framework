﻿#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2022 ZhaiFanhua All Rights Reserved.
// FileName:EnumDescriptionHelper
// Guid:23f4fdd1-650e-49f7-bdc6-7ba00110a2ac
// Author:zhaifanhua
// Email:me@zhaifanhua.com
// CreateTime:2022-05-09 上午 12:55:52
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

using System.ComponentModel;
using System.Reflection;

namespace ZhaiFanhuaBlog.Utils.Summary;

/// <summary>
/// 枚举描述帮助类
/// </summary>
public static class EnumDescriptionHelper
{
    /// <summary>
    /// 获取枚举描述信息
    /// </summary>
    /// <param name="enumObj"></param>
    /// <returns></returns>
    public static string GetEnumDescription(this Enum enumObj)
    {
        var enumName = enumObj.ToString();
        var type = enumObj.GetType();
        var field = type.GetField(enumName);
        if (field == null) return string.Empty;
        if (field.GetCustomAttributes(typeof(DescriptionAttribute), false) is DescriptionAttribute[] description)
            return description[0].Description;
        return string.Empty;
    }
}