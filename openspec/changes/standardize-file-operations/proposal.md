# Standardize File Operations in Project Generator

## Why
The ProjectGeneratorTool uses inconsistent file writing approaches across different project templates:
- `GenerateMvvmProject` uses async `AsyncFileService.WriteAllFilesAsync` but immediately blocks with `.Wait()` (lines 92-94)
- `GenerateBasicProject` uses synchronous `File.WriteAllText` (lines 113-134)
- `GenerateCrossPlatformProject` uses synchronous `File.WriteAllText` (lines 150-185)

This inconsistency is confusing and the async approach provides no benefit since MCP tools must be synchronous and the code blocks immediately with `.Wait()`. The hybrid approach creates unnecessary complexity.

## What Changes
- **BREAKING**: Standardize all file operations to use synchronous `File.WriteAllText`
- Remove usage of `AsyncFileService.WriteAllFilesAsync` in project generation
- Update `AsyncFileService` documentation to clarify when it should be used
- Add comprehensive error handling for file operations
- Add validation to ensure parent directories exist before writing
- Consider keeping `AsyncFileService` for resource loading scenarios where async is beneficial

## Impact
- Affected specs: project-generation (new)
- Affected code:
  - src/AvaloniaUI.MCP/Tools/ProjectGeneratorTool.cs (all three Generate* methods)
  - src/AvaloniaUI.MCP/Services/AsyncFileService.cs (documentation updates)
- Breaking change: NO (internal implementation only, external behavior unchanged)
- Code will be more consistent, maintainable, and easier to understand
