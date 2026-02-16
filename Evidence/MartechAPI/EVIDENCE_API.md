# Evidence – API REST (Fase 6)

This document describes the screenshots and Postman evidence for the **MartechAPI** REST API: **GET** (sales summary by customer) and **POST** (manual summary entry), including success and validation/error cases.

---

## Postman collection links

| Endpoint | Link |
|----------|------|
| **GET** – Sales summary by customer | [Postman – GET example](https://www.postman.com/restless-crater-334934/workspace/postman-api/example/22814138-d7a69e79-87f2-4949-8608-90d12a1fe2ed?action=share&source=copy-link&creator=22814138&ctx=documentation) |
| **POST** – Manual summary entry | [Postman – POST request](https://www.postman.com/restless-crater-334934/workspace/postman-api/request/22814138-c17f6b0f-84bc-4a23-a6aa-6737e884ab2f?action=share&source=copy-link&creator=22814138&ctx=documentation) |

---

## Endpoints

- **GET** `http://localhost:5000/api/salessummary?idCustomer=11000` — Returns sales summary records for the given customer (uses `sp_GetSalesSummaryByCustomer`).
- **POST** `http://localhost:5000/api/salessummary/manual_summary_entry` — Creates a manual entry in `tbSalesSummary` with `Source = 'MANUAL'` (uses `usp_InsertManualSummary`). Body: JSON with `CustomerId`, `CustomerName`, `SummaryDate`, `TotalItems`, `TotalSales`.

---

## Screenshots

### GET – Sales summary by customer

| # | Suggested filename | What the image shows |
|---|--------------------|----------------------|
| 1 | SS1_get_sales_summary_by_customerId_Success | **GET success (200 OK).** Postman: `GET http://localhost:5000/api/salessummary?idCustomer=11000`. Response **200 OK** with a JSON array of sales summary objects (salesSummaryId, customerId, customerName, summaryDate, totalItems, totalSales, source, createdAt). Proves the GET endpoint and stored procedure `sp_GetSalesSummaryByCustomer` work correctly. |
| 2 | SS2_get_sales_summary_by_customerId_Error | **GET validation error (400).** Postman: `GET http://localhost:5000/api/salessummary?idCustomer=diego`. Response **400 Bad Request** with validation error: `"errors": { "idCustomer": [ "The value 'diego' is not valid." ] }`. Proves that invalid `idCustomer` (non-numeric) is rejected. |

### POST – Manual summary entry (success)

| # | Suggested filename | What the image shows |
|---|--------------------|----------------------|
| 3 | SS3_post_manual_summary_entry_success | **POST success (200 OK).** Postman: `POST .../api/salessummary/manual_summary_entry` with body `CustomerId: 11000`, `CustomerName: "Customer manual test"`, `SummaryDate: "2024-02-15"`, `TotalItems: 5`, `TotalSales: 250.50`. Response **200 OK** with `"message": "Manual summary entry created successfully"`. SSMS (optional in same screenshot): `SELECT * FROM tbSalesSummary WHERE CustomerId = 11000` showing the new row with `Source = 'MANUAL'`. Proves the POST endpoint and `usp_InsertManualSummary` work. |

### POST – Manual summary entry (validation errors)

| # | Suggested filename | What the image shows |
|---|--------------------|----------------------|
| 4a | SS4_post_manual_summary_entry_error_CustomerName | **POST 400 – CustomerName required.** Body with `CustomerName` commented out or missing. Response **400** with `"errors": { "CustomerName": [ "The CustomerName field is required." ] }`. |
| 4b | SS4_post_manual_summary_entry_error_CustomerId | **POST 400 – CustomerId required.** Body with `CustomerId` commented out or missing. Response **400** with `"error": "CustomerId must be greater than 0"`. |
| 4c | SS4_post_manual_summary_entry_error_totalItems_negative | **POST 400 – TotalItems negative.** Body with `"TotalItems": -5`. Response **400** with `"error": "TotalItems cannot be negative"`. |
| 4d | SS4_post_manual_summary_entry_error_totalSales_negative | **POST 400 – TotalSales negative.** Body with `"TotalSales": -250.50`. Response **400** with `"error": "TotalSales cannot be negative"`. |
| 4e | SS4_post_manual_summary_entry_error_SummaryDate | **POST 500 – SummaryDate invalid.** Body with `SummaryDate` commented out or invalid (e.g. default/overflow). Response **500** with message like `"SqlDateTime overflow. Must be between 1/1/1753 12:00:00 AM"` or validation error. Proves date validation or handling of missing/invalid date. |

---

## Summary

- **GET** `/api/salessummary?idCustomer={id}`: Returns sales summaries for the customer; validates `idCustomer` (must be valid integer > 0). Evidence: success (200) and error (400) screenshots.
- **POST** `/api/salessummary/manual_summary_entry`: Inserts a manual record with `Source = 'MANUAL'`. Validates: CustomerId > 0, CustomerName required, TotalItems ≥ 0, TotalSales ≥ 0, SummaryDate valid. Evidence: one success screenshot and several error screenshots (400/500).
- **Stored procedures used:** `sp_GetSalesSummaryByCustomer` (GET), `usp_InsertManualSummary` (POST).
- **Postman links** for GET and POST are included at the top of this document for sharing and reproduction.
