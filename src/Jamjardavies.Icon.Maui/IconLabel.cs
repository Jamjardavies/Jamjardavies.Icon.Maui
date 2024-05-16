// <copyright file="IconLabel.cs" author="Jamjardavies">
//      Copyright (c) 2024 Jamjardavies.
// </copyright>

namespace Jamjardavies.Icon.Maui;

/// <summary>
///     Provides a lightweight control for displaying icons as text.
/// </summary>
public class IconLabel : Label, IIcon, ISpinnable
{
    /// <inheritdoc cref="IconProperties.IconProperty" />
    public static readonly BindableProperty IconProperty = IconProperties.IconProperty;

    /// <inheritdoc cref="IconProperties.SpinProperty" />
    public static readonly BindableProperty SpinProperty = IconProperties.SpinProperty;

    /// <inheritdoc cref="IconProperties.SpinDurationProperty" />
    public static readonly BindableProperty SpinDurationProperty = IconProperties.SpinDurationProperty;

    #region Fields

    private readonly SpinnerAnimation spinAnimation;

    #endregion

    public IconLabel()
    {
        this.spinAnimation = new SpinnerAnimation(this);
    }

    #region Properties

    /// <summary>
    ///     Gets or sets the FontAwesomeLabel icon.
    ///     Note: Changing this property will cause the icon to be redrawn.
    /// </summary>
    public Icon? Icon
    {
        get => this.GetValue(IconProperty) as Icon;
        set => this.SetValue(IconProperty, value);
    }

    #region ISpinnable Properties

    /// <inheritdoc />
    public bool Spin
    {
        get => (bool)this.GetValue(SpinProperty);
        set => this.SetValue(SpinProperty, value);
    }

    /// <inheritdoc />
    public double SpinDuration
    {
        get => (double)this.GetValue(SpinDurationProperty);
        set => this.SetValue(SpinDurationProperty, value);
    }

    #endregion

    #endregion

    #region Methods

    #region Public

    #region IIcon Members

    /// <inheritdoc />
    public void UpdateIcon()
    {
        Icon? icon = this.Icon;

        if (icon?.Glyph is null)
        {
            return;
        }

        this.SetValue(FontFamilyProperty, icon.Glyph.ToFontFamily(icon.Style));
        this.SetValue(TextProperty, icon.Glyph.ToIconGlyph());
    }

    #endregion

    #region ISpinnable Members

    /// <inheritdoc />
    public void BeginSpin()
    {
        this.spinAnimation.Length = this.SpinDuration;
        this.spinAnimation.Start();
    }

    /// <inheritdoc />
    public void StopSpin()
    {
        this.spinAnimation.Stop();
    }

    #endregion

    #endregion

    #endregion
}