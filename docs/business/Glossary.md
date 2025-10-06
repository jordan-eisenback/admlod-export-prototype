# Glossary – ADMLOD Export Prototype

| Term                 | Definition                                                                 |
| :------------------: | :------------------------------------------------------------------------: |
| ADMLOD               | Automated Data Management Load – DTN’s protocol for batch file integration |
| **DTN**              | Data Transmission Network – external system receiving ADMLOD files         |
| **Product**          | An individual item to be exported (e.g., Diesel #1)                        |
| **Product Group**    | A logical grouping of products (e.g., DIESEL, GASOLINE)                    |
| **Export File**      | A file containing ADMLOD commands to merge products/groups                 |
| **FTP Upload**       | Simulated file transfer to DTN (mocked locally)                            |
| **Mock Data**        | Simulated product and group data used for testing                          |
| **Mock Response**    | Simulated DTN feedback file indicating success or error                    |
| **MERGE Command**    | ADMLOD command to add/update product or group data                         |
| **Logger**           | Component that records actions and errors with timestamps and severity     |
| **Correlation ID**   | Unique identifier used to trace actions across logs                        |
| **Success Scenario** | Test case where all commands succeed                                       |
| **Error Scenario**   | Test case with simulated failures (e.g., missing fields)                   |
| **Runbook**          | Step-by-step guide to build, run, and troubleshoot the prototype           |
