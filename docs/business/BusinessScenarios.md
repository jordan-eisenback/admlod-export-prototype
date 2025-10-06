# Business Scenarios – ADMLOD Export Prototype

This document outlines example scenarios that simulate real-world business conditions using the ADMLOD Export Prototype. These help developers, testers, and stakeholders validate functionality and ensure alignment with business rules.

## Scenario 1: Successful Product and Group Export

**Context:**  
A fuel distributor wants to validate that their product and product group data can be exported in ADMLOD format before sending to DTN.

**Steps:**  
1. Run: `dotnet run export --success`  
2. Review `/MockFiles/adm_export.txt` and `adm_response_success.txt`  
3. Check console or log output for success messages

**Expected Outcome:**  
- Export file is generated with correct structure  
- FTP upload is simulated successfully  
- DTN response shows all commands succeeded  
- Log confirms test passed

**Business Value:**  
Ensures readiness for live integration and confirms that mock data meets ADMLOD standards.

## Scenario 2: Missing Required Field in Product

**Context:**  
A QA analyst wants to test how the system handles incomplete product data, such as a missing product code.

**Steps:**  
1. Modify `MockData.cs` to omit `ProductCode` for one product  
2. Run: `dotnet run export --error`  
3. Review `adm_response_error.txt` and logs

**Expected Outcome:**  
- Export file includes malformed product line  
- DTN response flags the error  
- Log shows validation failure and error categorization

**Business Value:**  
Validates error handling and ensures that incomplete data is caught before integration.

## Scenario 3: Simulated FTP Upload Failure

**Context:**  
A developer wants to test how the system behaves when FTP upload fails due to network or configuration issues.

**Steps:**  
1. Run: `dotnet run export --error` with FTP failure logic enabled  
2. Observe console output and log file

**Expected Outcome:**  
- Export file generation is attempted  
- FTP upload fails (simulated)  
- Process aborts before response parsing  
- Error is logged with severity and correlation ID

**Business Value:**  
Ensures robustness of the CLI tool and confirms that failures are logged and handled gracefully.

## Scenario 4: Invalid Command Format

**Context:**  
A stakeholder wants to verify that malformed ADMLOD commands are detected and reported.

**Steps:**  
1. Modify `AdmLodFileGen.cs` to produce an invalid command line  
2. Run: `dotnet run export --error`  
3. Review response file and logs

**Expected Outcome:**  
- Export file contains invalid command  
- DTN response flags the error  
- Log shows parsing failure and test result as “Passed” (error expected)

**Business Value:**  
Confirms that the prototype enforces ADMLOD syntax rules and supports error simulation.


## Scenario 5: Review of Output Files for Audit

**Context:**  
A business stakeholder wants to review the export file and response output for traceability and audit purposes.

**Steps:**  
1. Run: `dotnet run export --success --logfile audit.log`  
2. Inspect `adm_export.txt`, `adm_response_success.txt`, and `audit.log`

**Expected Outcome:**  
- All files are generated and stored in `/MockFiles`  
- Log includes timestamps, severity, and correlation ID  
- Outputs match business expectations

**Business Value:**  
Supports audit readiness and stakeholder confidence in the prototype’s behavior.


## Notes

- These scenarios are designed for sandbox testing only.
- All integrations are mocked—no live FTP or DTN systems are involved.
- Response files can be edited to simulate additional business rule violations.