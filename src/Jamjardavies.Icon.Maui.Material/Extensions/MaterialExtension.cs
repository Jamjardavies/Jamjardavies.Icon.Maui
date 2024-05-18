// <copyright file="MaterialExtension.cs" author="Jamjardavies">
//      Copyright (c) 2024 Jamjardavies.
// </copyright>

namespace Jamjardavies.Icon.Maui.Material;

public enum MaterialIconStyles
{
    Outlined,
    OutlinedFilled,
    Rounded,
    RoundedFilled,
    Sharp,
    SharpFilled
}

public sealed class MaterialExtension : IconExtension<MaterialIcon, MaterialIconStyles>
{
    internal const string OutlinedStyle = "MaterialOutlined";
    internal const string OutlinedFilledStyle = "MaterialOutlinedFilled";
    internal const string RoundedStyle = "MaterialRounded";
    internal const string RoundedFilledStyle = "MaterialRoundedFilled";
    internal const string SharpStyle = "MaterialSharp";
    internal const string SharpFilledStyle = "MaterialSharpFilled";

    internal static readonly Dictionary<MaterialIconStyles, string> StyleMap = new()
    {
        { MaterialIconStyles.Outlined, OutlinedStyle },
        { MaterialIconStyles.OutlinedFilled, OutlinedFilledStyle },
        { MaterialIconStyles.Rounded, RoundedStyle },
        { MaterialIconStyles.RoundedFilled, RoundedFilledStyle },
        { MaterialIconStyles.Sharp, SharpStyle },
        { MaterialIconStyles.SharpFilled, SharpFilledStyle }
    };

    #region Properties

    /// <inheritdoc />
    protected override Dictionary<MaterialIconStyles, string> IconStyleMap => StyleMap;

    #endregion
}