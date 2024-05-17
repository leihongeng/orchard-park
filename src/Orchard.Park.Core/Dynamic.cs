using System;

namespace Orchard.Park.Core;

/// <summary>
/// 动态编译类
/// </summary>
public static class Dynamic
{
    /// <summary>
    /// 动态赋值
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="fieldName"></param>
    /// <param name="value"></param>
    public static void SetValue(this object obj, string fieldName, object value)
    {
        var info = obj.GetType().GetProperty(fieldName);
        if (info == null)
            throw new ArgumentNullException($"字段名:{fieldName}不存在");
        info.SetValue(obj, value);
    }

    /// <summary>
    /// 泛型动态赋值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="fieldName"></param>
    /// <param name="value"></param>
    public static void SetValue<T>(this object obj, string fieldName, T value)
    {
        var info = obj.GetType().GetProperty(fieldName);
        if (info == null)
            throw new ArgumentNullException($"字段名:{fieldName}不存在");
        info.SetValue(obj, value);
    }

    /// <summary>
    /// 动态取值
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="fieldName"></param>
    /// <returns></returns>
    public static object GetValue(this object obj, string fieldName)
    {
        var info = obj.GetType().GetProperty(fieldName);
        if (info == null)
            throw new ArgumentNullException($"字段名:{fieldName}不存在");
        return info.GetValue(obj);
    }

    /// <summary>
    /// 动态取值泛型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="fieldName"></param>
    /// <returns></returns>
    public static T GetValue<T>(this object obj, string fieldName)
    {
        var info = obj.GetType().GetProperty(fieldName);
        if (info == null)
            throw new ArgumentNullException($"字段名:{fieldName}不存在");
        return (T)info.GetValue(obj);
    }
}