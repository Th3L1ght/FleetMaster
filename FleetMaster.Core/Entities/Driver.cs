using System;

namespace FleetMaster.Core.Entities
{
    public class Driver : BaseEntity
    {
        private string _firstName;
        private string _lastName;
        private string _licenseNumber;
        private DateTime _dateOfBirth;
        private int _experienceYears;
        private int? _currentVehicleId;

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Ім'я не може бути пустим");
                _firstName = value;
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Прізвище не може бути пустим");
                _lastName = value;
            }
        }

        public string LicenseNumber
        {
            get { return _licenseNumber; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Номер посвідчення обов'язковий");
                _licenseNumber = value;
            }
        }

        public DateTime DateOfBirth
        {
            get { return _dateOfBirth; }
            set
            {
                if (value > DateTime.Now.AddYears(-18))
                    throw new ArgumentException("Водій має бути повнолітнім (18+)");
                _dateOfBirth = value;
            }
        }

        public int ExperienceYears
        {
            get { return _experienceYears; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Стаж не може бути від'ємним");
                if (value > 60)
                    throw new ArgumentException("Некоректний стаж роботи");
                _experienceYears = value;
            }
        }

        public int? CurrentVehicleId
        {
            get { return _currentVehicleId; }
            set { _currentVehicleId = value; }
        }

        public string FullName => $"{FirstName} {LastName}";
    }
}