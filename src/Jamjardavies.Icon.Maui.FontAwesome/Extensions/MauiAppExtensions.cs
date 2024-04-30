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
                fonts.AddEmbeddedResourceFont(
                    typeof(MauiAppExtensions).Assembly,
                    "FontAwesomeBrands-Regular.otf",
                    "FontAwesomeBrands");

                fonts.AddEmbeddedResourceFont(
                    typeof(MauiAppExtensions).Assembly,
                    "FontAwesome-Solid.otf",
                    "FontAwesomeSolid");
            });

        return builder;
    }

    #endregion

    #endregion
}