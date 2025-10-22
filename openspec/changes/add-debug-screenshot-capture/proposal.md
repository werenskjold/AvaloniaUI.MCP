# Add Debug Screenshot Capture Tool

## Why
Developers asked for a way to launch Avalonia applications through the MCP server and capture screenshots for debugging and review. The current toolset only returns textual guidance; there is no mechanism to render UI or verify visual output. Leveraging the Avalonia Headless platform (per [docs](https://docs.avaloniaui.net/docs/concepts/headless/)) will let contributors validate themes, layouts, and generated code samples without leaving their MCP client while keeping the workflow deterministic and test-friendly.

## What Changes
- Add a new MCP tool that can run Avalonia windows via `Avalonia.Headless` and capture PNG screenshots
- Support both existing project builds and ad-hoc XAML snippets
- Store screenshots in a predictable directory and return base64 strings for immediate preview
- Surface headless-only capabilities (window sizing, DPI, input simulation, visual diagnostics) so automated tests can exercise the full feature set
- Enforce timeouts, validation, and telemetry so the feature is safe to use in automated environments
- Document configuration, usage, and troubleshooting steps for the new workflow

## Impact
- Affected specs: debug-screenshot (new)
- Affected code:
  - New tool class under `src/AvaloniaUI.MCP/Tools/`
  - Possible additions to `Services/` for rendering helpers and configuration/constants
  - Updates to `InputValidationService`, `TelemetryService`, and related tests
- Breaking change: NO (feature is additive; defaults keep existing behaviour)
- Requires `Avalonia.Headless` packages during build/test environments
