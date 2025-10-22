<!-- OPENSPEC:START -->
# OpenSpec Instructions

These instructions are for AI assistants working in this project.

Always open `@/openspec/AGENTS.md` when the request:
- Mentions planning or proposals (words like proposal, spec, change, plan)
- Introduces new capabilities, breaking changes, architecture shifts, or big performance/security work
- Sounds ambiguous and you need the authoritative spec before coding

Use `@/openspec/AGENTS.md` to learn:
- How to create and apply change proposals
- Spec format and conventions
- Project structure and guidelines

Keep this managed block so 'openspec update' can refresh the instructions.

<!-- OPENSPEC:END -->

# Repository Guidelines
Welcome, agents! Use this guide to get productive quickly while keeping AvaloniaUI.MCP consistent and stable.

## Project Structure & Module Organization
- `src/AvaloniaUI.MCP/` hosts the .NET 9 MCP host with `Data/`, `Services/`, `Tools/`, `Prompts/`, and `Resources/` mirroring runtime domains.
- `tests/AvaloniaUI.MCP.Tests/` contains MSTest suites covering tools, telemetry, accessibility, and theming.
- `docs/`, `_layouts/`, `_sass/`, and `assets/` power the public Jekyll site; keep narrative updates with feature work.
- `Directory.Build.*` and `AvaloniaUI.MCP.slnx` centralize build settings and version management; edit these when adding projects or packages.

## Build, Test, and Development Commands
- `dotnet restore` pulls NuGet dependencies via `nuget.config`.
- `dotnet build AvaloniaUI.MCP.slnx` compiles all projects with shared analyzers enabled.
- `dotnet run --project src/AvaloniaUI.MCP/AvaloniaUI.MCP.csproj` launches the local MCP agent host.
- `dotnet test` executes the MSTest suite; add `--collect:"XPlat Code Coverage"` when validating coverage.
- `dotnet format` enforces `.editorconfig` rules before submitting changes.

## Coding Style & Naming Conventions
- C# uses 4-space indentation, file-scoped namespaces, and sorted usings with `System` directives first.
- Avoid `var` unless type inference improves clarity; explicit types are the default per `.editorconfig`.
- Follow naming rules: PascalCase for types and members, `_camelCase` for private fields, interfaces prefixed with `I`.
- Keep prompt and tool descriptors grouped under matching folder names; indent JSON/YAML with two spaces.

## Testing Guidelines
- MSTest attributes (`[TestClass]`, `[TestMethod]`) live under `tests/AvaloniaUI.MCP.Tests`; mirror production namespaces.
- Name files `<UnitUnderTest>Tests.cs` and methods `MethodName_State_Result`; annotate edge-case tests with comments when intent is non-obvious.
- Run `dotnet test --filter Category=Unit` for focused suites once categories are introduced; escalate to full `dotnet test` before pushing.
- Capture coverage locally with `dotnet test --collect:"XPlat Code Coverage"` when features touch critical services.

## Commit & Pull Request Guidelines
- Current history is sparse (`Initial commit`), so write imperative, scoped messages (e.g., `Add telemetry sampling guard`), wrapping at 72 characters.
- Reference issues or discussions in the body, summarize testing (`dotnet test`) results, and include UX screenshots for UI-related changes.
- Pull requests should describe intent, list follow-up tasks, request review from a maintainer, and highlight breaking changes or new MCP tools.

## Agent-Specific Tips
- Synchronize updates across `Tools/`, `Prompts/`, and `Resources/` so metadata, scripts, and localized assets stay aligned.
- When adding dependencies, update `Directory.Packages.props` and regenerate documentation snippets in `docs/` to keep the public site accurate.
