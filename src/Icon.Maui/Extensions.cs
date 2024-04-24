// <copyright file="Extensions.cs" company="Jamjardavies">
//      Copyright (c) 2024 Jamjardavies.
// </copyright>

using System.Reflection;

namespace Icon.Maui;

internal static class Extensions
{
    internal static string ToFontFamily<TIconType>(this TIconType icon)
        where TIconType : Enum
    {
        return GetValueAttribute<IconStyleAttribute, TIconType>(icon)?.FontFamily ?? throw new InvalidOperationException($"IconStyle attribute is missing from {icon}.");
    }

    internal static TAttribute? GetValueAttribute<TAttribute, TIconType>(TIconType icon)
        where TAttribute : Attribute
        where TIconType : Enum
    {
        MemberInfo? memberInfo = icon.GetType()
                                     .GetMember(icon.ToString())
                                     .FirstOrDefault(m => m.MemberType == MemberTypes.Field);

        return memberInfo?.GetCustomAttributes<TAttribute>(false).SingleOrDefault();
    }

    internal static string ToIconGlyph<TIconType>(this TIconType icon)
        where TIconType : Enum
    {
        return char.ConvertFromUtf32(Convert.ToInt32(icon));
    }
}