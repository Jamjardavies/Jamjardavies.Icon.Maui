// <copyright file="FontAwesomeExtension.cs" author="Jamjardavies">
//      Copyright (c) 2024 Jamjardavies.
// </copyright>

namespace Jamjardavies.Icon.Maui.FontAwesome;

public enum FontAwesomeIconStyles
{
    Solid,
    Regular,
    Brands
}

public sealed class FontAwesomeExtension : IconExtension<FontAwesomeIcon, FontAwesomeIconStyles>
{
    internal const string SolidStyle = "FontAwesomeSolid";
    internal const string RegularStyle = "FontAwesomeRegular";
    internal const string BrandsStyle = "FontAwesomeBrands";

    /// <inheritdoc />
    protected override Dictionary<FontAwesomeIconStyles, string> IconStyleMap { get; } = new()
    {
        { FontAwesomeIconStyles.Solid, SolidStyle },
        { FontAwesomeIconStyles.Regular, RegularStyle },
        { FontAwesomeIconStyles.Brands, BrandsStyle }
    };
}