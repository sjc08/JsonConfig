# JsonConfig

[![NuGet](https://img.shields.io/nuget/v/Asjc.JsonConfig)](https://www.nuget.org/packages/Asjc.JsonConfig/)

Quickly load and save configurations via JSON.

## Warning

- The package **System.Text.Json** used by this project has at least one **vulnerability** with **high** severity. Please update to the **latest version** as soon as possible to resolve the critical issue.

- As of 1.4.0, the Try* and Safe* methods have been removed as unnecessary and to streamline dependencies. Please install [Asjc.Utils](https://www.nuget.org/packages/Asjc.Utils) separately to use the simple Try method.

## Quick Start

```csharp
public class Settings : JsonConfig
{
    public string? Text { get; set; }
}
```

```csharp
var settings = JsonConfig.Load<Settings>();
if (settings != null)
{
    settings.Text = "Hello!";
    settings.Save();
}
```

## Credits

- Icon from https://www.iconfinder.com/icons/9040309