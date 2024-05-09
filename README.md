# Jamjardavies.Icon.Maui
Readme will be updated soon with more information, below is a quick-start guide.

## Using Jamjardavies.Icon.Maui.FontAwesome
First, inside of the `MauiProgram.cs` file, add `UseFontAwesome`

```csharp
return MauiApp.CreateBuilder()
              .UseMauiApp<App>()
              .UseFontAwesome()
              .Build();
```

## Using Jamjardavies.Icon.Maui.Material
First, inside of the `MauiProgram.cs` file, add `UseMaterial`

```csharp
return MauiApp.CreateBuilder()
              .UseMauiApp<App>()
              .UseMaterial()
              .Build();
```

## Xaml
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