# Patholab Diagnostic Management Suite
## Menu-by-Menu Presentation & Walkthrough Guide

Use this guide to walk your client through the software system by clicking through the sidebar menus one by one.

---

### 1. 📊 Main Console ➔ Dashboard (`/Dashboard`)
* **What to click**: "Dashboard"
* **Explain to the client**: 
  * This is the control center of the laboratory. It gives the laboratory manager a real-time clinical and financial summary.
  * **Key visual elements**:
    * **Key Metrics**: Total registrations, pending samples, reports under verification, and today's revenue.
    * **Revenue Chart**: An interactive chart showing daily collection trends.
    * **Department Stats**: Shows which department (Pathology, Biochemistry, Hematology) is processing the most workloads.
    * **Recent Activities**: A real-time timeline of patients registering and reports being published.

---

### 2. 📋 Registration & Bills ➔ Patients Master (`/Patients`)
* **What to click**: "Patients Master"
* **Explain to the client**:
  * This page manages patient records.
  * **Key actions**:
    * **Search Bar**: Search patients by Mobile, Patient Code (e.g. `PAT-001`), or Name.
    * **"Add Patient" Button**: Opens a profile form to register Name, DOB, Age, Blood Group, Identity Details (Aadhaar/Passport), and Emergency Contact info.
    * **Action Column**: Edit demographics or view the complete case history of a patient.

---

### 3. 🩺 Registration & Bills ➔ Doctor Profiles (`/Doctors`)
* **What to click**: "Doctor Profiles"
* **Explain to the client**:
  * Manages the referral network of doctors.
  * **Key actions**:
    * **Referral Settings**: Add doctors, configure qualifications, contact numbers, and **Referral Commission Rates**.
    * **Commission Tracking**: Configured either as a *Percentage* (e.g. 15% commission) or a *Fixed Amount* per test. The system automatically tracks payouts for referrals.

---

### 4. ➕ Registration & Bills ➔ Order Booking (`/Orders/Create`)
* **What to click**: "Order Booking"
* **Explain to the client**:
  * This is the patient check-in portal used by the front desk receptionist.
  * **Key actions**:
    * **Patient Search**: Instantly pull up demographics.
    * **Doctor Referral Selection**: Select the referring doctor.
    * **Select Tests/Packages**: Select individual tests (e.g., Fasting Sugar) or bundled savings Packages (e.g., General Wellness Package).
    * **Automatic Billing Summary**: Dynamically calculates Gross Amount, Discounts, Tax, and Net Due in real-time.
    * Click **"Book Order"** to save the invoice and transition status to `SamplePending`.

---

### 5. 📄 Registration & Bills ➔ Booking History (`/Orders`)
* **What to click**: "Booking History"
* **Explain to the client**:
  * A master record of all diagnostic orders booked in the system.
  * **Key actions**:
    * Track real-time status: `Registered`, `SamplePending`, `SampleCollected`, `UnderVerification`, or `Published`.
    * Easy filter shortcuts to see orders that are unpaid, pending review, or completed.

---

### 6. 💳 Registration & Bills ➔ Invoices & Receipts (`/Billing/Invoices`)
* **What to click**: "Invoices & Receipts"
* **Explain to the client**:
  * This is the accounting terminal for payments.
  * **Key actions**:
    * **Invoice Ledger**: Lists invoices (`INV-XXXXXX`) with Gross, Net, Paid, and Due amounts.
    * **"Record Payment" Modal**: Record payments (UPI, Cash, Card) with transaction references.
    * **"Print Receipt"**: Generates a clean thermal/PDF receipt containing itemized test charges for the patient.

---

### 7. 🧪 Laboratory Workstation ➔ Specimens Receipt (`/Laboratory/Samples`)
* **What to click**: "Specimens Receipt"
* **Explain to the client**:
  * Used by lab technicians to log received biological samples.
  * **Key actions**:
    * **Sample Status**: Tracks sample transit. Click **"Collect Sample"** to generate barcodes (`SMP-XXXXXX`) and record the collector's name.
    * Click **"Mark Received"** once the specimen vial is physically checked into the analyzer tray. Status updates to `Received` and automatically queues for testing.

---

### 8. ✍️ Laboratory Workstation ➔ Pathologist Review (`/Laboratory/Verification`)
* **What to click**: "Pathologist Review"
* **Explain to the client**:
  * This is the validation workstation for Lab Technicians and Pathologists.
  * **Key actions**:
    * **Results Grid**: Select an order to enter findings.
    * **Parameter Input**: Enter diagnostic values (e.g., Hemoglobin `14.5`).
    * **Smart Flagging**: System highlights out-of-range values in **Yellow (Low/High)** and **Red (Critical)** compared against age/gender reference ranges.
    * The technician clicks **"Submit for Review"**, sending it to the pathologist verification queue.
    * The Pathologist logs in, adds clinical interpretations, and clicks **"Verify & Sign-off"**.

---

### 9. 🖨️ Laboratory Workstation ➔ Diagnostic Reports (`/Reports`)
* **What to click**: "Diagnostic Reports"
* **Explain to the client**:
  * The final repository of patient clinical reports.
  * **Key actions**:
    * Access reports with `Published` status.
    * **"View PDF" Button**: Displays a premium, print-ready diagnostic report showing patient details, test findings, reference ranges, abnormal flags, pathologist comments, and an official **Digital Signature**.

---

### 10. 🏠 Operations ➔ Home Collections (`/HomeCollection`)
* **What to click**: "Home Collections"
* **Explain to the client**:
  * Portal for external phlebotomy home visit scheduling.
  * **Key actions**:
    * **Scheduler**: Requests a home visit, specifies address, date, preferred time slot.
    * **Agent Assignment**: Admin assigns a phlebotomist to the visit.
    * **Transit Workflow**: The assigned agent updates visit status (`Requested` ➔ `OnTheWay` ➔ `SampleCollected` ➔ `Completed`) from their mobile device.

---

### 11. ⚙️ Administration ➔ User Accounts (`/Admin/Users`)
* **What to click**: "User Accounts" (Visible only to Admins)
* **Explain to the client**:
  * Staff profile manager.
  * **Key actions**:
    * Create user accounts for Receptionists, Techs, Phlebotomists, and Pathologists.
    * Link them to specific Roles to restrict access to unauthorized sections (e.g. receptionist cannot edit lab results).

---

### 12. 🕒 Administration ➔ Security Audits (`/Admin/AuditLogs`)
* **What to click**: "Security Audits" (Visible only to Admins)
* **Explain to the client**:
  * Regulatory audit log tracking data integrity.
  * **Key actions**:
    * Logs every database change: Tracks the **Old Value** and **New Value** of modified items, the responsible user, IP Address, and timestamp.

---

### 13. 📝 Administration ➔ System Logs (`/Admin/TodayLogs`)
* **What to click**: "System Logs" (Visible only to Admins)
* **Explain to the client**:
  * A real-time, terminal-themed logging page.
  * **Key actions**:
    * Monitors background tasks, database connections, and warning logs directly from the web interface without needing access to server files.

---

### 14. 🎛️ Administration ➔ Diagnostic Setup (`/Tests`)
* **What to click**: "Diagnostic Setup" (Visible only to Admins)
* **Explain to the client**:
  * The system configurator.
  * **Key actions**:
    * Set up departments, test directories (Hematology, Endocrinology, etc.), individual test parameters (Normal, Low, High thresholds), and test packages.
