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
                fonts.AddEmbeddedResourceFont(
                    typeof(MauiAppExtensions).Assembly,
                    "MaterialSymbolsOutlined.ttf",
                    MaterialExtension.OutlinedStyle);
            });

        return builder;
    }

    #endregion

    #endregion
}