using System;
using System.Collections.Generic;
using FleetMaster.Core.Entities;

namespace FleetMaster.BusinessLogic.Validators
{
    public class DriverValidator
    {
        public List<string> Validate(Driver driver)
        {
            var errors = new List<string>();

            if (driver == null)
            {
                errors.Add("Дані водія відсутні.");
                return errors;
            }

            if (string.IsNullOrWhiteSpace(driver.FirstName))
            {
                errors.Add("Ім'я водія обов'язкове.");
            }

            if (string.IsNullOrWhiteSpace(driver.LastName))
            {
                errors.Add("Прізвище водія обов'язкове.");
            }

            var age = DateTime.Now.Year - driver.DateOfBirth.Year;
            if (driver.DateOfBirth > DateTime.Now.AddYears(-age)) age--;

            if (age < 18)
            {
                errors.Add($"Водій занадто молодий ({age} років). Мінімальний вік: 18.");
            }

            if (age > 75)
            {
                errors.Add($"Водій занадто літній ({age} років) для роботи в логістиці.");
            }

            if (driver.ExperienceYears < 0)
            {
                errors.Add("Стаж не може бути від'ємним.");
            }

            if (driver.ExperienceYears > (age - 18))
            {
                errors.Add("Помилка: Стаж перевищує можливий робочий час водія.");
            }

            if (string.IsNullOrEmpty(driver.LicenseNumber))
            {
                errors.Add("Номер водійського посвідчення відсутній.");
            }

            return errors;
        }
    }
}