# Technical Test – Marketing Automation Specialist (Martech)

This repository contains the deliverable for the technical assessment: **databases**, **SSIS packages**, **SSRS report**, and **REST API**, with evidence and documentation.

---

## Overview

The solution implements a data pipeline and reporting layer:

- **Data sources:** SQL Server with **AdventureWorks** (sample DB) and **AdHocDB** (destination).
- **SSIS:** Incremental loads (Customers, Products, Sales Summary) and a package that calls the Star Wars API (SWAPI).
- **SSRS:** A report that filters sales summary by date range (InitialDate / FinalDate), using a view and a UDF.
- **API:** A .NET Web API with GET (sales summary by customer) and POST (manual summary entry), using stored procedures.

---

## Repository structure

```
Technical-Test-Martech/
├── Martech/                          # Solution folder (or root)
│   ├── MartechSSIS/                  # SSIS project (Packages A, B, C, Star Wars)
│   ├── MartechSSRS/                  # SSRS project (Sales Summary report)
│   ├── MartechAPI/                   # ASP.NET Core Web API
│   └── MartechSSIS.slnx              # Solution file
├── Evidence/                         # Screenshots and documentation
│   ├── Data Bases/                   # AdventureWorks & AdHocDB structure
│   │   └── EVIDENCE_DATABASES.md
│   ├── SSIS Package A/               # Customers incremental load
│   │   └── EVIDENCE_PACKAGE_A.md
│   ├── SSIS Package B/               # Products incremental load
│   │   └── EVIDENCE_PACKAGE_B.md
│   ├── SSIS Package C/               # Sales Summary incremental load
│   │   └── EVIDENCE_PACKAGE_C.md
│   ├── SSIS Package Star Wars/       # SWAPI integration
│   │   └── EVIDENCE_PACKAGE_STARWARS.md
│   ├── SSRS Reports/                 # Sales Summary report
│   │   └── EVIDENCE_SSRS_REPORT.md
│   └── MartechAPI/                   # API (GET/POST) and Postman evidence
│       └── EVIDENCE_API.md
├── TechTest MartechPosition.pdf                      # Original technical test (PDF)
├── SCRIPTS_SQL_QUERIES.md            # SQL scripts (tables, views, UDF, SPs, queries)
└── README.md                         # This file
```

---

## Prerequisites

- **SQL Server** (2019/2022/2025) and **SSMS**
- **AdventureWorks** sample database restored
- **AdHocDB** created with tables, view, UDF, and stored procedures (see `SCRIPTS_SQL_QUERIES.md`)
- **Visual Studio 2022** with:
  - Data storage and processing (SSIS, SSRS)
  - ASP.NET and web development (for the API)
- **.NET SDK** (e.g. .NET 8 or 10) for the API
- **Postman** (optional) for testing the API

---

## How to run

### 1. Databases 

- Restore **AdventureWorks** and create **AdHocDB** in SSMS.
- Run the scripts in `SCRIPTS_SQL_QUERIES.md` in order (tables, view, UDF, stored procedures).

### 2. SSIS packages 

- Open the solution in Visual Studio and open the **MartechSSIS** project.
- Run **Package A** (Customers), **Package B** (Products), **Package C** (Sales Summary), and **Package Star Wars** as needed.
- Connection managers must point to your **AdventureWorks** and **AdHocDB** instances.

### 3. SSRS report 

- Open the **MartechSSRS** project in Visual Studio.
- Set the report data source to **AdHocDB**.
- Run the report in preview and use **InitialDate** and **FinalDate** parameters.

### 4. API 

- Set **MartechAPI** as the startup project.
- In `appsettings.json` (or `appsettings.Development.json`), set the **AdHocDB** connection string.
- Run the project (F5). The API listens on the URL shown in the console (e.g. `https://localhost:7207` or `http://localhost:5000`).
- Test with **Postman**:
  - **GET** `http://localhost:5000/api/salessummary?idCustomer=11000`
  - **POST** `http://localhost:5000/api/salessummary/manual_summary_entry` with a JSON body (see `Evidence/MartechAPI/EVIDENCE_API.md`).

---

## Evidence and documentation

| Phase        | Description                    | Evidence |
|-------------|---------------------------------|----------|
| Databases   | AdventureWorks + AdHocDB setup   | [Evidence/Data Bases/EVIDENCE_DATABASES.md](Evidence/Data%20Bases/EVIDENCE_DATABASES.md) |
| SSIS A      | Customers incremental load      | [Evidence/SSIS Package A/EVIDENCE_PACKAGE_A.md](Evidence/SSIS%20Package%20A/EVIDENCE_PACKAGE_A.md) |
| SSIS B      | Products incremental load       | [Evidence/SSIS Package B/EVIDENCE_PACKAGE_B.md](Evidence/SSIS%20Package%20B/EVIDENCE_PACKAGE_B.md) |
| SSIS C      | Sales Summary incremental load  | [Evidence/SSIS Package C/EVIDENCE_PACKAGE_C.md](Evidence/SSIS%20Package%20C/EVIDENCE_PACKAGE_C.md) |
| SSIS Star Wars | SWAPI → tbStarWars           | [Evidence/SSIS Package Star Wars/EVIDENCE_PACKAGE_STARWARS.md](Evidence/SSIS%20Package%20Star%20Wars/EVIDENCE_PACKAGE_STARWARS.md) |
| SSRS        | Sales Summary report (dates)    | [Evidence/SSRS Reports/EVIDENCE_SSRS_REPORT.md](Evidence/SSRS%20Reports/EVIDENCE_SSRS_REPORT.md) |
| API         | GET/POST endpoints + Postman    | [Evidence/MartechAPI/EVIDENCE_API.md](Evidence/MartechAPI/EVIDENCE_API.md) |

Screenshots referenced in these documents are in the same folders.

---

## API endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET    | `/api/salessummary?idCustomer={id}` | Returns sales summary records for the given customer (uses `sp_GetSalesSummaryByCustomer`). |
| POST   | `/api/salessummary/manual_summary_entry` | Creates a manual entry in `tbSalesSummary` with `Source = 'MANUAL'` (uses `usp_InsertManualSummary`). Body: JSON with `CustomerId`, `CustomerName`, `SummaryDate`, `TotalItems`, `TotalSales`. |

---

## References

- **Technical test (PDF):** [TechTest MartechPosition](TechTest MartechPosition.pdf) — Original instructions of the assessment.
- **SQL scripts:** [SCRIPTS_SQL_QUERIES.md](SCRIPTS_SQL_QUERIES.md)

---

## Author
- **Juan Diego Apuy Villalobos**.
Technical test deliverable – Marketing Automation Specialist position.
