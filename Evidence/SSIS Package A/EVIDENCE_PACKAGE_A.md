# Evidence – SSIS Package A (Customers)

This document describes the screenshots that document the correct execution of **Package A** and the incremental load into **tbCustomers** without duplicates.

---

## Objective

Load customers from AdventureWorks into **AdHocDB.dbo.tbCustomers** incrementally: only new records, no duplicates by **CustomerId**. The Lookup compares each CustomerId against tbCustomers; only "No Match" rows are inserted.

---

## Screenshots

| # | Suggested filename | What the image shows |
|---|--------------------|----------------------|
| 1 | SS1_Empty_Customers_Table | **Initial state.** SSMS window showing the result of `SELECT COUNT(*) AS Total FROM dbo.tbCustomers;` with **Total = 0**. The table is empty before the first package run. |
| 2 | SS2_PackageA_Flow | **Data flow design.** Package A components: OLE DB Source (customer query from AdventureWorks), Lookup (match by CustomerId against tbCustomers), OLE DB Destination (tbCustomers). The **Lookup No Match Output** (red path) must be connected to the Destination. If the package was already run, components show a green success checkmark. |
| 3 | SS3_First_Execution | **First run – Progress.** Progress tab showing the package finished successfully. **OLE DB Destination** shows a row count **greater than 0** (e.g. 19,119), indicating the initial customer load. |
| 4 | SS4_First_Execution_LookUp_0 | **First run – Lookup detail.** Lookup messages in the Progress tab: e.g. *"Lookup has cached 0 rows"* (tbCustomers was empty), so all source rows were "No Match" and were inserted into the destination. |
| 5 | SS5_Second_Execution | **Second run – Progress.** Progress tab for the **second** execution. **OLE DB Destination** must show **0 rows written**, because all CustomerIds already exist in tbCustomers. |
| 6 | SS6_Second_Execution_LookUp_19119 | **Second run – Lookup detail.** Lookup messages: e.g. *"Lookup has cached a total of 19119 rows"*. All source rows matched; none were sent to the destination. Confirms no duplicate inserts. |

---

## Summary

- **Source:** AdventureWorks (Sales.Customer, Person.Person, Phone, Email).
- **Destination:** AdHocDB.dbo.tbCustomers.
- **Incremental key:** CustomerId.
- **Proof:** First run inserts rows; second run inserts 0 rows; Total in tbCustomers does not increase.
