
---

## In AdHocDB – Tables (Phase 1)

### Script 1 – Create table tbCustomers

```sql
USE AdHocDB;
GO

CREATE TABLE dbo.tbCustomers (
    CustomerId     INT          NOT NULL PRIMARY KEY,
    FirstName      NVARCHAR(50) NULL,
    LastName       NVARCHAR(50) NULL,
    Phone          NVARCHAR(25) NULL,
    Email          NVARCHAR(50) NULL,
    CreatedAt      DATETIME2(7) NOT NULL DEFAULT GETDATE()
);
GO
```

---

### Script 2 – Create table tbProduct

```sql
USE AdHocDB;
GO

CREATE TABLE dbo.tbProduct (
    ProductId      INT             NOT NULL PRIMARY KEY,
    Name           NVARCHAR(50)    NOT NULL,
    CurrentPrice   MONEY           NULL,
    CategoryName   NVARCHAR(50)    NULL,
    CreatedAt      DATETIME2(7)    NOT NULL DEFAULT GETDATE()
);
GO
```

---

### Script 3 – Create table tbSalesSummary

```sql
USE AdHocDB;
GO

CREATE TABLE dbo.tbSalesSummary (
    SalesSummaryId INT             NOT NULL IDENTITY(1,1) PRIMARY KEY,
    CustomerId      INT             NOT NULL,
    CustomerName    NVARCHAR(100)   NULL,
    SummaryDate     DATE            NOT NULL,
    Total_Items     INT             NOT NULL DEFAULT 0,
    Total_Sales     MONEY           NOT NULL DEFAULT 0,
    Source          VARCHAR(10)     NOT NULL CHECK (Source IN ('SYSTEM','MANUAL')),
    CreatedAt       DATETIME2(7)    NOT NULL DEFAULT GETDATE()
);
GO
```

---

## In AdHocDB – View, UDF and Stored Procedures

### Script 4 – UDF to return date only (no time)

```sql
USE AdHocDB;
GO

CREATE FUNCTION dbo.fn_DateOnly (@Dt DATETIME2)
RETURNS DATE
AS
BEGIN
    RETURN CAST(@Dt AS DATE);
END;
GO
```

---

### Script 5 – View for the report (uses the UDF)

```sql
USE AdHocDB;
GO

CREATE VIEW dbo.vw_SalesSummaryReport
AS
SELECT
    SalesSummaryId,
    CustomerId,
    CustomerName,
    SummaryDate,
    Total_Items,
    Total_Sales,
    Source,
    dbo.fn_DateOnly(CreatedAt) AS CreatedDateOnly
FROM dbo.tbSalesSummary;
GO
```

---

### Script 6 – Stored Procedure: get summary by customer (for API GET)

```sql
USE AdHocDB;
GO

CREATE PROCEDURE dbo.sp_GetSalesSummaryByCustomer
    @CustomerId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT SalesSummaryId, CustomerId, CustomerName, SummaryDate, Total_Items, Total_Sales, Source, CreatedAt
    FROM dbo.tbSalesSummary
    WHERE CustomerId = @CustomerId
    ORDER BY SummaryDate DESC;
END;
GO
```

---

### Script 7 – Stored Procedure: insert manual record (for API POST)

```sql
USE AdHocDB;
GO

CREATE PROCEDURE dbo.usp_InsertManualSummary
    @CustomerId    INT,
    @CustomerName  NVARCHAR(100),
    @SummaryDate   DATE,
    @Total_Items   INT,
    @Total_Sales   MONEY
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.tbSalesSummary (CustomerId, CustomerName, SummaryDate, Total_Items, Total_Sales, Source)
    VALUES (@CustomerId, @CustomerName, @SummaryDate, @Total_Items, @Total_Sales, 'MANUAL');
END;
GO
```

---

## In AdventureWorks – Queries for SSIS

Replace your AdventureWorks database name if different (e.g. AdventureWorks_v2019).

### Query for SSIS Package A – Customers

Use as **source** in the package (connection to **AdventureWorks**). Incremental load: in SSIS add a **Lookup** against AdHocDB.tbCustomers by CustomerId; only "No match" rows go to the destination.

```sql
SELECT
    c.CustomerID    AS CustomerId,
    p.FirstName,
    p.LastName,
    pp.PhoneNumber  AS Phone,
    e.EmailAddress  AS Email
FROM Sales.Customer c
INNER JOIN Person.Person p ON p.BusinessEntityID = c.PersonID
LEFT JOIN Person.PersonPhone pp ON pp.BusinessEntityID = p.BusinessEntityID
LEFT JOIN Person.EmailAddress e ON e.BusinessEntityID = p.BusinessEntityID
```

---

### Query for SSIS Package B – Products

**Source:** connection to AdventureWorks. **Incremental:** Lookup against AdHocDB.tbProduct by ProductId; only "No match" to destination. (CurrentPrice from ListPrice; if your DB has LastPrice, change it in the query.)

```sql
SELECT
    pr.ProductID     AS ProductId,
    pr.Name,
    pr.ListPrice     AS CurrentPrice,
    pc.Name          AS CategoryName
FROM Production.Product pr
LEFT JOIN Production.ProductSubcategory psc ON psc.ProductSubcategoryID = pr.ProductSubcategoryID
LEFT JOIN Production.ProductCategory pc ON pc.ProductCategoryID = psc.ProductCategoryID
WHERE pr.ListPrice > 0
```

---

### Query for SSIS Package C – Sales summary

**Source:** AdventureWorks. In destination map **Source = 'SYSTEM'**. **Incremental:** Lookup in AdHocDB by (CustomerId, SummaryDate); only "No match" to destination.

```sql
SELECT
    h.CustomerID AS CustomerId,
    LEFT(ISNULL(p.FirstName + ' ' + p.LastName, ''), 100) AS CustomerName,
    CAST(h.OrderDate AS DATE) AS SummaryDate,
    SUM(d.OrderQty) AS Total_Items,
    SUM(d.LineTotal) AS Total_Sales
FROM Sales.SalesOrderHeader h
INNER JOIN Sales.SalesOrderDetail d ON d.SalesOrderID = h.SalesOrderID
LEFT JOIN Sales.Customer c ON c.CustomerID = h.CustomerID
LEFT JOIN Person.Person p ON p.BusinessEntityID = c.PersonID
GROUP BY h.CustomerID, CAST(h.OrderDate AS DATE), p.FirstName, p.LastName
```

---

## In AdHocDB – Star Wars table

### Script 8 – Table for Star Wars API data (e.g. characters)

```sql
USE AdHocDB;
GO

CREATE TABLE dbo.tbStarWars (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    Name        NVARCHAR(200) NULL,
    BirthYear   NVARCHAR(20)  NULL,
    Gender      NVARCHAR(20)  NULL,
    Url         NVARCHAR(500) NULL,
    CreatedAt   DATETIME2(7)  NOT NULL DEFAULT GETDATE()
);
GO
```

---

## For the SSRS report

### Dataset query (parameters @InitialDate, @FinalDate)

Create report parameters **InitialDate** and **FinalDate** (Date type) and assign them to @InitialDate and @FinalDate.

```sql
SELECT SalesSummaryId, CustomerId, CustomerName, SummaryDate, Total_Items, Total_Sales, Source
FROM dbo.vw_SalesSummaryReport
WHERE SummaryDate >= @InitialDate AND SummaryDate <= @FinalDate
ORDER BY SummaryDate, CustomerId
```

---


## Summary – what to run and in what order

| Order | Script / Query | Database | Where it is used |
|-------|----------------|----------|------------------|
| 1 | Scripts 1, 2, 3 (tables) | AdHocDB | Phase 1 |
| 2 | Scripts 4, 5, 6, 7 (UDF, view, SPs) | AdHocDB | Phase 2, report and API |
| 3 | Customers query | AdventureWorks | SSIS Package A – source |
| 4 | Products query | AdventureWorks | SSIS Package B – source |
| 5 | Sales summary query | AdventureWorks | SSIS Package C – source |
| 6 | Script 8 (tbStarWars) | AdHocDB | Phase 4 |
| 7 | Report query | AdHocDB | SSRS dataset |
