// <copyright file="IconButton.cs" author="Jamjardavies">
//      Copyright (c) 2024 Jamjardavies.
// </copyright>

namespace Jamjardavies.Icon.Maui;

/// <summary>
///     Base button for showing an Icon.
/// </summary>
public class IconButton : Button, IIcon
{
    /// <inheritdoc cref="IconProperties.IconProperty" />
    public static readonly BindableProperty IconProperty = IconProperties.IconProperty;

    /// <inheritdoc cref="IconProperties.IconColorProperty" />
    public static readonly BindableProperty IconColorProperty = IconProperties.IconColorProperty;

    /// <inheritdoc cref="IconProperties.IconSizeProperty" />
    public static readonly BindableProperty IconSizeProperty = IconProperties.IconSizeProperty;

    #region Properties

    /// <summary>
    ///     Gets or sets the FontAwesomeLabel icon.
    ///     Note: Changing this property will cause the icon to be redrawn.
    /// </summary>
    public Enum? Icon
    {
        get => (Enum?)this.GetValue(IconProperty);
        set => this.SetValue(IconProperty, value);
    }

    /// <summary>
    ///     The colour of the text used for the image.
    /// </summary>
    public Color IconColor
    {
        get => (Color)this.GetValue(IconColorProperty);
        set => this.SetValue(IconColorProperty, value);
    }

    /// <summary>
    ///     The size of the font to use for the image.
    /// </summary>
    public int IconSize
    {
        get => (int)this.GetValue(IconSizeProperty);
        set => this.SetValue(IconSizeProperty, value);
    }

    #endregion

    #region Methods

    #region Public

    #region IIcon Members

    /// <inheritdoc />
    public void UpdateIcon()
    {
        Enum? icon = this.Icon;

        if (icon is null)
        {
            return;
        }

        ImageSource? image = icon.ToIconSource(this.IconColor, this.IconSize);

        this.SetValue(ImageSourceProperty, image);
    }

    #endregion

    #endregion

    #endregion
}