# Evidence – SSRS Report (Sales Summary by Date Range)

This document describes the screenshots and export that document the report: Sales Summary with parameters **InitialDate** and **FinalDate**, filtering data from **vw_SalesSummaryReport** (AdHocDB).

---

## Objective

Build an SSRS report that shows sales summary data (from **tbSalesSummary** via the view **vw_SalesSummaryReport**), with two required **date parameters** so the user can filter by range. The report must display only rows where **SummaryDate** is between **Initial Date** and **Final Date**.

---

## Screenshots and exports

| # | Suggested filename | What the image/file shows |
|---|--------------------|---------------------------|
| 1 | SS1_Report_05-02-2024_to_08-02-2024 | **Report in Preview.** Tab "Vista previa" (Preview) of **SalesSummaryReport.rdl**. At the top, the two parameters are visible: **Initial Date: 05/02/2024** and **Final Date: 08/02/2024**. The table below shows columns: Sales Summary Id, Customer Identification, Customer Name, Summary Date, Total Items, Total Sales, Source. All **Summary Date** values fall within the selected range (e.g. 05/02/2024, 06/02/2024, 07/02/2024, 08/02/2024), confirming that date filtering works. The report is paginated (e.g. "Page 1 of 2"). In the Solution Explorer, **MartechSSRS** project is visible with **SalesSummaryReport.rdl** under Informes (Reports). The Output window shows a successful build and deployment: "Generación completa: 0 errores, 0 advertencias" and "Implementación: 1 correcta". |
| 2 | SS2_Report_Export_HTML_July_2024 | **Report exported to HTML.** Same report (**SalesSummaryReport.rdl**) exported in **HTML** format, with the date range set to **July 2024** (e.g. Initial Date and Final Date covering July 2024). The HTML file shows the same table structure and confirms that the report can be run for different date ranges and exported for sharing or archiving. |

---

## Summary

- **Report:** SalesSummaryReport.rdl (MartechSSRS project).
- **Data source:** AdHocDB.
- **Dataset:** Query runs against the **view** **dbo.vw_SalesSummaryReport**, not directly against the table. That view reads from **tbSalesSummary** and uses the **UDF dbo.fn_DateOnly** (e.g. to expose a date-only column such as CreatedDateOnly). The report query filters with **@InitialDate** and **@FinalDate** in the WHERE clause.
- **Parameters:** Initial Date and Final Date (Date/Time), used to filter by **SummaryDate**.
- **Proof:** Screenshot shows the report running with a date range and only rows within that range displayed; build and deployment completed with 0 errors.
