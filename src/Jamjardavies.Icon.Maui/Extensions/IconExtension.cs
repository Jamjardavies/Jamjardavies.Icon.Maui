// <copyright file="IconExtension.cs" author="Jamjardavies">
//      Copyright (c) 2024 Jamjardavies.
// </copyright>

using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using Microsoft.Maui.Graphics.Converters;

namespace Jamjardavies.Icon.Maui;

[ContentProperty(nameof(Icon))]
[RequireService([typeof(IProvideValueTarget), typeof(IRootObjectProvider)])]
public abstract class IconExtension<TIcon, TIconStyle> : Element, IMarkupExtension<BindingBase>, IValueConverter where TIcon : Enum where TIconStyle : Enum
{
    public static readonly BindableProperty IconProperty = BindableProperty.Create(nameof(Icon), typeof(TIcon?), typeof(IconExtension<TIcon, TIconStyle>));

    public static readonly BindableProperty IconStyleProperty = BindableProperty.Create(nameof(IconStyle), typeof(TIconStyle?), typeof(IconExtension<TIcon, TIconStyle>));

    public static readonly BindableProperty IconColorProperty = BindableProperty.Create(nameof(IconColor), typeof(Color), typeof(IconExtension<TIcon, TIconStyle>), Colors.White);

    public static readonly BindableProperty IconSizeProperty = BindableProperty.Create(nameof(IconSize), typeof(double), typeof(IconExtension<TIcon, TIconStyle>), 32d);

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

    #region IMarkupExtension<BindingBase> Members

    /// <inheritdoc />
    object? IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
    {
        IProvideValueTarget valueProvider = serviceProvider.GetService<IProvideValueTarget>() ?? throw new ArgumentException("Unable to get IProvideValueTarget service.");

        if (valueProvider.TargetObject is not Setter setter)
        {
            return this.ProvideValue(serviceProvider);
        }

        this.SetResourceParent(serviceProvider, valueProvider);
        return this.Convert(null, this.GetType(), new IconBinding(setter.Property.ReturnType, valueProvider.TargetObject), CultureInfo.CurrentCulture);
    }

    /// <inheritdoc />
    public BindingBase ProvideValue(IServiceProvider serviceProvider)
    {
        IProvideValueTarget valueProvider = serviceProvider.GetService<IProvideValueTarget>() ?? throw new ArgumentException("Unable to get IProvideValueTarget service.");

        this.SetResourceParent(serviceProvider, valueProvider);

        Type propertyType = valueProvider.TargetObject is Setter setter
            ? setter.Property.ReturnType
            : valueProvider.TargetProperty switch
            {
                BindableProperty bp => bp.ReturnType,
                PropertyInfo pi => pi.PropertyType,
                _ => throw new InvalidOperationException()
            };

        return new Binding
        {
            Path = nameof(this.Icon),
            Converter = this,
            ConverterParameter = new IconBinding(propertyType, valueProvider.TargetObject),
            Mode = BindingMode.OneWay,
            Source = this
        };
    }

    #endregion

    #region IValueConverter Members

    /// <inheritdoc />
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (parameter is not IconBinding binding)
        {
            return null;
        }

        if (value is not TIcon boundIcon ||
            !Enum.IsDefined(typeof(TIcon), boundIcon))
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
            BindableType.ImageSource => this.CreateImageSource(),
            _ => throw new InvalidOperationException()
        };
    }

    /// <inheritdoc />
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
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

    private FontImageSource? CreateImageSource()
    {
        if (this.Icon is null)
        {
            return null;
        }

        FontImageSource source = this.Icon.ToImageSource(this.GetIconStyle(), this.IconColor, this.IconSize);

        source.SetBinding(
            FontImageSource.ColorProperty,
            new Binding
            {
                Path = nameof(this.IconColor),
                Source = this,
                Mode = BindingMode.OneWay
            });

        source.SetBinding(
            FontImageSource.SizeProperty,
            new Binding
            {
                Path = nameof(this.IconSize),
                Source = this,
                Mode = BindingMode.OneWay
            });

        return source;
    }

    private void SetResourceParent(IServiceProvider serviceProvider, IProvideValueTarget valueProvider)
    {
        if (valueProvider.TargetObject is Element targetElement)
        {
            this.Parent = targetElement;
        }
        else if (serviceProvider.GetService<IRootObjectProvider>()?.RootObject is Element rootElement)
        {
            this.Parent = rootElement;
        }
        else
        {
            return;
        }

        this.SetBinding(BindingContextProperty, new Binding
        {
            Path = nameof(this.BindingContext),
            Source = this.Parent,
            Mode = BindingMode.OneWay
        });
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
}