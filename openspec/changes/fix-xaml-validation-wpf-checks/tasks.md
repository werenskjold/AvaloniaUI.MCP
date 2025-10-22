# Implementation Tasks

## 1. Research
- [ ] 1.1 Review AvaloniaUI documentation for complete control list
- [ ] 1.2 Identify truly WPF-specific controls (if any)
- [ ] 1.3 Document findings for future reference

## 2. Code Changes
- [ ] 2.1 Remove DockPanel, UniformGrid, Viewbox from wpfOnlyElements array
- [ ] 2.2 Add accurate WPF-specific controls if identified
- [ ] 2.3 Update ValidateAvaloniaSpecificIssues method documentation
- [ ] 2.4 Consider adding a comprehensive control compatibility check

## 3. Testing
- [ ] 3.1 Add test for DockPanel in XAML (should not warn)
- [ ] 3.2 Add test for UniformGrid in XAML (should not warn)
- [ ] 3.3 Add test for Viewbox in XAML (should not warn)
- [ ] 3.4 Add test for actual WPF-only controls (should warn)
- [ ] 3.5 Update existing validation tests if needed

## 4. Documentation
- [ ] 4.1 Update tool documentation with accurate control compatibility info
- [ ] 4.2 Add migration guide section about control availability
- [ ] 4.3 Document how the validation check works
