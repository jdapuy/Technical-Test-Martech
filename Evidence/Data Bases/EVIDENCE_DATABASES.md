# Evidence – Databases (AdventureWorks & AdHocDB)

This document describes the screenshots that document the database setup for the technical test: **AdventureWorks** (restored) and **AdHocDB** (structure with tables, views, stored procedures, and UDF).

---

## Objective

Show that the required databases exist and are correctly structured: **AdventureWorks** as the source for SSIS packages (A, B, C), and **AdHocDB** as the destination with all objects created in Fases 1 and 2 (tables, view, UDF, stored procedures).

---

## Screenshots

| # | Suggested filename | What the image shows |
|---|--------------------|----------------------|
| 1 | AdventureWorks_Restored | **AdventureWorks and AdHocDB in Object Explorer.** SSMS connected to `localhost`. Under **Bases de datos** (Databases): **AdHocDB** is expanded, showing **Tablas** (Tables) with **dbo.tbCustomers**, **dbo.tbProduct**, **dbo.tbSalesSummary**. **AdventureWorks_v2025** is visible (restored AdventureWorks database used as the data source for the SSIS packages). This confirms both databases are present and that the main tables in AdHocDB exist. |
| 2 | AdHocDB_tables | **Full AdHocDB structure.** Object Explorer with **AdHocDB** expanded. **Tablas (Tables):** dbo.tbCustomers, dbo.tbProduct, dbo.tbSalesSummary, dbo.tbStarWars. **Vistas (Views):** dbo.vw_SalesSummaryReport. **Programación (Programmability):** under **Procedimientos almacenados (Stored Procedures):** dbo.sp_GetSalesSummaryByCustomer, dbo.usp_InsertManualSummary; under **Funciones (Functions)** > **Funciones escalares:** dbo.fn_DateOnly. This confirms all objects required for the test (tables, view, UDF, stored procedures) are created in AdHocDB. |

---

## Summary

- **AdventureWorks_v2025:** Restored sample database; source for SSIS packages A (Customers), B (Products), and C (Sales Summary).
- **AdHocDB:** Destination database containing:
  - **Tables:** tbCustomers, tbProduct, tbSalesSummary, tbStarWars.
  - **View:** vw_SalesSummaryReport (used by SSRS report; uses UDF fn_DateOnly).
  - **UDF:** fn_DateOnly (used inside vw_SalesSummaryReport).
  - **Stored procedures:** sp_GetSalesSummaryByCustomer (API GET), usp_InsertManualSummary (API POST).
