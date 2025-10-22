# Debug Screenshot Capture Specification

## ADDED Requirements

### Requirement: Headless App Launch & Capture
The MCP server SHALL provide a capability to launch an Avalonia application in a controlled, non-interactive environment using the Avalonia Headless platform and capture a screenshot.

#### Scenario: Screenshot from project path
- **WHEN** a user invokes the debug screenshot tool with a filesystem path to an Avalonia project
- **THEN** the server SHALL build (if necessary) and launch the application using `Avalonia.Headless` with the immediate renderer enabled
- **AND** render the default window without opening a visible UI
- **AND** capture a bitmap of the rendered root visual
- **AND** return the screenshot as a file path on disk and a base64-encoded payload

#### Scenario: Screenshot from XAML sample
- **WHEN** a user supplies raw XAML markup instead of a project path
- **THEN** the server SHALL host the XAML within a temporary Avalonia `Window` built on the headless platform
- **AND** render it off-screen to produce a screenshot using the same output format

#### Scenario: Failed launch
- **WHEN** the application fails to launch or throws during rendering
- **THEN** the tool SHALL terminate the headless process
- **AND** return a descriptive error payload with captured stdout/stderr logs
- **AND** clean up any temporary files that were created

### Requirement: Output Management
Captured screenshots SHALL be written to a configured output directory to keep artifacts organized.

#### Scenario: Output directory configuration
- **WHEN** the screenshot tool runs for the first time
- **THEN** it SHALL create a `/tmp/avalonia-mcp/screenshots` (or configurable) directory if it does not exist
- **AND** filenames SHALL be timestamped and unique to avoid collisions
- **AND** the response SHALL include both the absolute path and a short-lived base64 string

### Requirement: Security & Resource Limits
The screenshot workflow SHALL guard against malicious input and runaway processes.

#### Scenario: Execution limits
- **WHEN** the launched process exceeds a configurable timeout (default 20 seconds)
- **THEN** the tool SHALL kill the process and return a timeout error
- **AND** log the event through `ITelemetryService`

#### Scenario: Input validation
- **WHEN** paths or XAML input fail validation (e.g., unsupported scheme, excessive size)
- **THEN** the tool SHALL refuse execution using `ErrorHandlingService` with actionable guidance

### Requirement: Telemetry & Logging
All screenshot attempts SHALL be observable.

#### Scenario: Telemetry events
- **WHEN** a screenshot is attempted
- **THEN** the tool SHALL record a telemetry event including duration, success flag, trigger type (project vs XAML), and output path
- **AND** include failure reason when applicable

### Requirement: Headless Feature Parity
The screenshot tool SHALL expose the relevant capabilities provided by Avalonia Headless for automated UI validation.

#### Scenario: Window sizing & DPI control
- **WHEN** a caller specifies desired window dimensions or DPI
- **THEN** the headless renderer SHALL respect those settings using `HeadlessWindowResize`/`HeadlessRenderingScaling`
- **AND** document the defaults when callers omit them

#### Scenario: Input simulation
- **WHEN** tests opt-in to simulate pointer or keyboard events before capturing
- **THEN** the tool SHALL wire through Avalonia Headless input APIs so callers can queue pointer moves, clicks, and key presses supported by the platform

#### Scenario: Render hooks
- **WHEN** callers request layout diagnostics
- **THEN** the tool SHALL surface headless-only metadata (e.g., measured size, visual tree) alongside the screenshot to match the capabilities described in the Avalonia Headless documentation
