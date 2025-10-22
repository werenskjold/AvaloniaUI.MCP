# Fix XAML Validation WPF Element Checks

## Why
The XamlValidationTool incorrectly identifies DockPanel, UniformGrid, and Viewbox as WPF-only elements (line 194), when all three are actually available in AvaloniaUI. This causes false warnings that confuse users and reduces trust in the validation tool.

## What Changes
- Remove incorrect WPF-only element checks for DockPanel, UniformGrid, and Viewbox
- Add proper documentation about which controls are truly WPF-specific
- Research and add actual WPF-only controls if any exist
- Add test cases to prevent regression

## Impact
- Affected specs: xaml-validation (new)
- Affected code: src/AvaloniaUI.MCP/Tools/XamlValidationTool.cs line 194
- Breaking change: NO (removes false warnings, improves accuracy)
- Users will no longer receive incorrect warnings about valid AvaloniaUI controls
