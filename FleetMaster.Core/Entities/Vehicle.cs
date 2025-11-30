using System;
using FleetMaster.Core.Enums;

namespace FleetMaster.Core.Entities
{
    public abstract class Vehicle : BaseEntity
    {
        private string _brand;
        private string _model;
        private string _licensePlate;
        private int _yearOfManufacture;
        private double _loadCapacityKg;
        private double _fuelConsumptionPer100Km;
        private VehicleType _type;
        private bool _isOperational = true;

        public string Brand
        {
            get { return _brand; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Марка авто не може бути пустою");
                _brand = value;
            }
        }

        public string Model
        {
            get { return _model; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Модель авто не може бути пустою");
                _model = value;
            }
        }

        public string LicensePlate
        {
            get { return _licensePlate; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Номерний знак обов'язковий");
                _licensePlate = value.ToUpper();
            }
        }

        public int YearOfManufacture
        {
            get { return _yearOfManufacture; }
            set
            {
                if (value < 1950 || value > DateTime.Now.Year + 1)
                    throw new ArgumentException($"Некоректний рік випуску: {value}");
                _yearOfManufacture = value;
            }
        }

        public double LoadCapacityKg
        {
            get { return _loadCapacityKg; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Вантажопідйомність має бути більше 0");
                _loadCapacityKg = value;
            }
        }

        public double FuelConsumptionPer100Km
        {
            get { return _fuelConsumptionPer100Km; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Витрата палива має бути додатною");
                _fuelConsumptionPer100Km = value;
            }
        }

        public VehicleType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public bool IsOperational
        {
            get { return _isOperational; }
            set { _isOperational = value; }
        }

        public virtual string GetDescription()
        {
            return $"{Brand} {Model} ({YearOfManufacture}) - {LicensePlate}";
        }
    }
}