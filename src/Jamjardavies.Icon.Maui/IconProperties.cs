// <copyright file="IconProperties.cs" author="Jamjardavies">
//      Copyright (c) 2024 Jamjardavies.
// </copyright>

namespace Jamjardavies.Icon.Maui;

/// <summary>
///     Contains all <see cref="BindableProperty" /> for icons.
/// </summary>
public static class IconProperties
{
    /// <summary>
    ///     Identifies the Icon dependency property.
    /// </summary>
    public static readonly BindableProperty IconProperty = BindableProperty.Create(
        "Icon",
        typeof(Icon),
        typeof(IIcon),
        propertyChanged: OnIconChanged);

    /// <summary>
    ///     Identifies the IconColor dependency property.
    /// </summary>
    public static readonly BindableProperty IconColorProperty = BindableProperty.Create(
        "IconColor",
        typeof(Color),
        typeof(IIcon),
        Colors.White,
        propertyChanged: OnIconColorPropertyChanged);

    /// <summary>
    ///     Identifies the IconSize dependency property.
    /// </summary>
    public static readonly BindableProperty IconSizeProperty = BindableProperty.Create(
        "IconSize",
        typeof(int),
        typeof(IIcon),
        24,
        propertyChanged: OnIconSizePropertyChanged);

    /// <summary>
    ///     Identifies the Spin dependency property.
    /// </summary>
    public static readonly BindableProperty SpinProperty = BindableProperty.Create(
        "Spin",
        typeof(bool),
        typeof(IIcon),
        false,
        propertyChanged: OnSpinPropertyChanged,
        coerceValue: SpinCoerceValue);

    /// <summary>
    ///     Identifies the SpinDuration dependency property.
    /// </summary>
    public static readonly BindableProperty SpinDurationProperty = BindableProperty.Create(
        "SpinDuration",
        typeof(double),
        typeof(IIcon),
        1d,
        propertyChanged: OnSpinDurationPropertyChanged,
        coerceValue: SpinDurationCoerceValue);

    #region Methods

    #region Public

    public static BindableProperty CreateIconProperty<TIcon>(TIcon defaultContent)
    {
        return BindableProperty.CreateAttached(
            "Icon",
            typeof(TIcon),
            typeof(IIcon),
            defaultContent,
            propertyChanged: OnIconChanged);
    }

    #endregion

    #region Private

    private static void OnIconChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not IIcon icon)
        {
            return;
        }

        icon.UpdateIcon();
    }

    private static void OnIconStylePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not IIcon icon)
        {
            return;
        }

        icon.UpdateIcon();
    }

    private static void OnIconColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not IIcon icon)
        {
            return;
        }

        icon.UpdateIcon();
    }

    private static void OnIconSizePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not IIcon icon)
        {
            return;
        }

        icon.UpdateIcon();
    }

    private static void OnSpinPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not ISpinnable spinnable)
        {
            return;
        }

        if ((bool)newValue)
        {
            spinnable.BeginSpin();
        }
        else
        {
            spinnable.StopSpin();
        }
    }

    private static object SpinCoerceValue(BindableObject bindable, object baseValue)
    {
        if (bindable is not ISpinnable spinnable)
        {
            return false;
        }

        if (bindable is VisualElement visualElement && (!visualElement.IsVisible || visualElement.Opacity is 0.0))
        {
            return false;
        }

        return spinnable.SpinDuration is 0.0 ? false : baseValue;
    }

    private static void OnSpinDurationPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not ISpinnable spinnable)
        {
            return;
        }

        if (!spinnable.Spin || newValue is not double || newValue.Equals(oldValue))
        {
            return;
        }

        spinnable.StopSpin();
        spinnable.BeginSpin();
    }

    private static object SpinDurationCoerceValue(BindableObject bindable, object value)
    {
        return (double)value < 0 ? 0d : value;
    }

    #endregion

    #endregion
}