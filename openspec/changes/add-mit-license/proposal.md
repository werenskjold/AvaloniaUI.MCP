# Add MIT License File

## Why
The README.md states "This project is licensed under the MIT License" and includes an MIT license badge, but there is no LICENSE file in the repository. This creates legal ambiguity and doesn't follow open source best practices. Users and contributors need a clear, legally-binding license file.

## What Changes
- Add LICENSE file with full MIT License text
- Include copyright holder information
- Add year and maintainer details
- Update documentation references to point to LICENSE file
- Ensure license is discoverable by GitHub and package managers

## Impact
- Affected specs: project-licensing (new)
- Affected code:
  - New file: LICENSE
  - Potential updates to package metadata if/when published to NuGet
- Breaking change: NO (clarifies existing MIT license intent)
- Users and contributors will have legal clarity about usage rights
