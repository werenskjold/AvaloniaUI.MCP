# Project Generation Specification

## ADDED Requirements

### Requirement: Consistent File Operations
Project generation tools SHALL use consistent file operation patterns across all project templates.

#### Scenario: Synchronous file writing
- **WHEN** generating any project template (MVVM, Basic, or CrossPlatform)
- **THEN** all file write operations SHALL use synchronous File.WriteAllText
- **AND** NOT use async operations that are immediately blocked with .Wait()
- **AND** follow the same error handling pattern

#### Scenario: Directory validation
- **WHEN** writing files to a project directory
- **THEN** the tool SHALL validate parent directories exist before writing
- **AND** create missing directories as needed
- **AND** provide clear error messages if directory creation fails

### Requirement: Robust Error Handling
Project generation SHALL handle file system errors gracefully with clear error messages.

#### Scenario: Directory already exists
- **WHEN** creating a project in a directory that already exists
- **THEN** the tool SHALL fail with error message: "Directory '{path}' already exists"
- **AND** suggest using a different path or deleting the existing directory
- **AND** NOT partially create files

#### Scenario: Permission denied
- **WHEN** file write fails due to insufficient permissions
- **THEN** the tool SHALL catch the exception
- **AND** return error message indicating permission issue
- **AND** suggest checking directory permissions

#### Scenario: Disk space error
- **WHEN** file write fails due to insufficient disk space
- **THEN** the tool SHALL catch the exception
- **AND** return error message indicating disk space issue
- **AND** suggest freeing up space

### Requirement: Atomic Project Creation
Project generation SHALL either fully succeed or fully fail without leaving partial project files.

#### Scenario: Successful project creation
- **WHEN** generating a project completes successfully
- **THEN** all required files SHALL be written
- **AND** all directories SHALL be created
- **AND** the project SHALL be immediately buildable

#### Scenario: Failed project creation cleanup
- **WHEN** an error occurs during project generation
- **THEN** the tool SHALL attempt to clean up any partially created files
- **AND** leave the file system in original state
- **AND** report what was cleaned up in the error message

#### Scenario: Pre-validation
- **WHEN** starting project generation
- **THEN** the tool SHALL validate all inputs before creating any files
- **AND** check for available disk space
- **AND** verify write permissions on target directory
