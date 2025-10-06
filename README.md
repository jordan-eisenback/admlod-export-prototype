README.md
Markdown# ADMLOD Export Prototype – Integration Framework
## Overview
This prototype simulates integration with the DTN ADMLOD FTP interface. It generates ADMLOD-compliant export files for Products and Product Groups using mock data, simulates FTP upload, and parses mock DTN response files. It supports both success and error scenarios and is designed as a reusable framework for integration prototypes across teams.

## SPARC Framework 
AlignmentPhasePurposeKey ArtifactsSpecifyDefine business goals and rulesBusinessContext.md, 01.Business Requirements Specification.docx| Plan        | Design functional and technical behavior     | FunctionalSpecification.docx, TechnicalSpecification.docx      || Act         | Build and simulate integration               | Source code, Runbook.docx                                      || Review      | Validate outputs and test coverage           | Test Plan.docx, TestSummaryReport.md                           || Communicate | Share with stakeholders and other teams      | README.md, BusinessScenarios.md, Personas.md                   |
---## Getting Started for Developers

###  1. Understand the Business Context- Read `/docs/business/BusinessContext.md`- Review `/docs/business/Glossary.md` and `/docs/business/Personas.md`
### 2. Set Up Locally- Install https://dotnet.microsoft.com/download- Clone the repo and navigate to the root- Run `dotnet build`---## ⚙️ Configuration SupportThe prototype supports external configuration via `appsettings.json`, allowing flexible control over paths, logging, and feature flags.

### Sample `appsettings.json`{  "Paths": {    "ExportFile": "MockFiles/adm_export.txt",    "ResponseSuccessFile": "MockFiles/adm_response_success.txt",    "ResponseErrorFile": "MockFiles/adm_response_error.txt",    "LogFile": "MockFiles/export.log"  },  "Logging": {    "EnableFileLogging": true,    "LogLevel": "INFO"  },  "FeatureFlags": {    "SimulateFtpFailure": false,    "EnableDynamicResponse": true  }}

Notes:
CLI flags override values in appsettings.json.
If EnableFileLogging is true, logs are saved to the file specified in Paths.LogFile.
SimulateFtpFailure can be toggled to test FTP error handling.


Run Export Scenarios
Success Scenario
Shelldotnet run export --success
Error Scenario
Shelldotnet run export --error
With Logging to File
Shelldotnet run export --success --logfile logfile1_successdotnet run export --error --logfile logfile2_errors

CLI flags override configuration values when specified.


Review Outputs

Export file: /MockFiles/adm_export.txt (or path from appsettings.json)
Response files: /MockFiles/adm_response_success.txt, /MockFiles/adm_response_error.txt
Logs: Console output and optional file output (logfile1_success, logfile2_errors)


Testing

Run automated tests using dotnet test
Validate export content, FTP simulation, and response parsing
Expand test coverage using mock repositories and dependency injection


Explore Business Scenarios
See /docs/business/BusinessScenarios.md for real-world use cases.

Documentation Index
Business Context

BusinessContext.md
Glossary.md
Personas.md
BusinessScenarios.md

Specifications
01.Business Requirements Specification.docx
02.Functional Specification.docx
03.Technical Specification.docx
04.User Stories.docx

Testing & Validation
05.Test Plan.docx
TestSummaryReport.md
IntegrationFlow.mmd

Development & Execution
06.Coding Guidelines.docx
07.Tasks.docx
08.Runbook.docx
README.md


🧩 Example Code
The prototype includes modular C# components for each major function:

| File | Purpose | 
| :_: | :-: |
|Program.cs | CLI orchestration and argument parsing |
MockData.csSupplies mock Products and Product GroupsAdmLodFileGen.csGenerates ADMLOD export file contentFtpClientMock.csSimulates FTP upload (local write + logging)ResponseParser.csParses DTN response filesLogger.csLogs actions and errors to console or file
See 09.Example Snippets.docx for sample usage patterns and implementation tips.

🌟 Key Highlights
Fully mocked environment—no live FTP or DTN integration
Demonstrates traceability from requirements to test cases
Supports error simulation and structured logging
Modular design for reuse across integration prototypes
SPARC-aligned documentation for stakeholder clarity
Developer onboarding checklist included in /docs/onboarding/


Contents of the Package

ComponentPurpose01.Business Requirements Specification.docxDefines project scope, objectives, business rules, and success criteria.02.Functional Specification.docxDetails CLI commands, mock data structure, and export file format.03.Technical Specification.docxDescribes architecture, technology stack, and file I/O design.04.User Stories.docxCaptures user requirements with acceptance criteria.05.Test Plan.docxOutlines test objectives, scope, environment, and test cases.06.Coding Guidelines.docxEstablishes code style, naming conventions, and error handling rules.07.Tasks.docxLists implementation tasks for mock data, export logic, FTP simulation, and logging.08.Runbook.docxProvides build, run, and troubleshooting instructions.09.Example Snippets.docxIncludes sample C# code for export generation and mock data setup.ADMLOD Response.docxShows example DTN response output for validation.ProductGroupADMLODTemplate.xlsxTemplate for product group commands and structure.CAT_ADMLOD_PRODUCTGROUP.txt & ADMLOD Product Group Export.docxSample export files for ADMLOD format.DTN TABS ADMLOD User Guide rel_40_41.pdfReference guide for ADMLOD protocol and standards.logfile1_success, logfile2_errorsSample log outputs from success and error scenarios.

License & Usage
This framework is intended for internal use in sandbox environments. It does not connect to live systems and should not be used for production deployment without modification.