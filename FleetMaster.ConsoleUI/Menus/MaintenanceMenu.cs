using System;
using FleetMaster.BusinessLogic.Services;
using FleetMaster.Core.Entities;
using FleetMaster.ConsoleUI.Utilities;

namespace FleetMaster.ConsoleUI.Menus
{
    public class MaintenanceMenu
    {
        private readonly MaintenanceService _service;

        public MaintenanceMenu(MaintenanceService service)
        {
            _service = service;
        }

        public void Show()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== СТО / СЕРВІС ===");
                Console.WriteLine("1. Додати запис про ремонт");
                Console.WriteLine("2. Історія ремонтів авто");
                Console.WriteLine("3. Загальна вартість обслуговування");
                Console.WriteLine("4. Повернути машину з ремонту (Розблокувати)");
                Console.WriteLine("0. Назад");

                int choice = InputHelper.ReadInt("Вибір", 0, 4);

                switch (choice)
                {
                    case 1: AddRepair(); break;
                    case 2: ShowHistory(); break;
                    case 3: ShowCost(); break;
                    case 4: FinishRepair(); break;
                    case 0: return;
                }
                Console.WriteLine("Натисніть будь-яку клавішу...");
                Console.ReadKey();
            }
        }

        private void AddRepair()
        {
            Console.WriteLine("\n--- Новий запис СТО ---");
            int carId = InputHelper.ReadInt("ID Машини", 1, 9999);

            var record = new MaintenanceRecord
            {
                VehicleId = carId,
                Description = InputHelper.ReadString("Опис робіт"),
                Cost = InputHelper.ReadDouble("Вартість (грн)"),
                MileageAtService = InputHelper.ReadInt("Поточний пробіг (км)", 0, 2000000),
                ServiceDate = DateTime.Now,
                IsScheduled = InputHelper.ReadInt("Це планове ТО? (1-Так, 0-Ні/Ремонт)", 0, 1) == 1
            };

            try
            {
                _service.AddRecord(record);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Запис успішно додано.");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Помилка: " + ex.Message);
                Console.ResetColor();
            }
        }

        private void ShowHistory()
        {
            int carId = InputHelper.ReadInt("ID Машини", 1, 9999);
            var history = _service.GetHistory(carId);

            Console.WriteLine($"\nІсторія обслуговування авто #{carId}:");
            foreach (var h in history)
            {
                Console.WriteLine(h);
            }
        }

        private void ShowCost()
        {
            int carId = InputHelper.ReadInt("ID Машини", 1, 9999);
            double total = _service.GetTotalMaintenanceCost(carId);
            Console.WriteLine($"\nВсього витрачено на це авто: {total} грн");
        }

        private void FinishRepair()
        {
            int carId = InputHelper.ReadInt("ID Машини", 1, 9999);
            _service.FinishRepair(carId);
            Console.WriteLine("Статус машини оновлено на 'Робоча'.");
        }
    }
}