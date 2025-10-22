# WPF Migration Specification

## ADDED Requirements

### Requirement: Accurate XAML Conversion
The WPF-to-AvaloniaUI conversion tool SHALL accurately convert WPF XAML patterns to their AvaloniaUI equivalents.

#### Scenario: Namespace conversion
- **WHEN** converting WPF XAML with presentation namespace
- **THEN** the tool SHALL replace `http://schemas.microsoft.com/winfx/2006/xaml/presentation` with `https://github.com/avaloniaui`
- **AND** preserve the XAML namespace unchanged
- **AND** validate the converted namespaces

#### Scenario: RelativeSource FindAncestor conversion
- **WHEN** converting XAML with `{Binding RelativeSource={RelativeSource FindAncestor, AncestorType={x:Type UserControl}}, Path=PropertyName}`
- **THEN** the tool SHALL convert to `{Binding $parent[UserControl].PropertyName}`
- **AND** warn if the pattern is complex and needs manual verification

#### Scenario: No-op replacements removed
- **WHEN** performing conversion
- **THEN** the tool SHALL NOT apply replacement rules that don't change the XAML
- **AND** all replacement rules SHALL provide actual value

### Requirement: Manual Intervention Guidance
The conversion tool SHALL identify patterns that require manual attention and provide specific guidance.

#### Scenario: Trigger detection
- **WHEN** WPF Triggers are detected in XAML
- **THEN** the tool SHALL warn that AvaloniaUI uses Style selectors instead
- **AND** provide examples of equivalent AvaloniaUI patterns
- **AND** link to relevant documentation

#### Scenario: DependencyProperty conversion
- **WHEN** DependencyProperty usage is detected
- **THEN** the tool SHALL warn about conversion to AvaloniaProperty
- **AND** provide code examples showing the conversion pattern
- **AND** note registration syntax differences

#### Scenario: RoutedCommand conversion
- **WHEN** RoutedCommand usage is detected
- **THEN** the tool SHALL recommend ReactiveCommand as alternative
- **AND** provide guidance on command implementation in AvaloniaUI

### Requirement: Conversion Quality Verification
The conversion tool SHALL validate that converted XAML is syntactically correct and follows AvaloniaUI best practices.

#### Scenario: Post-conversion validation
- **WHEN** conversion completes
- **THEN** the tool SHALL automatically validate the converted XAML
- **AND** report any syntax errors or warnings
- **AND** indicate confidence level in the conversion

#### Scenario: Partial conversion indication
- **WHEN** some patterns could not be automatically converted
- **THEN** the tool SHALL clearly mark sections needing manual review
- **AND** provide line numbers or element paths
- **AND** explain what needs to be done manually
