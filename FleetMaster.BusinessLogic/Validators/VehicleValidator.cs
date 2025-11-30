using System;
using System.Collections.Generic;
using FleetMaster.Core.Entities;

namespace FleetMaster.BusinessLogic.Validators
{
    public class VehicleValidator
    {
        public List<string> Validate(Vehicle vehicle)
        {
            var errors = new List<string>();

            if (vehicle == null)
            {
                errors.Add("Об'єкт транспортного засобу не може бути null.");
                return errors;
            }

            if (string.IsNullOrWhiteSpace(vehicle.Brand))
            {
                errors.Add("Помилка: Бренд авто не вказано.");
            }
            else if (vehicle.Brand.Length < 2)
            {
                errors.Add("Помилка: Назва бренду занадто коротка (мінімум 2 символи).");
            }

            if (string.IsNullOrWhiteSpace(vehicle.Model))
            {
                errors.Add("Помилка: Модель авто не вказана.");
            }

            int currentYear = DateTime.Now.Year;
            if (vehicle.YearOfManufacture < 1970)
            {
                errors.Add("Помилка: Ми не обслуговуємо авто старше 1970 року.");
            }

            if (vehicle.YearOfManufacture > currentYear + 1)
            {
                errors.Add($"Помилка: Рік випуску {vehicle.YearOfManufacture} ще не настав.");
            }

            if (vehicle.LoadCapacityKg <= 0)
            {
                errors.Add("Помилка: Вантажопідйомність повинна бути додатною.");
            }

            if (vehicle.FuelConsumptionPer100Km <= 0)
            {
                errors.Add("Помилка: Витрата палива повинна бути більше 0.");
            }

            if (vehicle.FuelConsumptionPer100Km > 100)
            {
                errors.Add("Попередження: Підозріло висока витрата палива (>100л).");
            }

            if (string.IsNullOrWhiteSpace(vehicle.LicensePlate))
            {
                errors.Add("Помилка: Відсутній номерний знак.");
            }
            else if (vehicle.LicensePlate.Length < 4)
            {
                errors.Add("Помилка: Номерний знак занадто короткий.");
            }

            return errors;
        }
    }
}