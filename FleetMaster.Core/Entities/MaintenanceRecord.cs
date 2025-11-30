using System;

namespace FleetMaster.Core.Entities
{
    public class MaintenanceRecord : BaseEntity
    {
        private int _vehicleId;
        private string _description;
        private double _cost;
        private DateTime _serviceDate;
        private int _mileageAtService;
        private bool _isScheduled;

        public int VehicleId
        {
            get { return _vehicleId; }
            set { _vehicleId = value; }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Опис робіт обов'язковий");
                _description = value;
            }
        }

        public double Cost
        {
            get { return _cost; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Вартість ремонту не може бути від'ємною");
                _cost = value;
            }
        }

        public DateTime ServiceDate
        {
            get { return _serviceDate; }
            set { _serviceDate = value; }
        }

        public int MileageAtService
        {
            get { return _mileageAtService; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Пробіг не може бути від'ємним");
                _mileageAtService = value;
            }
        }

        public bool IsScheduled
        {
            get { return _isScheduled; }
            set { _isScheduled = value; }
        }

        public override string ToString()
        {
            string type = IsScheduled ? "ТО" : "РЕМОНТ";
            return $"[{ServiceDate:d}] {type}: {Description} (-{Cost} грн)";
        }
    }
}