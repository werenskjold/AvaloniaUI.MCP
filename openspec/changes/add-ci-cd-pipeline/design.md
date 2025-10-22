# CI/CD Pipeline Design

## Context
The project currently has no automated CI/CD despite claiming build status in README. Need comprehensive automation for testing, documentation verification, coverage reporting, and releases.

## Goals / Non-Goals

### Goals
- Automated testing on every PR with multi-platform support
- Code coverage tracking and reporting
- Documentation completeness verification
- Automated release creation with semantic versioning
- Security and vulnerability scanning
- Fast feedback loop (< 5 minutes for most checks)

### Non-Goals
- Deployment automation (server runs locally via STDIO)
- Performance benchmarking (can be added later)
- Automated screenshot testing
- Multi-stage deployment pipelines

## Decisions

### Decision: GitHub Actions over alternatives
Use GitHub Actions exclusively for CI/CD.

**Rationale:**
- Native GitHub integration
- Free for public repositories
- Good .NET support
- Familiar to most contributors
- Extensive marketplace of actions

**Alternatives considered:**
- Azure Pipelines: Overkill for this project
- CircleCI/Travis: Additional service to manage
- Jenkins: Self-hosted complexity

### Decision: Matrix Build for Multi-Platform Testing
Run tests on Windows, macOS, and Linux using matrix strategy.

```yaml
strategy:
  matrix:
    os: [ubuntu-latest, windows-latest, macos-latest]
    dotnet-version: ['9.0.x']
```

**Rationale:**
- AvaloniaUI is cross-platform
- Catches platform-specific bugs early
- Verifies MCP server works everywhere
- Minimal additional cost (parallel execution)

### Decision: Coverlet for Code Coverage
Use coverlet for collecting coverage data and ReportGenerator for reports.

**Rationale:**
- Standard .NET coverage tool
- Works well with xUnit/MSTest
- Supports multiple output formats
- Easy GitHub Actions integration

### Decision: Two Separate Workflows
Create separate workflows for CI and releases.

**ci.yml**: Runs on every PR and push
- Build verification
- Test execution
- Coverage collection
- Documentation checks
- Security scanning

**release.yml**: Runs on version tags
- Build release artifacts
- Generate changelog
- Create GitHub release
- Publish packages

**Rationale:**
- Clear separation of concerns
- Faster PR feedback (skip release steps)
- Different trigger conditions
- Easier to maintain

## Technical Approach

### CI Workflow Structure
```yaml
name: CI

on:
  push:
    branches: [ main ]
  pull_request:
    branches: [ main ]

jobs:
  build-and-test:
    runs-on: ${{ matrix.os }}
    strategy:
      matrix:
        os: [ubuntu-latest, windows-latest, macos-latest]

    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '9.0.x'

      - name: Restore dependencies
        run: dotnet restore

      - name: Build
        run: dotnet build --no-restore

      - name: Test
        run: dotnet test --no-build --collect:"XPlat Code Coverage"

      - name: Upload coverage
        uses: codecov/codecov-action@v3

  documentation:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - name: Verify tool documentation
        run: |
          # Check each tool has corresponding .md file
          # Fail if missing
```

### Coverage Integration Options
1. **Codecov** - Popular, free for open source
2. **Coveralls** - Alternative to Codecov
3. **GitHub comment** - Post coverage report directly in PR

**Decision**: Use Codecov for badge + GitHub comment for inline feedback

### Documentation Verification Script
Create a verification script that:
1. Scans `src/AvaloniaUI.MCP/Tools/` for all `*Tool.cs` files
2. Checks `docs/tools/` for corresponding `.md` files
3. Reports missing documentation
4. Optionally runs the doc generator in verify mode

## Risks / Trade-offs

### Risk: CI costs for private repo
- **Mitigation**: Currently public, free
- **Mitigation**: Optimize workflow to complete quickly

### Risk: Matrix builds slow down feedback
- **Mitigation**: Run Linux build first, others in parallel
- **Mitigation**: Use caching for dependencies

### Trade-off: Coverage threshold enforcement
- **Pro**: Maintains code quality
- **Con**: Can block valid PRs
- **Decision**: Warn at 80%, fail only if drops >10%

### Risk: Documentation verification too strict
- **Mitigation**: Allow temporary bypass with PR labels
- **Mitigation**: Clear instructions on how to fix

## Migration Plan

### Phase 1: Basic CI
1. Create .github/workflows/ci.yml
2. Add build and test jobs
3. Configure multi-platform matrix
4. Test with draft PR

### Phase 2: Coverage
1. Add coverage collection
2. Set up Codecov integration
3. Add coverage badge to README
4. Configure thresholds

### Phase 3: Documentation Verification
1. Create verification script
2. Add to CI workflow
3. Document process
4. Fix any missing docs

### Phase 4: Release Automation
1. Create release.yml workflow
2. Test with tag on branch
3. Configure changelog generation
4. Document release process

### Phase 5: Security Scanning
1. Enable CodeQL
2. Add dependency scanning
3. Configure security policies
4. Set up notifications

## Open Questions

1. Should CI publish NuGet packages or just create releases?
   - **Tentative**: GitHub releases only for now, NuGet later if needed

2. Should we require all platforms to pass or allow platform-specific failures?
   - **Tentative**: All must pass for merge

3. How strict should documentation verification be?
   - **Tentative**: Fail CI if any tool lacks docs, but allow override label

4. Should we add performance benchmarks?
   - **Tentative**: Not in initial implementation, add later if needed
