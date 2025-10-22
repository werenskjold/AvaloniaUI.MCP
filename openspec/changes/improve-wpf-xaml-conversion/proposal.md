# Improve WPF XAML Conversion Tool

## Why
The ConvertWpfXamlToAvalonia method in XamlValidationTool has placeholder replacements that don't actually perform any conversions (lines 123-144). Many replacement rules are identity operations like `{ "WindowState=\"Maximized\"", "WindowState=\"Maximized\"" }` that provide no value. The tool needs to actually convert WPF-specific patterns to AvaloniaUI equivalents.

## What Changes
- Remove no-op replacement rules that don't change anything
- Add actual WPF-to-AvaloniaUI conversion rules for:
  - Trigger syntax → AvaloniaUI styling
  - FindAncestor RelativeSource → $parent syntax
  - WPF-specific attached properties
  - Thickness constructor differences
  - Event handler syntax differences
- Improve detection of patterns requiring manual attention
- Add comprehensive test cases for conversions
- Document conversion limitations and manual steps

## Impact
- Affected specs: wpf-migration (new)
- Affected code: src/AvaloniaUI.MCP/Tools/XamlValidationTool.cs lines 103-189
- Breaking change: NO (improves existing functionality)
- Users will get more accurate and useful WPF-to-AvaloniaUI conversions
