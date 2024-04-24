// <copyright file="IconLabel.cs" company="Jamjardavies">
//      Copyright (c) 2024 Jamjardavies.
// </copyright>

namespace Icon.Maui;

/// <summary>
///     Provides a lightweight control for displaying icons as text.
/// </summary>
public abstract class IconLabel<TIconType> : Label, IIcon, ISpinnable
    where TIconType : Enum
{
    /// <summary>
    ///     Identifies the Icon dependency property.
    /// </summary>
    public static readonly BindableProperty IconProperty =
        IconProperties.CreateIconProperty((TIconType)Enum.ToObject(typeof(TIconType), 0));

    private readonly SpinnerAnimation spinAnimation;

    protected IconLabel()
    {
        this.spinAnimation = new SpinnerAnimation(this);
    }

    /// <summary>
    ///     Gets or sets the FontAwesomeLabel icon.
    ///     Note: Changing this property will cause the icon to be redrawn.
    /// </summary>
    public TIconType Icon
    {
        get => (TIconType)this.GetValue(IconProperty);
        set => this.SetValue(IconProperty, value);
    }

    /// <inheritdoc />
    public bool Spin
    {
        get => (bool)this.GetValue(IconProperties.SpinProperty);
        set => this.SetValue(IconProperties.SpinProperty, value);
    }

    /// <inheritdoc />
    public double SpinDuration
    {
        get => (double)this.GetValue(IconProperties.SpinDurationProperty);
        set => this.SetValue(IconProperties.SpinDurationProperty, value);
    }

    /// <inheritdoc />
    public void UpdateIcon()
    {
        TIconType icon = this.Icon;

        this.SetValue(FontFamilyProperty, icon.ToFontFamily());
        this.SetValue(TextProperty, icon.ToIconGlyph());
    }

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
}