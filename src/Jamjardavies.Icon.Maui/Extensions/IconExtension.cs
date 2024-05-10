// <copyright file="IconExtension.cs" author="Jamjardavies">
//      Copyright (c) 2024 Jamjardavies.
// </copyright>

using System.ComponentModel;
using System.Reflection;
using Microsoft.Maui.Graphics.Converters;

namespace Jamjardavies.Icon.Maui;

[ContentProperty(nameof(Icon))]
public abstract class IconExtension<TEnum> : IMarkupExtension where TEnum : Enum
{
    private static readonly Dictionary<Type, BindableType> BindableTypeMap = new()
    {
        { typeof(Enum), BindableType.Enum },
        { typeof(string), BindableType.String },
        { typeof(ImageSource), BindableType.ImageSource }
    };

    #region Properties

    public TEnum? Icon { get; set; }

    [TypeConverter(typeof(ColorTypeConverter))]
    public Color IconColor { get; set; } = Colors.White;

    [TypeConverter(typeof(FontSizeConverter))]
    public double IconSize { get; set; } = 32;

    #endregion

    #region Methods

    #region Public

    #region IMarkupExtension Members

    /// <inheritdoc />
    public object? ProvideValue(IServiceProvider serviceProvider)
    {
        IProvideValueTarget valueProvider = serviceProvider.GetService<IProvideValueTarget>()
                                         ?? throw new ArgumentException("Unable to get IProvideValueTarget service.");

        if (this.Icon is null)
        {
            return null;
        }

        if (valueProvider.TargetProperty is not BindableProperty bindableProperty)
        {
            return null;
        }

        if (!BindableTypeMap.TryGetValue(bindableProperty.ReturnType, out BindableType type))
        {
            return null;
        }

        return type switch
        {
            BindableType.Enum => this.Icon,
            BindableType.String => this.PopulateString(valueProvider.TargetObject),
            BindableType.ImageSource => this.Icon.ToIconSource(this.IconColor, this.IconSize),
            _ => throw new InvalidOperationException()
        };
    }

    #endregion

    #endregion

    #region Private

    private string PopulateString(object targetObject)
    {
        if (this.Icon is null)
        {
            return string.Empty;
        }

        PropertyInfo? fontFamilyProp = targetObject.GetType().GetProperty("FontFamily");

        if (fontFamilyProp is not null && fontFamilyProp.CanWrite)
        {
            fontFamilyProp.SetValue(targetObject, this.Icon.ToFontFamily());
        }

        return this.Icon.ToIconGlyph();
    }

    #endregion

    #endregion

    #region Nested type: BindableType

    private enum BindableType
    {
        Enum,
        String,
        ImageSource
    }

    #endregion
}