// <copyright file="IconExtension.cs" author="Jamjardavies">
//      Copyright (c) 2024 Jamjardavies.
// </copyright>

namespace Jamjardavies.Icon.Maui;

[ContentProperty(nameof(Icon))]
public abstract class IconExtension<TEnum> : IMarkupExtension where TEnum : Enum
{
    #region Properties

    public TEnum? Icon { get; set; }

    #endregion

    #region Methods

    #region Public

    #region IMarkupExtension Members

    /// <inheritdoc />
    public object? ProvideValue(IServiceProvider serviceProvider)
    {
        IProvideValueTarget valueProvider = serviceProvider.GetService<IProvideValueTarget>()
                                         ?? throw new ArgumentException("Unable to get IProvideValueTarget service.");

        if (valueProvider.TargetProperty is not BindableProperty bindableProperty)
        {
            return null;
        }

        Type returnType = bindableProperty.ReturnType;

        return valueProvider.TargetObject switch
        {
            IconLabel => this.Icon,
            IconButton button => this.Icon,
            Label label => this.PopulateLabel(label),
            Button button => this.PopulateButton(button, returnType),
            _ => this.Icon
        };
    }

    #endregion

    #endregion

    #region Private

    private string PopulateLabel(Label label)
    {
        if (this.Icon is null)
        {
            throw new NullReferenceException("Icon must be set!");
        }

        label.FontFamily = this.Icon.ToFontFamily();

        return this.Icon.ToIconGlyph();
    }

    private object? PopulateButton(Button button, Type returnType)
    {
        if (this.Icon is null)
        {
            return null;
        }

        if (returnType == typeof(ImageSource))
        {
            return this.Icon.ToIconSource(button.TextColor, button.FontSize);
        }

        if (returnType == typeof(string))
        {
            button.FontFamily = this.Icon.ToFontFamily();
            return this.Icon.ToIconGlyph();
        }

        return null;
    }

    #endregion

    #endregion
}