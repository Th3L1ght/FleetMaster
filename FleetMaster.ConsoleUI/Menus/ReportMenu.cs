using System;
using System.Diagnostics;
using System.IO;
using FleetMaster.BusinessLogic.Services;
using FleetMaster.ConsoleUI.Utilities;

namespace FleetMaster.ConsoleUI.Menus
{
    public class ReportMenu
    {
        private readonly ReportService _reportService;

        public ReportMenu(ReportService reportService)
        {
            _reportService = reportService;
        }

        public void Show()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== БУХГАЛТЕРІЯ ТА ЗВІТИ ===");
                Console.WriteLine("1. Експорт всіх замовлень (.csv)");
                Console.WriteLine("2. Експорт стану автопарку (.csv)");
                Console.WriteLine("3. Експорт фінансових транзакцій (.csv)");
                Console.WriteLine("0. Назад");

                int choice = InputHelper.ReadInt("Виберіть звіт", 0, 3);

                switch (choice)
                {
                    case 1: GenerateOrderReport(); break;
                    case 2: GenerateFleetReport(); break;
                    case 3: GenerateFinanceReport(); break;
                    case 0: return;
                }
                Console.WriteLine("\nНатисніть будь-яку клавішу...");
                Console.ReadKey();
            }
        }

        private void GenerateOrderReport()
        {
            Console.WriteLine("Генерація звіту по замовленнях...");
            try
            {
                string path = _reportService.ExportOrdersToCsv();
                ShowSuccess(path);
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private void GenerateFleetReport()
        {
            Console.WriteLine("Генерація звіту по автопарку...");
            try
            {
                string path = _reportService.ExportVehiclesToCsv();
                ShowSuccess(path);
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private void GenerateFinanceReport()
        {
            Console.WriteLine("Генерація фінансового звіту...");
            try
            {
                string path = _reportService.ExportTransactionsToCsv();
                ShowSuccess(path);
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private void ShowSuccess(string path)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Успіх! Файл створено: {path}");
            Console.ResetColor();

            Console.WriteLine("Відкрити папку зі звітами? (y/n)");
            if (Console.ReadLine()?.ToLower() == "y")
            {
                Process.Start("explorer.exe", Path.GetDirectoryName(path));
            }
        }

        private void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Помилка при генерації звіту: {message}");
            Console.ResetColor();
        }
    }
}