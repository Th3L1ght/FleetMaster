using System;

namespace FleetMaster.Core.Exceptions
{
    public class FinancialException : FleetMasterException
    {
        public decimal Amount { get; }

        public FinancialException(string message, decimal amount)
            : base($"{message} (Сума: {amount:C})")
        {
            Amount = amount;
        }
    }
}