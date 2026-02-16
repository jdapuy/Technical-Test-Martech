# Evidence – SSIS Package C (Sales Summary)

This document describes the screenshots that document the correct execution of **Package C** and the incremental load into **tbSalesSummary** with **Source = 'SYSTEM'** without duplicates.

---

## Objective

Load sales summary from AdventureWorks (Header + Detail) into **AdHocDB.dbo.tbSalesSummary** incrementally: grouped by CustomerId and date, with **Source = 'SYSTEM'**. No duplicate (CustomerId, SummaryDate) combinations. The Lookup compares (CustomerId, SummaryDate) against tbSalesSummary; only "No Match" rows go through a Derived Column (Source = 'SYSTEM') and then to the Destination.

---

## Screenshots

| # | Suggested filename | What the image shows |
|---|--------------------|----------------------|
| 1 | SS1_Empty_SalesSummary_Table | **Initial state.** SSMS window showing the result of `SELECT COUNT(*) AS Total FROM dbo.tbSalesSummary;` with **Total = 0**. The table is empty before the first package run. |
| 2 | SS2_PackageC_Flow | **Data flow design.** Package C components: OLE DB Source (aggregated query by CustomerId and date from AdventureWorks), Lookup (match by CustomerId and SummaryDate against tbSalesSummary), **Derived Column** (add Source = 'SYSTEM'), OLE DB Destination (tbSalesSummary). The **Lookup No Match Output** (red path) must be connected to the Derived Column, then to the Destination. If the package was already run, components show a green success checkmark. |
| 3 | SS3_First_Execution | **First run – Progress.** Progress tab showing the package finished successfully. **OLE DB Destination** shows a row count **greater than 0** (e.g. 31,406), indicating the initial sales summary load. |
| 4 | SS4_First_Execution_LookUp_0 | **First run – Lookup detail.** Lookup messages in the Progress tab: e.g. *"Lookup has cached 0 rows"* (tbSalesSummary was empty), so all source rows were "No Match" and were inserted into the destination after the Derived Column set Source = 'SYSTEM'. |
| 5 | SS5_Second_Execution | **Second run – Progress.** Progress tab for the **second** execution. **OLE DB Destination** must show **0 rows written**, because all (CustomerId, SummaryDate) combinations already exist in tbSalesSummary. |
| 6 | SS6_Second_Execution_LookUp_31406 | **Second run – Lookup detail.** Lookup messages: e.g. *"Lookup has cached a total of 31406 rows"*. All source rows matched; none were sent to the destination. Confirms no duplicate inserts. |
| 7 | SS7_Select_tbSalesSummary | **Database check.** Screenshot of SSMS running `SELECT * FROM dbo.tbSalesSummary;` showing actual sales summary records loaded. This image should display key columns like SalesSummaryId, CustomerId, CustomerName, SummaryDate, Total_Items, Total_Sales, Source, and CreatedAt, confirming that real data has been inserted and demonstrating the table is properly populated with the expected rows.

---

## Summary

- **Source:** AdventureWorks (Sales.SalesOrderHeader + SalesOrderDetail), grouped by CustomerId and OrderDate.
- **Destination:** AdHocDB.dbo.tbSalesSummary.
- **Incremental key:** (CustomerId, SummaryDate).
- **Special:** Derived Column sets **Source = 'SYSTEM'** for all inserted rows.
- **Proof:** First run inserts rows; second run inserts 0 rows; Total in tbSalesSummary does not increase.
