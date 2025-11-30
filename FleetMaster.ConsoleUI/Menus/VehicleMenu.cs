using FleetMaster.BusinessLogic.Services;
using FleetMaster.Core.Entities;
using FleetMaster.Core.Enums;
using FleetMaster.ConsoleUI.Utilities;

namespace FleetMaster.ConsoleUI.Menus
{
    public class VehicleMenu
    {
        private readonly VehicleService _vehicleService;

        public VehicleMenu(VehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        public void Show()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== УПРАВЛІННЯ ТРАНСПОРТОМ ===");
                Console.WriteLine("1. Список всіх машин");
                Console.WriteLine("2. Додати нову машину");
                Console.WriteLine("3. Знайти машину за ID");
                Console.WriteLine("0. Назад");

                int choice = InputHelper.ReadInt("Виберіть дію", 0, 3);

                switch (choice)
                {
                    case 1: ShowAll(); break;
                    case 2: AddNew(); break;
                    case 3: FindById(); break;
                    case 0: return;
                }
                Console.WriteLine("\nНатисніть будь-яку клавішу...");
                Console.ReadKey();
            }
        }

        private void ShowAll()
        {
            var vehicles = _vehicleService.GetAllVehicles();
            foreach (var v in vehicles)
            {
                Console.WriteLine($"ID: {v.Id} | {v.GetDescription()} | Load: {v.LoadCapacityKg}kg");
            }
        }

        private void AddNew()
        {
            Console.WriteLine("\n--- Додавання машини ---");
            Console.WriteLine("1. Вантажівка (Truck)");
            Console.WriteLine("2. Фургон (Van)");
            int type = InputHelper.ReadInt("Тип авто", 1, 2);

            Vehicle vehicle;

            if (type == 1)
            {
                vehicle = new Truck
                {
                    Type = VehicleType.Truck,
                    CargoVolume = InputHelper.ReadDouble("Об'єм кузова (м3)"),
                    HasTrailer = InputHelper.ReadInt("Є причіп? (1-Так, 0-Ні)", 0, 1) == 1
                };
            }
            else
            {
                vehicle = new CargoVan
                {
                    Type = VehicleType.Van,
                    IsRefrigerated = InputHelper.ReadInt("Холодильник? (1-Так, 0-Ні)", 0, 1) == 1
                };
            }

            vehicle.Brand = InputHelper.ReadString("Марка");
            vehicle.Model = InputHelper.ReadString("Модель");
            vehicle.LicensePlate = InputHelper.ReadString("Номерний знак");
            vehicle.YearOfManufacture = InputHelper.ReadInt("Рік випуску", 1980, 2025);
            vehicle.LoadCapacityKg = InputHelper.ReadDouble("Вантажопідйомність (кг)");
            vehicle.FuelConsumptionPer100Km = InputHelper.ReadDouble("Витрата палива");

            try
            {
                _vehicleService.AddVehicle(vehicle);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Машину успішно додано!");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Помилка: {ex.Message}");
                Console.ResetColor();
            }
        }

        private void FindById()
        {
            int id = InputHelper.ReadInt("Введіть ID", 1, 10000);
            try
            {
                var v = _vehicleService.GetById(id);
                Console.WriteLine($"Знайдено: {v.GetDescription()}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}