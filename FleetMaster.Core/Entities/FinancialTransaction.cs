using System;
using FleetMaster.Core.Enums;

namespace FleetMaster.Core.Entities
{
    public class FinancialTransaction : BaseEntity
    {
        private decimal _amount;
        private TransactionType _type;
        private string _description;
        private DateTime _transactionDate;
        private int? _relatedOrderId;
        private int? _relatedDriverId;

        public decimal Amount
        {
            get { return _amount; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Сума транзакції має бути додатною (тип визначає знак)");
                _amount = value;
            }
        }

        public TransactionType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public DateTime TransactionDate
        {
            get { return _transactionDate; }
            set { _transactionDate = value; }
        }

        public int? RelatedOrderId
        {
            get { return _relatedOrderId; }
            set { _relatedOrderId = value; }
        }

        public int? RelatedDriverId
        {
            get { return _relatedDriverId; }
            set { _relatedDriverId = value; }
        }

        public override string ToString()
        {
            string sign = Type == TransactionType.Income ? "+" : "-";
            return $"[{TransactionDate:g}] {sign}{Amount} UAH | {Type} | {Description}";
        }
    }
}