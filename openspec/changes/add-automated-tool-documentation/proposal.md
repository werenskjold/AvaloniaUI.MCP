# Add Automated Tool Documentation Generation

## Why
Currently, only 2 out of 15+ tool documentation files exist (project-generator.md and README.md). Manually maintaining documentation for each tool is error-prone and time-consuming, leading to incomplete or outdated documentation. The project needs an automated way to generate and maintain tool documentation from source code attributes.

## What Changes
- Create a documentation generator tool that reads MCP tool attributes
- Generate markdown documentation for each tool automatically
- Extract descriptions, parameters, and usage examples from code
- Add a build step to regenerate documentation
- Create templates for consistent documentation structure
- Add verification to ensure all tools have documentation

## Impact
- Affected specs: tool-documentation (new)
- Affected code:
  - New file: src/AvaloniaUI.MCP/Tools/DocumentationGenerator.cs (or separate console app)
  - Build configuration updates for doc generation
  - All docs/tools/*.md files will be regenerated
- Breaking change: NO (documentation only, may replace existing manual docs)
- Users will have complete, accurate, and up-to-date documentation for all tools
