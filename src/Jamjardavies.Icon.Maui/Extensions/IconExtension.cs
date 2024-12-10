// <copyright file="IconExtension.cs" author="Jamjardavies">
//      Copyright (c) 2024 Jamjardavies.
// </copyright>

using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using Microsoft.Maui.Graphics.Converters;

namespace Jamjardavies.Icon.Maui;

[ContentProperty(nameof(Icon))]
#if NET9_0_OR_GREATER
[RequireService(new[] { typeof(IProvideValueTarget) })]
#endif
public abstract class IconExtension<TIcon, TIconStyle> : BindableObject, IMarkupExtension<BindingBase>, IValueConverter
    where TIcon : Enum
    where TIconStyle : Enum
{
    public static readonly BindableProperty IconProperty = BindableProperty.Create(
        nameof(Icon),
        typeof(TIcon?),
        typeof(IconExtension<TIcon, TIconStyle>));

    public static readonly BindableProperty IconStyleProperty = BindableProperty.Create(
        nameof(IconStyle),
        typeof(TIconStyle?),
        typeof(IconExtension<TIcon, TIconStyle>));

    public static readonly BindableProperty IconColorProperty = BindableProperty.Create(
        nameof(IconColor),
        typeof(Color),
        typeof(IconExtension<TIcon, TIconStyle>),
        Colors.White);

    public static readonly BindableProperty IconSizeProperty = BindableProperty.Create(
        nameof(IconSize),
        typeof(double),
        typeof(IconExtension<TIcon, TIconStyle>),
        32d);

    private static readonly Dictionary<Type, BindableType> BindableTypeMap = new()
    {
        { typeof(Icon), BindableType.Icon },
        { typeof(string), BindableType.String },
        { typeof(ImageSource), BindableType.ImageSource },
        { typeof(FontImageSource), BindableType.ImageSource }
    };

    #region Properties

    public TIcon? Icon
    {
        get => (TIcon?)this.GetValue(IconProperty);
        set => this.SetValue(IconProperty, value);
    }

    public TIconStyle? IconStyle
    {
        get => (TIconStyle)this.GetValue(IconStyleProperty);
        set => this.SetValue(IconStyleProperty, value);
    }

    [TypeConverter(typeof(ColorTypeConverter))]
    public Color IconColor
    {
        get => (Color)this.GetValue(IconColorProperty);
        set => this.SetValue(IconColorProperty, value);
    }

    [TypeConverter(typeof(FontSizeConverter))]
    public double IconSize
    {
        get => (double)this.GetValue(IconSizeProperty);
        set => this.SetValue(IconSizeProperty, value);
    }

    public IValueConverter? Converter { get; set; }

    public object? ConverterParameter { get; set; }

    protected abstract Dictionary<TIconStyle, string> IconStyleMap { get; }

    #endregion

    #region Methods

    #region Public

    #region IMarkupExtension Members

    /// <inheritdoc />
    object? IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
    {
        IProvideValueTarget valueProvider = serviceProvider.GetService<IProvideValueTarget>()
                                         ?? throw new ArgumentException("Unable to get IProvideValueTarget service.");

        if (valueProvider.TargetObject is Setter setter)
        {
            return this.Convert(
                null,
                this.GetType(),
                new IconBinding(setter.Property.ReturnType, valueProvider.TargetObject),
                CultureInfo.CurrentCulture);
        }

        return this.ProvideValue(serviceProvider);
    }

    /// <inheritdoc />
    public BindingBase ProvideValue(IServiceProvider serviceProvider)
    {
        IProvideValueTarget valueProvider = serviceProvider.GetService<IProvideValueTarget>()
                                         ?? throw new ArgumentException("Unable to get IProvideValueTarget service.");
        Type propertyType;

        if (valueProvider.TargetObject is Setter setter)
        {
            propertyType = setter.Property.ReturnType;
        }
        else
        {
            propertyType = valueProvider.TargetProperty switch
            {
                BindableProperty bp => bp.ReturnType,
                PropertyInfo pi => pi.PropertyType,
                _ => throw new InvalidOperationException()
            };
        }

        return new Binding
        {
            Converter = this,
            ConverterParameter = new IconBinding(propertyType, valueProvider.TargetObject),
            Mode = BindingMode.OneWay,
            Source = this
        };
    }

    #endregion

    #endregion

    #region Private

    private string GetIconStyle()
    {
        if (this.IconStyle is null || !this.IconStyleMap.TryGetValue(this.IconStyle, out string? fontFamily))
        {
            return string.Empty;
        }

        return fontFamily;
    }

    private string PopulateString(object targetObject)
    {
        if (this.Icon is null)
        {
            return string.Empty;
        }

        PropertyInfo? fontFamilyProp = targetObject.GetType().GetProperty("FontFamily");

        if (fontFamilyProp is null || !fontFamilyProp.CanWrite)
        {
            return this.Icon.ToIconGlyph();
        }

        fontFamilyProp.SetValue(targetObject, this.Icon.ToFontFamily(this.GetIconStyle()));

        return this.Icon.ToIconGlyph();
    }

    #endregion

    #endregion

    #region Nested type: BindableType

    private enum BindableType
    {
        Icon,
        String,
        ImageSource
    }

    #endregion

    /// <inheritdoc />
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (parameter is not IconBinding binding)
        {
            return null;
        }

        if (this.Icon is null)
        {
            return null;
        }

        Icon icon = new(this.Icon, this.GetIconStyle());

        if (this.Converter is not null)
        {
            return this.Converter.Convert(icon, binding.TargetType, this.ConverterParameter, CultureInfo.CurrentCulture);
        }

        if (!BindableTypeMap.TryGetValue(binding.TargetType, out BindableType type))
        {
            return null;
        }

        return type switch
        {
            BindableType.Icon => icon,
            BindableType.String => this.PopulateString(binding.TargetObject),
            BindableType.ImageSource => this.Icon.ToImageSource(this.GetIconStyle(), this.IconColor, this.IconSize),
            _ => throw new InvalidOperationException()
        };
    }

    /// <inheritdoc />
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}