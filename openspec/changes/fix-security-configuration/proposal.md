# Fix Security Configuration

## Why
The Sentry DSN (Data Source Name) is currently hardcoded in Program.cs:22, exposing project configuration in source code. While DSNs are technically public information, this creates security and operational issues:
- Allows unauthorized parties to send events to the Sentry account
- Can cause quota exhaustion or noise in error tracking
- Violates security best practices for secrets management
- Makes it difficult to use different Sentry projects for dev/staging/prod

## What Changes
- Move Sentry DSN from hardcoded string to environment variable
- Make Sentry configuration optional (skip if DSN not provided)
- Update documentation to explain environment variable configuration
- Add example configuration file showing required environment variables

## Impact
- Affected specs: server-configuration (new)
- Affected code: src/AvaloniaUI.MCP/Program.cs lines 20-31
- Breaking change: NO (defaults to skip Sentry if not configured)
- Migration: Users must set SENTRY_DSN environment variable to continue using Sentry integration
