﻿// <copyright file="IconStyleAttribute.cs" author="Jamjardavies">
//      Copyright (c) 2024 Jamjardavies.
// </copyright>

namespace Jamjardavies.Icon.Maui;

/// <summary>
///     Represents the Font Family the icon is in.
/// </summary>
[AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
public class IconStyleAttribute : Attribute
{
    /// <summary>
    ///     Initializes a new instance of the IconStyleAttribute class.
    /// </summary>
    /// <param name="fontFamily">
    ///     Font Family the icon is in.
    /// </param>
    public IconStyleAttribute(string fontFamily)
    {
        this.FontFamily = fontFamily;
    }

    #region Properties

    /// <summary>
    ///     Gets or sets the style the icon is in.
    /// </summary>
    public string FontFamily { get; set; }

    #endregion
}