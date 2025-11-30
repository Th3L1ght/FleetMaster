using System;
using FleetMaster.Core.Enums;

namespace FleetMaster.Core.Entities
{
    public class Order : BaseEntity
    {
        private string _description;
        private string _destination;
        private double _weightKg;
        private double _price;
        private OrderStatus _status = OrderStatus.New;
        private int? _assignedVehicleId;
        private int? _assignedDriverId;

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public string Destination
        {
            get { return _destination; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Адреса доставки обов'язкова");
                _destination = value;
            }
        }

        public double WeightKg
        {
            get { return _weightKg; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Вага вантажу має бути більше 0");
                _weightKg = value;
            }
        }

        public double Price
        {
            get { return _price; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Ціна не може бути від'ємною");
                _price = value;
            }
        }

        public OrderStatus Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public int? AssignedVehicleId
        {
            get { return _assignedVehicleId; }
            set { _assignedVehicleId = value; }
        }

        public int? AssignedDriverId
        {
            get { return _assignedDriverId; }
            set { _assignedDriverId = value; }
        }

        public override string ToString()
        {
            return $"#{Id} | {Description} -> {Destination} | {WeightKg}kg | {Status}";
        }
    }
}