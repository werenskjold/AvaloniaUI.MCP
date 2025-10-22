# Implementation Tasks

## 1. Design
- [ ] 1.1 Decide hosting approach (embed Avalonia Headless renderer vs spawning compiled app)
- [ ] 1.2 Define output directory convention and retention policy
- [ ] 1.3 Document timeout, memory, and process limits for screenshot runs
- [ ] 1.4 Map Avalonia Headless capabilities (window sizing, DPI, input simulation, visual diagnostics) to exposed tool options

## 2. Core Implementation
- [ ] 2.1 Add new MCP tool (e.g., `DebugScreenshotTool`) that accepts project path or XAML
- [ ] 2.2 Implement headless launch pipeline using `Avalonia.Headless` and `AppBuilder` with `ImmediateRenderer`
- [ ] 2.3 Capture rendered visual via `RenderTargetBitmap` and encode to PNG
- [ ] 2.4 Persist screenshots to configurable directory and return base64 payload
- [ ] 2.5 Ensure processes/temporary files are cleaned up on success and failure
- [ ] 2.6 Expose optional parameters for window size/DPI and pass through to headless APIs
- [ ] 2.7 Provide hooks for optional pointer/keyboard event simulation prior to capture

## 3. Validation & Safety
- [ ] 3.1 Extend `InputValidationService` for project path and XAML size limits
- [ ] 3.2 Add timeout / cancellation handling for hung processes
- [ ] 3.3 Guard against launching non-Avalonia assemblies
- [ ] 3.4 Integrate telemetry events for attempts and outcomes
- [ ] 3.5 Verify headless feature set is only available when `Avalonia.Headless` package is present

## 4. Testing
- [ ] 4.1 Add unit tests for new validation helpers
- [ ] 4.2 Add integration test using a minimal Avalonia sample app
- [ ] 4.3 Verify screenshot files are created with unique names
- [ ] 4.4 Simulate failure scenarios (build error, invalid XAML, timeout)
- [ ] 4.5 Test optional window sizing/DPI and input simulation pathways

## 5. Documentation
- [ ] 5.1 Document tool usage in `docs/tools/debug-screenshot.md`
- [ ] 5.2 Update README quick start / troubleshooting with screenshot guidance
- [ ] 5.3 Add configuration notes for output directory and timeouts
- [ ] 5.4 Reference Avalonia Headless docs and enumerate supported headless features/options
