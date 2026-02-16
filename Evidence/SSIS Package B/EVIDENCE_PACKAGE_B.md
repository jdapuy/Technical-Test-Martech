# Evidence – SSIS Package B (Products)

This document describes the screenshots that document the correct execution of **Package B** and the incremental load into **tbProduct** without duplicates.

---

## Objective

Load products from AdventureWorks into **AdHocDB.dbo.tbProduct** incrementally: only new records, no duplicates by **ProductId**. The Lookup compares each ProductId against tbProduct; only "No Match" rows are inserted.

---

## Screenshots

| # | Suggested filename | What the image shows |
|---|--------------------|----------------------|
| 1 | SS1_Empty_Products_Table | **Initial state.** SSMS window showing the result of `SELECT COUNT(*) AS Total FROM dbo.tbProduct;` with **Total = 0**. The table is empty before the first package run. |
| 2 | SS2_PackageB_Flow | **Data flow design.** Package B components: OLE DB Source (product query from AdventureWorks), Lookup (match by ProductId against tbProduct), OLE DB Destination (tbProduct). The **Lookup No Match Output** (red path) must be connected to the Destination. If the package was already run, components show a green success checkmark. |
| 3 | SS3_First_Execution | **First run – Progress.** Progress tab showing the package finished successfully. **OLE DB Destination** shows a row count **greater than 0**, indicating the initial product load. |
| 4 | SS4_First_Execution_LookUp_0 | **First run – Lookup detail.** Lookup messages in the Progress tab: e.g. *"Lookup has cached 0 rows"* (tbProduct was empty), so all source rows were "No Match" and were inserted into the destination. |
| 5 | SS5_Second_Execution | **Second run – Progress.** Progress tab for the **second** execution. **OLE DB Destination** must show **0 rows written**, because all ProductIds already exist in tbProduct. |
| 6 | SS6_Second_Execution_LookUp_304 | **Second run – Lookup detail.** Lookup messages: e.g. *"Lookup has cached a total of 304 rows"*. All source rows matched; none were sent to the destination. Confirms no duplicate inserts. |
| 7 | SS7_Select_tbProducts | **Database check.** Screenshot of SSMS running `SELECT * FROM dbo.tbProduct;` showing the actual product records loaded. This image should display key columns like ProductId, Name, CurrentPrice, CategoryName, and CreatedAt, confirming that real data has been inserted and demonstrating the table is properly populated with the expected rows.

---

## Summary

- **Source:** AdventureWorks (Production.Product, ProductCategory, ProductSubcategory).
- **Destination:** AdHocDB.dbo.tbProduct.
- **Incremental key:** ProductId.
- **Proof:** First run inserts rows; second run inserts 0 rows; Total in tbProduct does not increase.
