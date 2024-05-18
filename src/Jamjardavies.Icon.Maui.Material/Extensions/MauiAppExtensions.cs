// <copyright file="MauiAppExtensions.cs" author="Jamjardavies">
//      Copyright (c) 2024 Jamjardavies.
// </copyright>

namespace Jamjardavies.Icon.Maui.Material;

public static class MauiAppExtensions
{
    #region Methods

    #region Public

    public static MauiAppBuilder UseMaterial(this MauiAppBuilder builder)
    {
        builder.ConfigureFonts(
            fonts =>
            {
                fonts.AddEmbeddedResourceFont(MaterialIconStyles.Outlined)
                     .AddEmbeddedResourceFont(MaterialIconStyles.OutlinedFilled)
                     .AddEmbeddedResourceFont(MaterialIconStyles.Rounded)
                     .AddEmbeddedResourceFont(MaterialIconStyles.RoundedFilled)
                     .AddEmbeddedResourceFont(MaterialIconStyles.Sharp)
                     .AddEmbeddedResourceFont(MaterialIconStyles.SharpFilled);
            });

        return builder;
    }

    #endregion

    #endregion

    private static IFontCollection AddEmbeddedResourceFont(this IFontCollection fonts, MaterialIconStyles style)
    {
        fonts.AddEmbeddedResourceFont(typeof(MauiAppExtensions).Assembly, $"MaterialSymbols{style}.ttf", MaterialExtension.StyleMap[style]);
        return fonts;
    }
}