﻿#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2023 ZhaiFanhua All Rights Reserved.
// Licensed under the MulanPSL2 License. See LICENSE in the project root for license information.
// FileName:ISubscriptionsManager
// Guid:35ab7506-8eb2-4717-bb95-53381de47626
// Author:zhaifanhua
// Email:me@zhaifanhua.com
// CreateTime:2023/12/31 4:10:26
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

using XiHan.EventBus.Bases.Handlers;
using XiHan.EventBus.Bases.Models;

namespace XiHan.EventBus.Bases;

/// <summary>
/// 订阅管理器接口
/// </summary>
/// <remarks>
/// 提供事件处理的注册和分发，从而使得相同领域事件可被微服务内多个事件处理器接收
/// </remarks>
public interface ISubscriptionsManager
{
    /// <summary>
    /// 事件移除触发
    /// </summary>
    event EventHandler<string> OnEventRemoved;

    /// <summary>
    /// 是否为空
    /// </summary>
    bool IsEmpty { get; }

    /// <summary>
    /// 添加事件T的订阅
    /// </summary>
    /// <typeparam name="T">事件模型</typeparam>
    /// <typeparam name="TH">事件处理器(事件模型)</typeparam>
    void AddSubscription<T, TH>()
        where T : IntegrationEventModel
        where TH : IIntegrationEventHandler<T>;

    /// <summary>
    /// 移除事件T的订阅
    /// </summary>
    /// <typeparam name="T">事件模型</typeparam>
    /// <typeparam name="TH">事件处理器(事件模型)</typeparam>
    void RemoveSubscription<T, TH>()
        where TH : IIntegrationEventHandler<T>
        where T : IntegrationEventModel;

    /// <summary>
    /// 添加事件eventName的动态订阅
    /// </summary>
    /// <typeparam name="TH">事件处理器</typeparam>
    /// <param name="eventName">事件名称</param>
    void AddDynamicSubscription<TH>(string eventName)
        where TH : IDynamicIntegrationEventHandler;

    /// <summary>
    /// 移除事件eventName的动态订阅
    /// </summary>
    /// <typeparam name="TH">事件处理器</typeparam>
    /// <param name="eventName">事件名称</param>
    void RemoveDynamicSubscription<TH>(string eventName)
        where TH : IDynamicIntegrationEventHandler;

    /// <summary>
    /// 是否有类型监听T这个事件
    /// </summary>
    /// <typeparam name="T">事件模型</typeparam>
    /// <returns></returns>
    bool HasSubscriptionsForEvent<T>()
        where T : IntegrationEventModel;

    /// <summary>
    /// 是否有类型监听eventName这个事件
    /// </summary>
    /// <param name="eventName">事件名称</param>
    /// <returns></returns>
    bool HasSubscriptionsForEvent(string eventName);

    /// <summary>
    /// 获取事件模型
    /// </summary>
    /// <param name="eventName">事件名称</param>
    /// <returns></returns>
    Type GetEventTypeByName(string eventName);

    /// <summary>
    /// 获取事件T的所有事件监听器
    /// </summary>
    /// <typeparam name="T">订阅模型</typeparam>
    /// <returns></returns>
    IEnumerable<SubscriptionModel> GetHandlersForEvent<T>()
        where T : IntegrationEventModel;

    /// <summary>
    /// 获取事件eventName的所有事件监听器
    /// </summary>
    /// <param name="eventName">事件名称</param>
    /// <returns></returns>
    IEnumerable<SubscriptionModel> GetHandlersForEvent(string eventName);

    /// <summary>
    /// 获取事件T的主键标识
    /// </summary>
    /// <typeparam name="T">事件模型</typeparam>
    /// <returns></returns>
    string GetEventBaseId<T>();

    /// <summary>
    /// 清空
    /// </summary>
    void Clear();
}