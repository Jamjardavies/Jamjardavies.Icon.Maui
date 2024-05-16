// <copyright file="MaterialExtension.cs" author="Jamjardavies">
//      Copyright (c) 2024 Jamjardavies.
// </copyright>

namespace Jamjardavies.Icon.Maui.Material;

public enum MaterialIconStyles
{
    Outlined,
    Rounded,
    Sharp
}

public sealed class MaterialExtension : IconExtension<MaterialIcon, MaterialIconStyles>
{
    internal const string OutlinedStyle = "MaterialOutlined";
    internal const string RoundedStyle = "MaterialRounded";
    internal const string SharpStyle = "MaterialSharp";

    internal static readonly Dictionary<MaterialIconStyles, string> StyleMap = new()
    {
        { MaterialIconStyles.Outlined, OutlinedStyle },
        { MaterialIconStyles.Rounded, RoundedStyle },
        { MaterialIconStyles.Sharp, SharpStyle }
    };

    #region Properties

    /// <inheritdoc />
    protected override Dictionary<MaterialIconStyles, string> IconStyleMap => StyleMap;

    #endregion
}