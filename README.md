# Terminal.Gui Examples

Community examples and standalone demos for [Terminal.Gui](https://github.com/gui-cs/Terminal.Gui) — the cross-platform .NET console UI toolkit.

> **Note:** These examples are companion projects to the main Terminal.Gui repository.
> For the UICatalog demo app and ScenarioRunner, see [gui-cs/Terminal.Gui/Examples](https://github.com/gui-cs/Terminal.Gui/tree/develop/Examples).

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) (or later)
- For F# examples: F# support is included in the .NET SDK

## Building

```bash
dotnet restore
dotnet build
```

Or build a specific example:

```bash
cd Example
dotnet run
```

### Building Against Local Terminal.Gui Source

To build the examples against a local clone of Terminal.Gui (instead of the NuGet package), pass the `TerminalGuiRoot` property pointing to your Terminal.Gui repository root:

```bash
dotnet build -p:TerminalGuiRoot=../Terminal.Gui
dotnet run --project Example -p:TerminalGuiRoot=../Terminal.Gui
```

This replaces the NuGet `PackageReference` with a `ProjectReference`, so any breaking changes in Terminal.Gui are caught immediately during development.

## Examples

| Project | Description |
|---------|-------------|
| [Example](Example/) | Minimal Terminal.Gui app — good starting point |
| [CommunityToolkitExample](CommunityToolkitExample/) | MVVM with CommunityToolkit.Mvvm |
| [ReactiveExample](ReactiveExample/) | ReactiveUI integration |
| [FluentExample](FluentExample/) | Fluent API usage |
| [FSharpExample](FSharpExample/) | Terminal.Gui from F# |
| [InlineCLI](InlineCLI/) | Inline CLI rendering |
| [InlineColorPicker](InlineColorPicker/) | Inline color picker demo |
| [InlineSelect](InlineSelect/) | Inline selection demo |
| [PromptExample](PromptExample/) | Prompt/dialog usage |
| [SelfContained](SelfContained/) | AOT/self-contained publishing |
| [ShortcutTest](ShortcutTest/) | Shortcut/keybinding testing |
| [WideCharRepro](WideCharRepro/) | Wide character rendering |

### Non-.NET Examples

| File | Description |
|------|-------------|
| [DatePicker.ps1](DatePicker.ps1) | PowerShell DatePicker example |
| [PowershellExample.ps1](PowershellExample.ps1) | PowerShell Terminal.Gui example |

### Configuration Examples

| File | Description |
|------|-------------|
| [Config/example_config.json](Config/example_config.json) | Example configuration file |
| [Config/macos.json](Config/macos.json) | macOS-specific config |
| [Config/windows.json](Config/windows.json) | Windows-specific config |
| [Themes/code-dark.config.json](Themes/code-dark.config.json) | Dark theme configuration |

## Contributing

1. Follow Terminal.Gui [coding conventions](https://github.com/gui-cs/Terminal.Gui/blob/develop/CONTRIBUTING.md)
2. Examples reference Terminal.Gui via NuGet by default; use `TerminalGuiRoot` for local development
3. Each example should have its own project directory with a descriptive name
4. Include a README.md in each example explaining what it demonstrates

## License

[MIT](LICENSE)
