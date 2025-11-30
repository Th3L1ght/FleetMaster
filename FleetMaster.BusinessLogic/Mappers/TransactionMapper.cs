using FleetMaster.Core.DTOs;
using FleetMaster.Core.Entities;

namespace FleetMaster.BusinessLogic.Mappers
{
    public static class TransactionMapper
    {
        public static TransactionDto ToDto(FinancialTransaction transaction)
        {
            return new TransactionDto
            {
                Id = transaction.Id,
                Amount = transaction.Amount,
                Type = transaction.Type.ToString(),
                Description = transaction.Description,
                Date = transaction.TransactionDate
            };
        }
    }
}