﻿// ----------------------------------------------------------------
// Copyright ©2022 ZhaiFanhua All Rights Reserved.
// FileName:IRootRoleAuthorityService
// Guid:619a9c65-08b5-b2c7-0e17-57a30f09e61d
// Author:zhaifanhua
// Email:me@zhaifanhua.com
// CreateTime:2022-01-06 下午 10:37:03
// ----------------------------------------------------------------

using ZhaiFanhuaBlog.IServices.Bases;
using ZhaiFanhuaBlog.Models.Roots;

namespace ZhaiFanhuaBlog.IServices.Roots;

public interface IRootRoleAuthorityService : IBaseService<RootRoleAuthority>
{
    Task<RootRoleAuthority> IsExistenceAsync(Guid guid);

    Task<bool> InitRootRoleAuthorityAsync(List<RootRoleAuthority> rootRoleAuthorities);

    Task<bool> CreateRootRoleAuthorityAsync(RootRoleAuthority userRole);

    Task<bool> DeleteRootRoleAuthorityAsync(Guid guid, Guid deleteId);

    Task<RootRoleAuthority> ModifyRootRoleAuthorityAsync(RootRoleAuthority userRole);

    Task<RootRoleAuthority> FindRootRoleAuthorityAsync(Guid guid);

    Task<List<RootRoleAuthority>> QueryRootRoleAuthorityAsync();
}