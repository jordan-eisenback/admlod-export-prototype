# Changelog

## [v1.0.0] - 2025-10-06

### 🎉 Initial Creation
- Project initialized with Git version control and remote repository setup.
- Folder structure organized: `/src`, `/docs`, `/tests`, `/MockFiles`.

### 🚀 Core Functionality
- CLI-based export tool built using .NET 6.0.
- Generates ADMLOD export files from mock data.
- Simulates FTP upload by writing locally and logging actions.
- Parses mock DTN response files for success and error scenarios.
- Supports logging to console and optional log file.

### 🧪 Testing & Validation
- Automated test cases created using xUnit/NUnit.
- Validated CLI export success and error scenarios.
- Simulated FTP failures and malformed command handling.
- Verified logging behavior and CLI option parsing.
- All test cases documented and executed in 05.Test Plan.docx.

### 📘 Documentation
- Runbook finalized with build, run, and troubleshooting instructions in 08.Runbook.docx.
- Coding standards and best practices documented in 06.Coding Guidelines.docx.
- Test summary report generated (retrieval pending).
- README.md completed.

### 🔐 Security & Compliance
- Confirmed no real credentials or sensitive data used.
- All file operations sandboxed.
- Mock data verified for safe internal sharing.

### 🛠 Configuration & Maintenance
- Modular code structure with clear separation of concerns.
- Logging centralized with timestamps and severity levels.
- Mock data customizable via `MockData.cs` and `/MockFiles`.

### 📤 Stakeholder Review
- Demoed success and error scenarios.
- Documentation and test results shared for sign-off.