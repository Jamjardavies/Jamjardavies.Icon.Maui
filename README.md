# Jamjardavies.Icon.Maui
Readme will be updated soon with more information, below is a quick-start guide.

## Using Jamjardavies.Icon.Maui
First, inside of the `MauiProgram.cs` file, add `UseMauiIcons`

```csharp
return MauiApp.CreateBuilder()
              .UseMauiApp<App>()
              .UseMauiIcons()
              .Build();
```

Next one of the icon packages will be required in order to use. Follow the following steps. Note: More icon packages coming soon.

## Using Jamjardavies.Icon.Maui.FontAwesome
First, inside of the `MauiProgram.cs` file, add `UseFontAwesome`

```csharp
return MauiApp.CreateBuilder()
              .UseMauiApp<App>()
              .UseMauiIcons()
              .UseFontAwesome()
              .Build();
```

## Xaml
To use the icons in Xaml, first a namespace is required using: `xmlns:icon="http://www.jamjardavies.co.uk/maui/icon"`, next use the following controls:

### Maui Label
```xml
<Label Text="{icon:FontAwesome Spinner}" />
```

### Maui Button
Using ImageSource:
```xml
<Button ImageSource="{icon:FontAwesome Facebook}"
                Text="Facebook"
                FontSize="32" />
```

Using ImageSource and Text:
```xml
<Button ImageSource="{icon:FontAwesome Facebook}"
                Text="{icon:FontAwesome Facebook}"
                FontSize="32" />
```

### IconLabel
```xml
<icon:IconLabel Margin="16"
                Icon="{icon:FontAwesome Spinner}" 
                Spin="True"
                SpinDuration="4"
                HorizontalOptions="Center"
                FontSize="48" />
```

### IconButton
```xml
<icon:IconButton Icon="{icon:FontAwesome Facebook}"
                 IconSize="32"
                 Text="Facebook" />
```