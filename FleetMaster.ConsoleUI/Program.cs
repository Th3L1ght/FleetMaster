using System;
using FleetMaster.BusinessLogic.Services;
using FleetMaster.ConsoleUI.Menus;
using FleetMaster.Core.Entities;
using FleetMaster.Infrastructure.Data;
using FleetMaster.Infrastructure.Logging;
using FleetMaster.Infrastructure.Repositories;

namespace FleetMaster.ConsoleUI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Title = "FleetMaster ERP v3.0 (Full Enterprise)";

            var logger = new FileLogger();
            logger.LogInfo("=== СИСТЕМА ЗАПУЩЕНА ===");

            var vehicleRepo = new FileRepository<Vehicle>("vehicles.json");
            var driverRepo = new FileRepository<Driver>("drivers.json");
            var orderRepo = new FileRepository<Order>("orders.json");
            var maintRepo = new FileRepository<MaintenanceRecord>("maintenance.json");
            var transactionRepo = new FileRepository<FinancialTransaction>("transactions.json");

            var seeder = new DataSeeder(driverRepo, vehicleRepo, orderRepo);
            seeder.SeedAll();

            var vehicleService = new VehicleService(vehicleRepo, logger);

            var orderService = new OrderService(orderRepo, vehicleRepo, driverRepo, logger);

            var maintService = new MaintenanceService(maintRepo, vehicleRepo, logger);

            var reportService = new ReportService(orderRepo, vehicleRepo, driverRepo, transactionRepo, logger);

            var financeService = new FinanceService(transactionRepo, orderRepo, driverRepo, maintRepo, logger);

            var vehicleMenu = new VehicleMenu(vehicleService);
            var orderMenu = new OrderMenu(orderService);
            var driverMenu = new DriverMenu(driverRepo);
            var maintMenu = new MaintenanceMenu(maintService);
            var reportMenu = new ReportMenu(reportService);
            var financeMenu = new FinanceMenu(financeService);

            bool isRunning = true;
            while (isRunning)
            {
                Console.Clear();
                DrawHeader();

                Console.WriteLine("1. Транспортний відділ (Машини)");
                Console.WriteLine("2. Логістичний відділ (Замовлення)");
                Console.WriteLine("3. Відділ кадрів (Водії)");
                Console.WriteLine("4. СТО / Технічний сервіс");
                Console.WriteLine("5. Звіти та Експорт (CSV)");
                Console.WriteLine("6. Фінансовий відділ (Зарплати/Баланс)");
                Console.WriteLine("0. Вихід");
                Console.WriteLine("------------------------------------------");

                Console.Write("Ваш вибір: ");
                string input = Console.ReadLine();

                try
                {
                    switch (input)
                    {
                        case "1":
                            vehicleMenu.Show();
                            break;
                        case "2":
                            orderMenu.Show();
                            break;
                        case "3":
                            driverMenu.Show();
                            break;
                        case "4":
                            maintMenu.Show();
                            break;
                        case "5":
                            reportMenu.Show();
                            break;
                        case "6":
                            financeMenu.Show();
                            break;
                        case "0":
                            logger.LogInfo("Завершення роботи користувачем.");
                            isRunning = false;
                            Console.WriteLine("До побачення!");
                            break;
                        default:
                            ShowError("Невірний вибір. Спробуйте ще раз.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError("Критична помилка в головному меню", ex);
                    ShowError($"Помилка: {ex.Message}\nДеталі записані в лог.");
                }
            }
        }

        static void DrawHeader()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==========================================");
            Console.WriteLine("       FleetMaster ERP System v3.0        ");
            Console.WriteLine("==========================================");
            Console.ResetColor();
        }

        static void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
            Console.WriteLine("Натисніть Enter...");
            Console.ReadLine();
        }
    }
}