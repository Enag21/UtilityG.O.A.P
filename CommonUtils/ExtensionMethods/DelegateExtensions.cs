using System;
using System.Linq;
using System.Linq.Expressions;

namespace UGOAP.CommonUtils.ExtensionMethods;

public static class DelegateExtensions
{
    public static T Clone<T>(this T original) where T : Delegate
    {
        return (T)original.DynamicInvoke(original.Method.GetParameters().Select(p => Expression.Parameter(p.ParameterType)).ToArray());
    }

}
