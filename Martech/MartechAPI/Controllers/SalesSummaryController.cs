using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using MartechAPI.Models;

namespace MartechAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesSummaryController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public SalesSummaryController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("AdHocDB");
        }

        // GET: api/salessummary?idCustomer=123
        [HttpGet]
        public IActionResult GetSalesSummaryByCustomer([FromQuery] int idCustomer)
        {
            try
            {
                if (idCustomer <= 0)
                {
                    return BadRequest(new { error = "idCustomer must be greater than 0" });
                }

                var results = new List<object>();

                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("dbo.sp_GetSalesSummaryByCustomer", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CustomerId", idCustomer);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                results.Add(new
                                {
                                    SalesSummaryId = reader.GetInt32("SalesSummaryId"),
                                    CustomerId = reader.GetInt32("CustomerId"),
                                    CustomerName = reader.IsDBNull("CustomerName") ? null : reader.GetString("CustomerName"),
                                    SummaryDate = reader.GetDateTime("SummaryDate"),
                                    TotalItems = reader.GetInt32("Total_Items"),
                                    TotalSales = reader.GetDecimal("Total_Sales"),
                                    Source = reader.GetString("Source"),
                                    CreatedAt = reader.GetDateTime("CreatedAt")
                                });
                            }
                        }
                    }
                }

                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        // POST: api/salessummary/manual_summary_entry
        [HttpPost("manual_summary_entry")]
        public IActionResult InsertManualSummary([FromBody] SalesSummaryEntry entry)
        {
            try
            {
                // Validaciones
                if (entry == null)
                {
                    return BadRequest(new { error = "Request body is required" });
                }

                if (entry.CustomerId <= 0)
                {
                    return BadRequest(new { error = "CustomerId must be greater than 0" });
                }

                if (string.IsNullOrWhiteSpace(entry.CustomerName))
                {
                    return BadRequest(new { error = "CustomerName is required" });
                }

                if (entry.TotalItems < 0)
                {
                    return BadRequest(new { error = "TotalItems cannot be negative" });
                }

                if (entry.TotalSales < 0)
                {
                    return BadRequest(new { error = "TotalSales cannot be negative" });
                }

                // Validar fecha (opcional: que no sea futura o muy antigua)
                if (entry.SummaryDate > DateTime.Now)
                {
                    return BadRequest(new { error = "SummaryDate cannot be in the future" });
                }

                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("dbo.usp_InsertManualSummary", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CustomerId", entry.CustomerId);
                        cmd.Parameters.AddWithValue("@CustomerName", entry.CustomerName);
                        cmd.Parameters.AddWithValue("@SummaryDate", entry.SummaryDate.Date); // Solo fecha, sin hora
                        cmd.Parameters.AddWithValue("@Total_Items", entry.TotalItems);
                        cmd.Parameters.AddWithValue("@Total_Sales", entry.TotalSales);

                        cmd.ExecuteNonQuery();
                    }
                }

                return Ok(new { message = "Manual summary entry created successfully" });
            }
            catch (SqlException sqlEx)
            {
                return StatusCode(500, new { error = "Database error", message = sqlEx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }
    }
}