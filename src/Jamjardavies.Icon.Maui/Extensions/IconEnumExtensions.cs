// <copyright file="IconEnumExtensions.cs" author="Jamjardavies">
//      Copyright (c) 2024 Jamjardavies.
// </copyright>

using System.Reflection;

namespace Jamjardavies.Icon.Maui;

public static class IconEnumExtensions
{
    #region Methods

    #region Public

    public static FontImageSource? ToIconSource<TIconType>(this TIconType icon)
        where TIconType : Enum
    {
        return icon.ToIconSource(Colors.White, 32, false);
    }

    public static FontImageSource? ToIconSource<TIconType>(this TIconType icon, Color color) where TIconType : Enum
    {
        return icon.ToIconSource(color, 32, false);
    }

    public static FontImageSource? ToIconSource<TIconType>(this TIconType icon, double size) where TIconType : Enum
    {
        return icon.ToIconSource(Colors.White, size, false);
    }

    public static FontImageSource? ToIconSource<TIconType>(this TIconType icon, Color color, double size)
        where TIconType : Enum
    {
        return icon.ToIconSource(color, size, false);
    }

    public static FontImageSource? ToIconSource<TIconType>(this TIconType icon, Color color, double size, bool autoScale)
        where TIconType : Enum
    {
        return new FontImageSource
        {
            Color = color,
            FontFamily = icon.ToFontFamily(),
            Glyph = icon.ToIconGlyph(),
            Size = size,
            FontAutoScalingEnabled = autoScale
        };
    }

    public static string ToFontFamily<TIconType>(this TIconType icon) where TIconType : Enum
    {
        return GetValueAttribute<IconStyleAttribute, TIconType>(icon)?.FontFamily
            ?? throw new InvalidOperationException($"IconStyle attribute is missing from {icon}.");
    }

    public static string ToIconGlyph<TIconType>(this TIconType icon) where TIconType : Enum
    {
        return char.ConvertFromUtf32(Convert.ToInt32(icon));
    }

    #endregion

    #region Internal

    internal static TAttribute? GetValueAttribute<TAttribute, TIconType>(TIconType icon)
        where TAttribute : Attribute where TIconType : Enum
    {
        MemberInfo? memberInfo = icon.GetType()
                                     .GetMember(icon.ToString())
                                     .FirstOrDefault(m => m.MemberType == MemberTypes.Field);

        return memberInfo?.GetCustomAttributes<TAttribute>(false).SingleOrDefault();
    }

    #endregion

    #endregion
}