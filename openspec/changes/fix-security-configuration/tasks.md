# Implementation Tasks

## 1. Code Changes
- [ ] 1.1 Update Program.cs to read Sentry DSN from environment variable
- [ ] 1.2 Add conditional logic to skip Sentry if DSN not provided
- [ ] 1.3 Add validation for DSN format if provided
- [ ] 1.4 Update logging to indicate Sentry status (enabled/disabled)

## 2. Documentation
- [ ] 2.1 Create .env.example file with SENTRY_DSN template
- [ ] 2.2 Update README.md with environment variable configuration
- [ ] 2.3 Update CLAUDE.md to reflect configuration approach
- [ ] 2.4 Add troubleshooting section for Sentry configuration

## 3. Testing
- [ ] 3.1 Test server startup with SENTRY_DSN set
- [ ] 3.2 Test server startup without SENTRY_DSN (should work)
- [ ] 3.3 Test with invalid DSN format (should log warning and skip)
- [ ] 3.4 Verify telemetry service still works without Sentry
