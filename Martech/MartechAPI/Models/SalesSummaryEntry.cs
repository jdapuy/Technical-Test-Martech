namespace MartechAPI.Models
{
    public class SalesSummaryEntry
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public DateTime SummaryDate { get; set; }
        public int TotalItems { get; set; }
        public decimal TotalSales { get; set; }
    }
}