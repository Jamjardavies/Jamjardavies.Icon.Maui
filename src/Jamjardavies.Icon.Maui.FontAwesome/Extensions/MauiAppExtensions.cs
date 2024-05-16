// <copyright file="MauiAppExtensions.cs" author="Jamjardavies">
//      Copyright (c) 2024 Jamjardavies.
// </copyright>

namespace Jamjardavies.Icon.Maui.FontAwesome;

public static class MauiAppExtensions
{
    #region Methods

    #region Public

    public static MauiAppBuilder UseFontAwesome(this MauiAppBuilder builder)
    {
        builder.ConfigureFonts(
            fonts =>
            {
                fonts.AddEmbeddedResourceFont(FontAwesomeIconStyles.Solid)
                     .AddEmbeddedResourceFont(FontAwesomeIconStyles.Regular)
                     .AddEmbeddedResourceFont(FontAwesomeIconStyles.Brands);
            });

        return builder;
    }

    #endregion

    #endregion

    private static IFontCollection AddEmbeddedResourceFont(this IFontCollection fonts, FontAwesomeIconStyles style)
    {
        fonts.AddEmbeddedResourceFont(typeof(MauiAppExtensions).Assembly, $"FontAwesome-{style}.otf", FontAwesomeExtension.StyleMap[style]);
        return fonts;
    }
}