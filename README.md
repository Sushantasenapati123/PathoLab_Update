# Patholab - Enterprise Laboratory Information Management System

Patholab is a complete, client-demo-ready Pathology Laboratory Management System built using a .NET Clean Architecture. It automates diagnostics workflows from patient intake registration through specimen collections, laboratory result entries, pathologist approvals, PDF report generation, and public scan QR authenticity verification.

---

## 🚀 Quick Start Guide

### Prerequisites
*   .NET 8.0 SDK
*   SQL Server LocalDB or standard SQL Server instance
*   Visual Studio 2022 or VS Code

### Setup Instructions

1.  **Configure Database Connection**:
    Open the Web API config file: `Patholab.API/appsettings.json`. Verify the `DefaultConnection` string under `ConnectionStrings`:
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=PatholabDB;Trusted_Connection=True;MultipleActiveResultSets=true"
    }
    ```

2.  **Initialize Database Schema**:
    Navigate to the solution directory and run database updates to apply migrations and seed the initial dataset:
    ```powershell
    dotnet ef database update --project Patholab.Infrastructure/Patholab.Infrastructure.csproj --startup-project Patholab.API/Patholab.API.csproj
    ```

3.  **Run the Web API Backend**:
    ```powershell
    dotnet run --project Patholab.API/Patholab.API.csproj
    ```
    The REST API and Swagger documentation will spin up at: `https://localhost:7198/swagger/index.html`.

4.  **Run the MVC Portal Frontend**:
    ```powershell
    dotnet run --project Patholab.MVC/Patholab.MVC.csproj
    ```
    Open your browser to: `https://localhost:7083/` to access the portal.

---

## 🔑 Demo Account Credentials

Log in with any of these seeded credentials to test specific role actions:

| Role designation | Username | Password | Actions allowed |
| :--- | :--- | :--- | :--- |
| **Super Admin** | `admin` | `Admin@123` | System settings, user accounts, security logs |
| **Receptionist** | `reception` | `Reception@123` | Patient registry, order bookings, invoice settlements |
| **Lab Technician** | `technician` | `Technician@123` | Specimen tube collect, lab receipt, result values entry |
| **Pathologist** | `pathologist` | `Pathologist@123` | Locked observations review, sign-off and release reports |
| **Accountant** | `accountant` | `Accountant@123` | General ledger audits, outstanding balance collection |

---

## 🔬 Core End-to-End Diagnostics Cycle

Patholab simulates the full lifecycle of laboratory operations:
1.  **Register Patients**: Sign in as `reception` and register patient profiles.
2.  **Case Booking**: In **Order Booking**, check tests and packages (e.g., *Full Body Checkup*). The system computes net price and prints receipt.
3.  **Vial Collection**: Sign in as `technician`. Under **Specimens Receipt**, select patient sample, click **Collect** to log barcode details, and click **Lab Receipt** to register processing.
4.  **Observation Input**: Under received samples, click **Enter Results**. Type numerical values. Click **Lock & Request Review** to submit to the pathology queue.
5.  **Pathologist Review**: Sign in as `pathologist`. Open the review queue, approve findings, and click **Sign-off & Publish**.
6.  **Report Dispatch**: Download the compiled PDF containing A4 branding layouts, digital signatures, and scan-to-verify QR code.
7.  **QR Authenticity Scan**: Scan the QR code with any mobile device to view public verification details confirming patient name, date, and pathologist details.

---

## 🏗️ Design Extensibility Plans

Patholab is structured to easily integrate with additional services:
*   **LIS Analyzer Integration**: Extend `SampleTest` processing loops to listen to HL7 ports.
*   **SMS/WhatsApp/Email dispatchers**: Replace the mock classes in `Patholab.Infrastructure/Services/MockNotificationServices.cs` with live Twilio, SendGrid, or WhatsApp Business API clients.
*   **Multiple Branches**: Extend domain models with `BranchId` to filter DB views based on user session branches.

---
*Developed for Patholab Diagnostics Suite. Copyright 2026. All rights reserved.*
