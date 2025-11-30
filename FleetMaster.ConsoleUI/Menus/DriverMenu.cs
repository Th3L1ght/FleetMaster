using FleetMaster.Core.Entities;
using FleetMaster.Core.Interfaces;
using FleetMaster.ConsoleUI.Utilities;

namespace FleetMaster.ConsoleUI.Menus
{
    public class DriverMenu
    {
        private readonly IRepository<Driver> _driverRepository;

        public DriverMenu(IRepository<Driver> driverRepository)
        {
            _driverRepository = driverRepository;
        }

        public void Show()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== ВІДДІЛ КАДРІВ (ВОДІЇ) ===");
                Console.WriteLine("1. Список водіїв");
                Console.WriteLine("2. Найняти нового водія");
                Console.WriteLine("3. Звільнити водія (Видалити)");
                Console.WriteLine("4. Оновити ліцензію");
                Console.WriteLine("0. Назад");

                int choice = InputHelper.ReadInt("Дія", 0, 4);

                switch (choice)
                {
                    case 1: ListDrivers(); break;
                    case 2: HireDriver(); break;
                    case 3: FireDriver(); break;
                    case 4: UpdateLicense(); break;
                    case 0: return;
                }
                Console.WriteLine("\nНатисніть будь-яку клавішу...");
                Console.ReadKey();
            }
        }

        private void ListDrivers()
        {
            var drivers = _driverRepository.GetAll();
            Console.WriteLine($"\nВсього водіїв: {drivers.Count()}");
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine("{0,-5} | {1,-20} | {2,-10} | {3,-10}", "ID", "ПІБ", "Стаж", "Статус");
            Console.WriteLine("---------------------------------------------------");

            foreach (var d in drivers)
            {
                string status = d.CurrentVehicleId.HasValue ? $"У рейсі (Авто #{d.CurrentVehicleId})" : "Вільний";
                Console.WriteLine("{0,-5} | {1,-20} | {2,-10} | {3,-10}", d.Id, d.FullName, d.ExperienceYears + " р.", status);
            }
        }

        private void HireDriver()
        {
            Console.WriteLine("\n--- Наймання водія ---");
            var driver = new Driver
            {
                FirstName = InputHelper.ReadString("Ім'я"),
                LastName = InputHelper.ReadString("Прізвище"),
                DateOfBirth = DateTime.Now.AddYears(-InputHelper.ReadInt("Вік", 18, 70)),
                ExperienceYears = InputHelper.ReadInt("Стаж (років)", 0, 50),
                LicenseNumber = InputHelper.ReadString("Номер прав"),
                CreatedAt = DateTime.Now
            };

            _driverRepository.Add(driver);
            Console.WriteLine("Водія успішно найнято!");
        }

        private void FireDriver()
        {
            int id = InputHelper.ReadInt("Введіть ID водія для звільнення", 1, 9999);
            var driver = _driverRepository.GetById(id);
            if (driver != null)
            {
                _driverRepository.Delete(id);
                Console.WriteLine("Водія видалено з бази.");
            }
            else
            {
                Console.WriteLine("Водія не знайдено.");
            }
        }

        private void UpdateLicense()
        {
            int id = InputHelper.ReadInt("Введіть ID водія", 1, 9999);
            var driver = _driverRepository.GetById(id);
            if (driver != null)
            {
                string newLicense = InputHelper.ReadString("Новий номер прав");
                driver.LicenseNumber = newLicense;
                _driverRepository.Update(driver); // Важливо: метод Update має бути реалізований в Repository
                Console.WriteLine("Дані оновлено.");
            }
        }
    }
}