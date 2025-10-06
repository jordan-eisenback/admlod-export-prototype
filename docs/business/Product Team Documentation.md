# ADMLOD Export Simulator – Product Team Documentation

## Overview

The ADMLOD Export Simulator is a command-line tool designed to simulate the generation, upload, and validation of ADMLOD export files. It supports both success and error scenarios to help product teams validate business logic, response formats, and error handling workflows without relying on live systems.

## Purpose

- Validate export file structure before integration with external systems.
- Simulate FTP upload behavior, including failure scenarios.
- Parse and verify response files to ensure business rules are enforced.
- Log all activity for traceability and audit purposes.

## Key Capabilities

| Feature                  | Description                                                                 |
| :----------------------: |:--------------------------------------------------------------------------: | 
| Export File Generation   | Creates a mock ADMLOD export file with predefined content.                  | 
| FTP Upload Simulation    | Mimics FTP upload success or failure based on CLI flags.                    |
| Response File Parsing    | Reads and interprets success/error responses from mock files.               |
| Test Validation          | Automatically checks if the response matches expected outcomes.             |
| Logging                  | Outputs timestamped logs to console and optional file.                      |

## How to Use

### Success Scenario

Generates export file
Simulates successful FTP upload
Parses adm_response_success.txt
Logs results to logfile01_success

### Error Scenario
dotnet run export --error --logfile logfile02_error

Generates export file
Simulates FTP failure
Parses adm_response_error.txt
Logs results to logfile02_error

### File Outputs

| File | Purpose |
|adm_export.txt | The generated export file |
|adm_response_success.txt | Mock response for successful upload |
|adm_response_error.txt | Mock response with validation errors
|logfileXX | Timestamped log of all actions|

##Business Validation Logic

Success Count and Error Count are extracted from the response file.
The tool checks if the response matches the expected outcome:
Success scenario must have 0 errors
Error scenario must have ≥1 errors

If the counts match expectations, the test is marked as passed.

## Example Log Output
2025-10-01 09:42:28 [INFO] Generating ADMLOD export file...
2025-10-01 09:42:28 [SUCCESS] Export file successfully written to MockFiles\adm_export.txt
2025-10-01 09:42:28 [SUCCESS] Simulated FTP upload completed.
2025-10-01 09:42:29 [INFO] Total Successes: 7
2025-10-01 09:42:29 [WARN] Total Errors: 0
2025-10-01 09:42:29 [SUCCESS] Test Passed: Response counts match expectations.

## Notes for Product Stakeholders

This tool is not connected to live systems — it is safe for sandbox testing.
Response files can be edited to simulate different business rule violations.
Logs provide a clear audit trail for each test run.
Ideal for UAT, QA, and business rule validation before deployment.