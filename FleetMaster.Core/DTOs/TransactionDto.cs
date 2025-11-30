using System;

namespace FleetMaster.Core.DTOs
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }

        public string FormattedAmount => $"{(Type == "Income" ? "+" : "-")}{Amount:N2} UAH";
    }
}