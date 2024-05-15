// <copyright file="IconEnumExtensions.cs" author="Jamjardavies">
//      Copyright (c) 2024 Jamjardavies.
// </copyright>

using System.Reflection;

namespace Jamjardavies.Icon.Maui;

public static class IconEnumExtensions
{
    #region Methods

    #region Public

    public static FontImageSource ToImageSource<TIcon>(this TIcon icon)
        where TIcon : Enum
    {
        return icon.ToImageSource(string.Empty, Colors.White, 32, false);
    }

    public static FontImageSource ToImageSource<TIcon>(this TIcon icon, Color color) where TIcon : Enum
    {
        return icon.ToImageSource(string.Empty, color, 32, false);
    }

    public static FontImageSource ToImageSource<TIcon>(this TIcon icon, double size) where TIcon : Enum
    {
        return icon.ToImageSource(string.Empty, Colors.White, size, false);
    }

    public static FontImageSource ToImageSource<TIcon>(this TIcon icon, Color color, double size)
        where TIcon : Enum
    {
        return icon.ToImageSource(string.Empty, color, size, false);
    }

    public static FontImageSource ToImageSource<TIcon>(this TIcon icon, string iconStyle, Color color, double size)
        where TIcon : Enum
    {
        return icon.ToImageSource(iconStyle, color, size, false);
    }

    public static FontImageSource ToImageSource<TIcon>(this TIcon icon, Color color, double size, bool autoScale)
        where TIcon : Enum
    {
        return icon.ToImageSource(string.Empty, color, size, autoScale);
    }

    public static FontImageSource ToImageSource<TIcon>(this TIcon icon, string iconStyle, Color color, double size, bool autoScale)
        where TIcon : Enum
    {
        return new FontImageSource
        {
            Color = color,
            FontFamily = icon.ToFontFamily(iconStyle),
            Glyph = icon.ToIconGlyph(),
            Size = size,
            FontAutoScalingEnabled = autoScale
        };
    }

    public static string ToFontFamily<TIcon>(this TIcon icon, string iconStyle = "")
        where TIcon : Enum
    {
        IEnumerable<IconStyleAttribute> fontFamilies = icon.GetValueAttributes<IconStyleAttribute, TIcon>().ToArray();

        IconStyleAttribute selectedFontFamily = fontFamilies.FirstOrDefault(a => a.FontFamily.Equals(iconStyle))
                                             ?? fontFamilies.FirstOrDefault()
                                             ?? throw new InvalidOperationException(
                                                    $"IconStyle attribute is missing from {icon}.");

        return selectedFontFamily.FontFamily;
    }

    public static string ToIconGlyph<TIconType>(this TIconType icon) where TIconType : Enum
    {
        return char.ConvertFromUtf32(Convert.ToInt32(icon));
    }

    #endregion

    #region Internal

    internal static TAttribute? GetValueAttribute<TAttribute, TIconType>(this TIconType icon)
        where TAttribute : Attribute where TIconType : Enum
    {
        return icon.GetValueAttributes<TAttribute, TIconType>().FirstOrDefault();
    }

    internal static IEnumerable<TAttribute> GetValueAttributes<TAttribute, TIconType>(this TIconType icon)
        where TAttribute : Attribute where TIconType : Enum
    {
        MemberInfo? memberInfo = icon.GetType()
                                     .GetMember(icon.ToString())
                                     .FirstOrDefault(m => m.MemberType == MemberTypes.Field);

        return memberInfo?.GetCustomAttributes<TAttribute>(false) ?? Array.Empty<TAttribute>();
    }

    #endregion

    #endregion
}