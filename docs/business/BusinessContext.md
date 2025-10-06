# Business Context Overview – ADMLOD Export Prototype

## What Is ADMLOD?

ADMLOD (Automated Data Management Load) is a batch file protocol used by DTN (Data Transmission Network) to ingest structured product and product group data from external systems. It is commonly used in energy, commodities, and logistics sectors to synchronize product catalogs and pricing data.

## Why Simulate ADMLOD?

Live integration with DTN requires FTP uploads and strict file formatting. This prototype simulates the entire flow—file generation, upload, and response parsing—without needing access to DTN or production systems. It enables safe testing, validation, and stakeholder review.

## Key Business Entities

- **Product**: An individual item (e.g., Diesel #1, Ethanol 95) with attributes like code, name, and family.
- **Product Group**: A logical grouping of products (e.g., DIESEL, GASOLINE) used for bulk operations.
- **Export File**: A structured ADMLOD file containing commands to merge products and groups.
- **DTN Response**: A feedback file indicating which commands succeeded or failed.

## Business Goals of the Prototype

- Validate that product and group data can be exported in ADMLOD format.
- Simulate FTP upload behavior and error handling.
- Parse DTN-like responses to confirm business rule enforcement.
- Provide traceability and auditability for all actions.

## Who Uses This?

| Role           | Purpose                                             |
| :------------: | :--------------------------------------------------:|
| Product Owner  | Ensures business rules are enforced in export logic |
| QA Analyst     | Verifies success and error scenarios                |
| Developer      | Builds and maintains CLI tool and mock logic        |
| Stakeholder    | Reviews logs and outputs for acceptance             |

## Why It Matters

This prototype helps teams:
- Avoid costly errors in live DTN integrations.
- Validate business logic early in the development cycle.
