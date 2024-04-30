// <copyright file="IconIdAttribute.cs" author="Jamjardavies">
//      Copyright (c) 2024 Jamjardavies.
// </copyright>

namespace Jamjardavies.Icon.Maui;

/// <summary>
///     Represents the ID (css class name) of the icon.
/// </summary>
[AttributeUsage(AttributeTargets.All, Inherited = false)]
public class IconIdAttribute : Attribute
{
    /// <summary>
    ///     Initializes a new instance of the IconIdAttribute class.
    /// </summary>
    /// <param name="id">
    ///     ID (css class name) of the icon.
    /// </param>
    public IconIdAttribute(string id)
    {
        this.Id = id;
    }

    #region Properties

    /// <summary>
    ///     Gets or sets the ID (css class name) of the icon.
    /// </summary>
    public string Id { get; set; }

    #endregion
}