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
                    "FontAwesome-Solid.otf",
                    FontAwesomeExtension.SolidStyle);

                fonts.AddEmbeddedResourceFont(
                    typeof(MauiAppExtensions).Assembly,
                    "FontAwesome-Regular.otf",
                    FontAwesomeExtension.RegularStyle);

                fonts.AddEmbeddedResourceFont(
                    typeof(MauiAppExtensions).Assembly,
                    "FontAwesomeBrands-Regular.otf",
                    FontAwesomeExtension.BrandsStyle);
            });

        return builder;
    }

    #endregion

    #endregion
}