// <copyright file="MaterialExtension.cs" author="Jamjardavies">
//      Copyright (c) 2024 Jamjardavies.
// </copyright>

namespace Jamjardavies.Icon.Maui.Material;

public enum MaterialIconStyles
{
    Outlined
}

public sealed class MaterialExtension : IconExtension<MaterialIcon, MaterialIconStyles>
{
    internal const string OutlinedStyle = "MaterialOutlined";

    #region Properties

    /// <inheritdoc />
    protected override Dictionary<MaterialIconStyles, string> IconStyleMap { get; } = new()
    {
        { MaterialIconStyles.Outlined, OutlinedStyle }
    };

    #endregion
}