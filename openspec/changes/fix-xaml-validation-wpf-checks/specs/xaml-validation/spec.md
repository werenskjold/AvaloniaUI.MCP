# XAML Validation Specification

## ADDED Requirements

### Requirement: Accurate Control Availability Detection
The XAML validation tool SHALL accurately identify which controls are available in AvaloniaUI versus WPF-only.

#### Scenario: AvaloniaUI-supported control validation
- **WHEN** validating XAML containing DockPanel, UniformGrid, or Viewbox
- **THEN** the validator SHALL NOT warn about WPF-only elements
- **AND** should confirm these controls are valid AvaloniaUI controls

#### Scenario: Truly WPF-only control detection
- **WHEN** validating XAML containing a control that truly exists only in WPF
- **THEN** the validator SHALL warn the user with a specific message
- **AND** suggest AvaloniaUI alternatives if available

#### Scenario: No false positives
- **WHEN** validating valid AvaloniaUI XAML
- **THEN** the validator SHALL NOT produce warnings about incorrect control availability
- **AND** users SHALL trust the validation results

### Requirement: Control Compatibility Documentation
The validation tool SHALL provide accurate guidance about control compatibility between WPF and AvaloniaUI.

#### Scenario: Warning messages include alternatives
- **WHEN** a WPF-only control is detected
- **THEN** the warning message SHALL suggest AvaloniaUI alternatives
- **AND** provide guidance on migration approaches

#### Scenario: Validation report accuracy
- **WHEN** validation completes
- **THEN** all warnings and errors SHALL be factually correct
- **AND** based on current AvaloniaUI capabilities
