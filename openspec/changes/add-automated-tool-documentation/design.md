# Automated Tool Documentation Generation Design

## Context
The project has 15+ MCP tools but only 2 have documentation. Manual documentation maintenance is error-prone and doesn't scale. We need automated generation that stays synchronized with code changes.

## Goals / Non-Goals

### Goals
- Automatically generate complete documentation for all MCP tools
- Extract information directly from code attributes to ensure accuracy
- Integrate into build/CI pipeline for continuous verification
- Maintain consistent documentation structure across all tools
- Support both generated and manually-enhanced documentation

### Non-Goals
- Generate code examples automatically (too complex, requires manual curation)
- Replace manually-written tutorials or guides
- Generate API reference for non-tool classes
- Support multiple output formats (focus on markdown only)

## Decisions

### Decision: Separate Console Application
Create a standalone console application (e.g., AvaloniaUI.MCP.DocGen) rather than adding to the main server or using a build task.

**Rationale:**
- Clean separation of concerns
- Can reference the main project to discover tools via reflection
- Easy to run manually or in CI
- Doesn't bloat the main server with doc generation dependencies

**Alternatives considered:**
- MSBuild task: Too complex, harder to debug
- MCP tool itself: Circular dependency, unnecessary
- Build script: Limited reflection capabilities

### Decision: Reflection-Based Discovery
Use .NET reflection to discover all MCP tools and their attributes at runtime.

**Rationale:**
- Single source of truth (code attributes)
- Automatically detects new tools
- Can extract all metadata including descriptions and parameter info
- No manual registration required

### Decision: Template + Generation Hybrid
Generate standard sections automatically, but support manual enhancement through separate files.

**Structure:**
- `tool-name.md` - Fully generated, overwritten each time
- `tool-name.examples.md` - Manual examples (not overwritten)
- Generator includes manual examples if file exists

**Rationale:**
- Keeps automation benefits while allowing human touch
- Prevents loss of manually-written content
- Clear separation between generated and curated content

## Technical Approach

### Tool Discovery
```csharp
// Scan for all types with [McpServerToolType]
var toolTypes = Assembly.GetAssembly(typeof(EchoTool))
    .GetTypes()
    .Where(t => t.GetCustomAttribute<McpServerToolTypeAttribute>() != null);

// For each type, find methods with [McpServerTool]
foreach (var type in toolTypes)
{
    var toolMethods = type.GetMethods()
        .Where(m => m.GetCustomAttribute<McpServerToolAttribute>() != null);
}
```

### Markdown Generation
```csharp
// Extract information
var toolName = method.Name;
var description = method.GetCustomAttribute<DescriptionAttribute>()?.Description;
var parameters = method.GetParameters();

// Generate sections
var markdown = new StringBuilder();
markdown.AppendLine($"# {toolName}");
markdown.AppendLine();
markdown.AppendLine($"## Overview");
markdown.AppendLine(description);
// ... more sections
```

### Integration Points
1. **Manual run**: `dotnet run --project tools/AvaloniaUI.MCP.DocGen`
2. **Pre-build**: Add as project dependency or pre-build event
3. **CI verification**: `dotnet run --project tools/AvaloniaUI.MCP.DocGen --verify-only`

## Risks / Trade-offs

### Risk: Breaking changes to attributes
- **Mitigation**: Version the generator with the main project
- **Mitigation**: Add tests for generator itself

### Risk: Generated docs lack context
- **Mitigation**: Support manual examples and notes
- **Mitigation**: Encourage good descriptions in code attributes

### Trade-off: Generated files in git
- **Pro**: Documentation visible in GitHub, works offline
- **Con**: Merge conflicts, larger diffs
- **Decision**: Commit generated files, regenerate pre-commit

## Migration Plan

### Phase 1: Initial Implementation
1. Create DocGen console application
2. Implement reflection-based discovery
3. Generate basic markdown files
4. Test with existing 2 documented tools

### Phase 2: Full Generation
1. Generate documentation for all 15+ tools
2. Review and enhance code attributes as needed
3. Add manual examples where valuable
4. Update main README with new documentation links

### Phase 3: CI Integration
1. Add DocGen to CI pipeline
2. Verify docs are up-to-date on every PR
3. Add pre-commit hook suggestion
4. Document the process for contributors

### Rollback
If issues arise:
1. Generated files can be deleted without breaking functionality
2. Manual documentation can be restored from git history
3. Generator can be removed without affecting server

## Open Questions

1. Should we generate resource and prompt documentation too?
   - **Tentative**: Yes, use same approach for consistency

2. Should generated files be gitignored and generated at build time?
   - **Tentative**: No, commit them for GitHub visibility

3. How detailed should parameter documentation be?
   - **Tentative**: Include type, description, default value, constraints from validation

4. Should we include performance characteristics in docs?
   - **Tentative**: Add manually where relevant (execution time, caching, etc.)
