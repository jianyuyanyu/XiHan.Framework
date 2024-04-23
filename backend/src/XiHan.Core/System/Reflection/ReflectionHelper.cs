﻿#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2023 ZhaiFanhua All Rights Reserved.
// Licensed under the MulanPSL2 License. See LICENSE in the project root for license information.
// FileName:ReflectionHelper
// Guid:e8f234f6-9d3e-4dbc-aee6-d02fbf424954
// Author:zhaifanhua
// Email:me@zhaifanhua.com
// CreatedTime:2023-04-11 下午 09:28:46
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Runtime.Loader;
using XiHan.Core.Verification;

namespace XiHan.Core.System.Reflection;

/// <summary>
/// 反射帮助类
/// </summary>
public static class ReflectionHelper
{
    #region 程序集

    /// <summary>
    /// 获取所有程序集
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<Assembly> GetAllAssemblies()
    {
        return AssemblyLoadContext.Default.Assemblies;
    }

    /// <summary>
    /// 获取所有引用的程序集
    /// </summary>
    /// <param name="skipSystemAssemblies"></param>
    /// <returns></returns>
    public static IEnumerable<Assembly> GetAllReferencedAssemblies(bool skipSystemAssemblies = true)
    {
        var rootAssembly = Assembly.GetEntryAssembly();
        rootAssembly ??= Assembly.GetCallingAssembly();

        var returnAssemblies = new HashSet<Assembly>(new AssemblyEquality());
        var loadedAssemblies = new HashSet<string>();
        var assembliesToCheck = new Queue<Assembly>();
        assembliesToCheck.Enqueue(rootAssembly);

        if (skipSystemAssemblies && IsSystemAssembly(rootAssembly))
        {
            if (IsValid(rootAssembly))
            {
                returnAssemblies.Add(rootAssembly);
            }
        }
        while (assembliesToCheck.Count != 0)
        {
            var assemblyToCheck = assembliesToCheck.Dequeue();
            foreach (var reference in assemblyToCheck.GetReferencedAssemblies())
            {
                if (!loadedAssemblies.Contains(reference.FullName))
                {
                    var assembly = Assembly.Load(reference);
                    if (skipSystemAssemblies && IsSystemAssembly(assembly))
                    {
                        continue;
                    }
                    assembliesToCheck.Enqueue(assembly);
                    loadedAssemblies.Add(reference.FullName);
                    if (IsValid(assembly))
                    {
                        returnAssemblies.Add(assembly);
                    }
                }
            }
        }
        var asmsInBaseDir = Directory.EnumerateFiles(AppContext.BaseDirectory, "*.dll", new EnumerationOptions { RecurseSubdirectories = true });
        foreach (var assemblyPath in asmsInBaseDir)
        {
            if (!IsManagedAssembly(assemblyPath))
            {
                continue;
            }
            AssemblyName asmName = AssemblyName.GetAssemblyName(assemblyPath);

            // 如果程序集已经加载过了就不再加载
            if (returnAssemblies.Any(x => AssemblyName.ReferenceMatchesDefinition(x.GetName(), asmName)))
            {
                continue;
            }
            if (skipSystemAssemblies && IsSystemAssembly(assemblyPath))
            {
                continue;
            }
            Assembly? asm = TryLoadAssembly(assemblyPath);
            if (asm == null)
            {
                continue;
            }
            if (!IsValid(asm))
            {
                continue;
            }
            if (skipSystemAssemblies && IsSystemAssembly(asm))
            {
                continue;
            }
            returnAssemblies.Add(asm);
        }
        return [.. returnAssemblies];
    }

    /// <summary>
    /// 获取符合条件名称的程序集
    /// </summary>
    /// <param name="prefix">前缀名</param>
    /// <param name="suffix">后缀名</param>
    /// <param name="contain">包含名</param>
    /// <returns></returns>
    public static IEnumerable<Assembly> GetEffectiveAssemblies([NotNull] string prefix, [NotNull] string suffix, [NotNull] string contain)
    {
        CheckHelper.NotNullOrEmpty(prefix, nameof(prefix));
        CheckHelper.NotNullOrEmpty(suffix, nameof(suffix));
        CheckHelper.NotNullOrEmpty(contain, nameof(contain));

        return GetAllAssemblies()
            .Where(assembly => assembly.ManifestModule.Name.EndsWith(suffix, StringComparison.InvariantCultureIgnoreCase))
            .Where(assembly => assembly.ManifestModule.Name.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
            .Where(assembly => assembly.ManifestModule.Name.Contains(contain, StringComparison.InvariantCultureIgnoreCase))
            .Distinct();
    }

    /// <summary>
    /// 获取符合条件前后缀名称的程序集
    /// </summary>
    /// <param name="prefix">前缀名</param>
    /// <param name="suffix">后缀名</param>
    /// <returns></returns>
    public static IEnumerable<Assembly> GetEffectivePatchAssemblies([NotNull] string prefix, [NotNull] string suffix)
    {
        CheckHelper.NotNullOrEmpty(prefix, nameof(prefix));
        CheckHelper.NotNullOrEmpty(suffix, nameof(suffix));

        return GetAllAssemblies()
            .Where(assembly => assembly.ManifestModule.Name.EndsWith(suffix, StringComparison.InvariantCultureIgnoreCase))
            .Where(assembly => assembly.ManifestModule.Name.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
            .Distinct();
    }

    /// <summary>
    /// 获取符合条件包含名称的程序集
    /// </summary>
    /// <param name="contain">包含名</param>
    /// <returns></returns>
    public static IEnumerable<Assembly> GetEffectiveCenterAssemblies([NotNull] string contain)
    {
        CheckHelper.NotNullOrEmpty(contain, nameof(contain));

        return GetAllAssemblies()
            .Where(assembly => assembly.ManifestModule.Name.Contains(contain, StringComparison.InvariantCultureIgnoreCase))
            .Distinct();
    }

    /// <summary>
    /// 获取曦寒程序集
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<Assembly> GetXiHanAssemblies()
    {
        return GetEffectivePatchAssemblies("XiHan", "dll");
    }

    /// <summary>
    /// 获取应用程序集
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<Assembly> GetApplicationAssemblies()
    {
        return GetEffectiveCenterAssemblies("Application");
    }

    /// <summary>
    /// 获取曦寒应用程序集
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<Assembly> GetXiHanApplicationAssemblies()
    {
        return GetEffectiveAssemblies("XiHan", "dll", "Application");
    }

    #endregion

    #region 程序集类

    /// <summary>
    /// 获取所有程序集类
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<Type> GetAllTypes()
    {
        return GetAllAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Distinct();
    }

    /// <summary>
    /// 获取曦寒程序集类
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<Type> GetXiHanTypes()
    {
        return GetXiHanAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Distinct();
    }

    /// <summary>
    /// 获取应用程序集类
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<Type> GetApplicationTypes()
    {
        return GetApplicationAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Distinct();
    }

    /// <summary>
    /// 获取应用程序集类
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<Type> GetXiHanApplicationTypes()
    {
        return GetXiHanApplicationAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Distinct();
    }

    #endregion

    #region 获取包含有某属性的类

    /// <summary>
    /// 获取包含有某属性的类
    /// 第一种实现
    /// </summary>
    /// <typeparam name="TAttribute"></typeparam>
    /// <returns></returns>
    public static IEnumerable<Type> GetContainsAttributeTypes<TAttribute>()
        where TAttribute : Attribute
    {
        return GetAllTypes()
            .Where(e => e.CustomAttributes.Any(g => g.AttributeType == typeof(TAttribute)));
    }

    /// <summary>
    /// 获取包含有某属性的类
    /// 第二种实现
    /// </summary>
    /// <param name="attribute"></param>
    /// <returns></returns>
    public static IEnumerable<Type> GetContainsAttributeTypes(Attribute attribute)
    {
        return GetAllTypes()
            .Where(e => e.CustomAttributes.Any(g => g.AttributeType == attribute.GetType()));
    }

    #endregion

    #region 获取不包含有某属性的类

    /// <summary>
    /// 获取不包含有某属性的类
    /// 第一种实现
    /// </summary>
    /// <typeparam name="TAttribute"></typeparam>
    /// <returns></returns>
    public static IEnumerable<Type> GetFilterAttributeTypes<TAttribute>()
        where TAttribute : Attribute
    {
        return GetAllTypes()
            .Where(e => e.CustomAttributes.All(g => g.AttributeType != typeof(TAttribute)));
    }

    /// <summary>
    /// 获取包含有某属性的类
    /// 第二种实现
    /// </summary>
    /// <param name="attribute"></param>
    /// <returns></returns>
    public static IEnumerable<Type> GetFilterAttributeTypes(Attribute attribute)
    {
        return GetAllTypes()
            .Where(e => e.CustomAttributes.All(g => g.AttributeType != attribute.GetType()));
    }

    #endregion

    #region 获取某类的子类(非抽象类)

    /// <summary>
    /// 获取某类的子类(非抽象类)
    /// 第一种实现
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IEnumerable<Type> GetSubClasses<T>()
        where T : class
    {
        return GetAllTypes()
            .Where(t => t is { IsInterface: false, IsClass: true, IsAbstract: false })
            .Where(t => typeof(T).IsAssignableFrom(t));
    }

    /// <summary>
    /// 获取某类的子类(非抽象类)
    /// 第二种实现
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static IEnumerable<Type> GetSubClasses(Type type)
    {
        return GetAllTypes()
            .Where(t => t is { IsInterface: false, IsClass: true, IsAbstract: false })
            .Where(type.IsAssignableFrom);
    }

    /// <summary>
    /// 获取某泛型接口的子类(非抽象类)
    /// </summary>
    /// <param name="interfaceType"></param>
    /// <returns></returns>
    public static IEnumerable<Type> GetSubClassesByGenericInterface(Type interfaceType)
    {
        return GetAllTypes()
            .Where(type => type is { IsInterface: false, IsClass: true, IsAbstract: false }
                            && type.GetInterfaces().Any(i => i.IsGenericType
                            && i.GetGenericTypeDefinition() == interfaceType))
            .ToList();
    }

    #endregion

    #region 获取继承自某类的包含有某属性的接口、类的子类(非抽象类)

    /// <summary>
    /// 获取继承自某类的包含有某属性的接口、类的子类(非抽象类)
    /// 第一种实现
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TAttribute"></typeparam>
    /// <returns></returns>
    public static IEnumerable<Type> GetContainsAttributeSubClasses<T, TAttribute>()
        where T : class
        where TAttribute : Attribute
    {
        return GetSubClasses<T>().Intersect(GetContainsAttributeTypes<TAttribute>());
    }

    /// <summary>
    /// 获取继承自某类的包含有某属性的接口、类的子类(非抽象类)
    /// 第二种实现
    /// </summary>
    /// <typeparam name="TAttribute"></typeparam>
    /// <param name="type"></param>
    /// <returns></returns>
    public static IEnumerable<Type> GetContainsAttributeSubClasses<TAttribute>(Type type)
        where TAttribute : Attribute
    {
        return GetSubClasses(type).Intersect(GetContainsAttributeTypes<TAttribute>());
    }

    #endregion

    #region 获取继承自某类的不包含有某属性的接口、类的子类(非抽象类)

    /// <summary>
    /// 获取继承自某类的不包含有某属性的接口、类的子类(非抽象类)
    /// 第一种实现
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TAttribute"></typeparam>
    /// <returns></returns>
    public static IEnumerable<Type> GetFilterAttributeSubClass<T, TAttribute>()
        where T : class
        where TAttribute : Attribute
    {
        return GetSubClasses<T>().Intersect(GetFilterAttributeTypes<TAttribute>());
    }

    /// <summary>
    /// 获取继承自某类的不包含有某属性的接口、类的子类(非抽象类)
    /// 第二种实现
    /// </summary>
    /// <typeparam name="TAttribute"></typeparam>
    /// <param name="type"></param>
    /// <returns></returns>
    public static IEnumerable<Type> GetFilterAttributeSubClass<TAttribute>(Type type)
        where TAttribute : Attribute
    {
        return GetSubClasses(type).Intersect(GetFilterAttributeTypes<TAttribute>());
    }

    #endregion

    #region 程序集依赖包

    /// <summary>
    /// 获取程序集依赖包
    /// </summary>
    /// <returns></returns>
    public static List<NuGetPackage> GetNuGetPackages()
    {
        List<NuGetPackage> nugetPackages = [];

        // 获取当前应用所有程序集
        var assemblies = GetXiHanAssemblies();

        // 查找被引用程序集中的 NuGet 库依赖项
        foreach (var assembly in assemblies)
        {
            var referencedAssemblies = assembly.GetReferencedAssemblies()
                .Where(s => !s.FullName.StartsWith("Microsoft") && !s.FullName.StartsWith("System"))
                .Where(s => !s.FullName.StartsWith("XiHan"));
            foreach (var referencedAssembly in referencedAssemblies)
                // 检查引用的程序集是否来自 NuGet
                if (referencedAssembly.FullName.Contains("Version="))
                {
                    var nuGetPackage = new NuGetPackage
                    {
                        // 获取 NuGet 包的名称和版本号
                        PackageName = referencedAssembly.Name!,
                        PackageVersion = new AssemblyName(referencedAssembly.FullName).Version!.ToString()
                    };

                    // 避免重复添加相同的 NuGet 包标识
                    if (!nugetPackages.Contains(nuGetPackage))
                        nugetPackages.Add(nuGetPackage);
                }
        }
        return nugetPackages;
    }

    #endregion

    #region 私有方法

    /// <summary>
    /// 是否是微软等的官方 Assembly
    /// </summary>
    /// <param name="assembly"></param>
    /// <returns></returns>
    private static bool IsSystemAssembly(Assembly assembly)
    {
        var asmCompanyAttr = assembly.GetCustomAttribute<AssemblyCompanyAttribute>();
        if (asmCompanyAttr == null)
        {
            return false;
        }
        else
        {
            string companyName = asmCompanyAttr.Company;
            return companyName.Contains("Microsoft");
        }
    }

    /// <summary>
    /// 是否是微软等的官方 Assembly
    /// </summary>
    /// <param name="assemblyPath"></param>
    /// <returns></returns>
    private static bool IsSystemAssembly(string assemblyPath)
    {
        var assembly = Assembly.LoadFrom(assemblyPath);
        return IsSystemAssembly(assembly);
    }

    /// <summary>
    /// 判断程序集是否有效
    /// </summary>
    /// <param name="assembly"></param>
    /// <returns></returns>
    private static bool IsValid(Assembly assembly)
    {
        try
        {
            assembly.GetTypes();
            _ = assembly.DefinedTypes.ToList();
            return true;
        }
        catch (ReflectionTypeLoadException)
        {
            return false;
        }
    }

    /// <summary>
    /// 判断这个文件是否是程序集
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    private static bool IsManagedAssembly(string file)
    {
        using var fs = File.OpenRead(file);
        using PEReader peReader = new(fs);
        return peReader.HasMetadata && peReader.GetMetadataReader().IsAssembly;
    }

    /// <summary>
    /// 尝试加载程序集
    /// </summary>
    /// <param name="assemblyPath"></param>
    /// <returns></returns>
    private static Assembly? TryLoadAssembly(string assemblyPath)
    {
        AssemblyName assemblyName = AssemblyName.GetAssemblyName(assemblyPath);
        Assembly? assembly = null;
        try
        {
            assembly = Assembly.Load(assemblyName);
        }
        catch (BadImageFormatException ex)
        {
            Debug.WriteLine(ex);
        }
        catch (FileLoadException ex)
        {
            Debug.WriteLine(ex);
        }

        if (assembly == null)
        {
            try
            {
                assembly = Assembly.LoadFile(assemblyPath);
            }
            catch (BadImageFormatException ex)
            {
                Debug.WriteLine(ex);
            }
            catch (FileLoadException ex)
            {
                Debug.WriteLine(ex);
            }
        }
        return assembly;
    }

    #endregion
}

/// <summary>
/// 程序集相等性
/// </summary>
internal class AssemblyEquality : EqualityComparer<Assembly>
{
    public override bool Equals(Assembly? x, Assembly? y)
    {
        if (x == null && y == null) return true;
        if (x == null || y == null) return false;
        return AssemblyName.ReferenceMatchesDefinition(x.GetName(), y.GetName());
    }

    public override int GetHashCode([DisallowNull] Assembly obj)
    {
        return obj.GetName().FullName.GetHashCode();
    }
}

/// <summary>
/// NuGet程序集
/// </summary>
public record NuGetPackage
{
    /// <summary>
    /// 程序集名称
    /// </summary>
    public string PackageName = string.Empty;

    /// <summary>
    /// 程序集版本
    /// </summary>
    public string PackageVersion = string.Empty;
}