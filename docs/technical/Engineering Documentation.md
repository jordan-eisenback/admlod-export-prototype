# ADMLOD Export Simulator – Engineering Documentation

## Overview

The ADMLOD Export Simulator is a .NET CLI tool designed to simulate the generation, upload, and validation of ADMLOD export files. It supports controlled success and failure scenarios to facilitate testing of downstream systems and business rule enforcement.

This tool is intended for use in development, QA, and UAT environments. It is not connected to live systems and does not perform actual FTP transfers.

## Architecture

### Components

- **Program.cs**: Entry point. Handles CLI parsing, logging setup, and orchestration.
- **AdmLodFileGen**: Generates mock export file content.
- **FtpClientMock**: Simulates FTP upload behavior (success or failure).
- **ResponseParser**: Parses mock response files and extracts success/error counts.
- **Logger**: Centralized logging utility with support for console and file output.

### Flow Diagram


[CLI Input]
     ↓
[Program.cs]
     ↓
[AdmLodFileGen] → [FtpClientMock] → [ResponseParser]
     ↓
   [Logger]

## CLI Usage
dotnet run export [--success|--error] [--logfile <filename>]

### lags

--success: Simulates a successful FTP upload and parses adm_response_success.txt.
--error: Simulates a failed FTP upload and parses adm_response_error.txt.
--logfile <filename>: Optional. Writes logs to the specified file.

## File Structure
AdmLodPrototype/
├── Program.cs
├── src/
│   ├── AdmLodFileGen.cs
│   ├── FtpClientMock.cs
│   ├── ResponseParser.cs
│   └── Logger.cs
├── MockFiles/
│   ├── adm_export.txt
│   ├── adm_response_success.txt
│   └── adm_response_error.txt

## Logging
Logs are written to both console and optionally to a file. Each log entry includes:

Timestamp
Severity (INFO, SUCCESS, WARN, ERROR)
Message

Example:
2025-10-01 09:42:28 [INFO] Generating ADMLOD export file...
2025-10-01 09:42:28 [SUCCESS] Export file successfully written to MockFiles\\adm_export.txt

### Response Validation Logic

Parses response file line-by-line.
Counts lines marked as success or error.
Validates against expected outcome:

Success scenario: ErrorCount == 0
Error scenario: ErrorCount > 0

Logs test result as pass/fail.

## Design Considerations
### Strengths
Simple CLI interface for quick testing.
Modular components for file generation, upload simulation, and parsing.
Centralized logging for traceability.

### Limitations
No dependency injection; static classes reduce testability.
CLI parsing is manual and fragile.
Hardcoded file paths and response formats.
No unit tests or CI integration.

## Recommendations for Refactoring
Introduce interfaces for IFtpUploader, IResponseParser, etc.
Replace static Logger with injectable logging abstraction.
Use a CLI parsing library (System.CommandLine, Spectre.Console) for robustness.
Add unit tests for each module.
Externalize file paths and formats to configuration.

## Future Enhancements
Support dynamic response file generation.
Add schema validation for export and response files.
Integrate with CI pipelines for automated regression testing.
Extend to support additional export formats or protocols.