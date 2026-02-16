# Evidence – SSIS Package Star Wars (API)

This document describes the screenshots that document the execution of the **Star Wars API** SSIS package and the load of paginated data into **tbStarWars**.

---

## Objective

Call the **SWAPI** (https://swapi.dev), fetch all paginated results from the **people** endpoint, and insert them into **AdHocDB.dbo.tbStarWars**. The script follows the `next` URL until it is null so that every page (and thus all characters) is loaded.

---

## Screenshots

| # | Suggested filename | What the image shows |
|---|--------------------|----------------------|
| 1 | SS1_Empty_StarWars_Table | **Before run.** Flujo de control with the **Script Task** "Call Star Wars API" (PackageStarWars.dtsx). In SSMS, the query `SELECT COUNT(*) AS Total FROM dbo.tbStarWars;` returns **Total = 0**. The table is empty before the package execution. Object Explorer shows `dbo.tbStarWars` under AdHocDB. |
| 2 | SS2_Select_StarWars_Table | **After run.** Script Task with a **green checkmark** and the message "La ejecución del paquete se completó correctamente." In SSMS, a query such as `SELECT TOP (1000) Id, Name, BirthYear, Gender, Url, CreatedAt FROM dbo.tbStarWars` shows the loaded data (e.g. **92 rows**). Columns include character names (e.g. Luke Skywalker, C-3PO, Darth Vader), BirthYear (e.g. 19BBY, 112BBY), Gender, Url, and CreatedAt. This confirms that all paginated API results were inserted. |

---

## Summary

- **Endpoint:** https://swapi.dev/api/people/
- **Method:** GET (paginated; follow `next` until null).
- **Destination:** AdHocDB.dbo.tbStarWars (Id, Name, BirthYear, Gender, Url, CreatedAt).
- **Proof:** First image shows empty table and package design; second shows successful execution and table populated with all characters (e.g. 92 rows).
