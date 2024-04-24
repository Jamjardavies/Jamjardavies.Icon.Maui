// <copyright file="IconButton.cs" company="Jamjardavies">
//      Copyright (c) 2024 Jamjardavies.
// </copyright>

namespace Icon.Maui;

/// <summary>
///     Base button for showing an Icon.
/// </summary>
public abstract class IconButton<TIconType> : Button, IIcon
    where TIconType : Enum
{
    /// <summary>
    ///     Identifies the Icon dependency property.
    /// </summary>
    public static readonly BindableProperty IconProperty =
        IconProperties.CreateIconProperty((TIconType)Enum.ToObject(typeof(TIconType), 0));

    #region Properties

    /// <summary>
    ///     Gets or sets the FontAwesomeLabel icon.
    ///     Note: Changing this property will cause the icon to be redrawn.
    /// </summary>
    public TIconType Icon
    {
        get => (TIconType)this.GetValue(IconProperty);
        set => this.SetValue(IconProperty, value);
    }

    /// <summary>
    ///     The colour of the text used for the image.
    /// </summary>
    public Color IconColor
    {
        get => (Color)this.GetValue(IconProperties.IconColorProperty);
        set => this.SetValue(IconProperties.IconColorProperty, value);
    }

    /// <summary>
    ///     The size of the font to use for the image.
    /// </summary>
    public int IconSize
    {
        get => (int)this.GetValue(IconProperties.IconSizeProperty);
        set => this.SetValue(IconProperties.IconSizeProperty, value);
    }

    #endregion

    #region Methods

    #region Public

    #region IIcon Members

    /// <inheritdoc />
    public void UpdateIcon()
    {
        ImageSource image = this.CreateImageSource();

        this.SetValue(ImageSourceProperty, image);
    }

    #endregion

    #endregion

    #region Private

    private FontImageSource CreateImageSource()
    {
        TIconType icon = this.Icon;

        return new FontImageSource
        {
            Color = this.IconColor,
            FontFamily = icon.ToFontFamily(),
            Glyph = icon.ToIconGlyph(),
            Size = this.IconSize
        };
    }

    #endregion

    #endregion
}