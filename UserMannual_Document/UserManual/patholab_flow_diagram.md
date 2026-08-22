# Patholab Diagnostic Management Suite
## Operational Flowchart

Below is the visual operational lifecycle mapping patient check-in, billing, sample collection, lab analysis, pathologist verification, and final report publishing.

```mermaid
graph TD
    classDef actor fill:#2563eb,stroke:#1e3a8a,stroke-width:2px,color:#fff;
    classDef process fill:#f3f4f6,stroke:#d1d5db,stroke-width:1px,color:#1f2937;
    classDef database fill:#fef3c7,stroke:#f59e0b,stroke-width:1.5px,color:#78350f;
    classDef action fill:#ecfdf5,stroke:#10b981,stroke-width:1.5px,color:#065f46;

    %% Actors
    Patient((Patient)):::actor
    Receptionist[Receptionist]:::actor
    Agent[Collection Agent]:::actor
    Technician[Lab Technician]:::actor
    Pathologist[Pathologist]:::actor

    %% Steps
    Patient -->|1. Walk-in or Request| Receptionist
    
    subgraph Front_Desk ["1. Front Desk (Booking & Billing)"]
        Receptionist -->|2. Register / Find Profile| PatientDB[(PTH_Patients)]:::database
        Receptionist -->|3. Book Tests & Packages| OrderDB[(PTH_Orders)]:::database
        Receptionist -->|4. Generate Invoice & Payment| InvoiceDB[(PTH_Invoices & Payments)]:::database
    end

    OrderDB -->|Option A: In-Center| Lab_Reception[5. Direct Lab Collection]:::process
    OrderDB -->|Option B: Home Visit| Home_Visit[5. Home Visit Scheduled]:::process

    subgraph Home_Collection ["2. Home Collection Transit"]
        Home_Visit -->|Assign Agent| Agent
        Agent -->|Update Status: OnTheWay| Transit[Transit Status Updates]:::process
        Agent -->|Collect Biological Samples| Transit
        Transit -->|Deliver to Central Lab| Lab_Reception
    end

    subgraph Lab_Workstation ["3. Lab Workstation & Result Entry"]
        Lab_Reception -->|6. Scan & Generate Barcode| SampleDB[(PTH_Samples)]:::database
        SampleDB -->|7. Processing at Workstation| Technician
        Technician -->|8. Input Parameter Values| ResultsDB[(PTH_TestResults)]:::database
        ResultsDB -->|Auto-Flag Levels| LevelCheck{Normal, High, Low, Critical}:::process
    end

    LevelCheck -->|Queue for validation| Pathologist

    subgraph Validation ["4. Pathology Review & Sign-Off"]
        Pathologist -->|9. Review Flags & Findings| Pathologist
        Pathologist -->|10. Add Clinical Interpretation| Verify[Verify & Sign-off]:::process
    end

    Verify -->|11. Generate Signed PDF| Publish[Report Published]:::action
    Publish -->|12. View / Download Online| Patient

    %% Styling links
    linkStyle default stroke:#4b5563,stroke-width:2px;
```
