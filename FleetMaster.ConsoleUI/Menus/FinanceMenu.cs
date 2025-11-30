using System;
using FleetMaster.BusinessLogic.Services;
using FleetMaster.ConsoleUI.Utilities;

namespace FleetMaster.ConsoleUI.Menus
{
    public class FinanceMenu
    {
        private readonly FinanceService _financeService;

        public FinanceMenu(FinanceService financeService)
        {
            _financeService = financeService;
        }

        public void Show()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== ФІНАНСОВИЙ ВІДДІЛ ===");

                _financeService.GenerateFinancialReport();

                decimal balance = _financeService.GetCompanyBalance();
                Console.WriteLine($"БАЛАНС КОМПАНІЇ: {balance:N2} UAH");
                Console.WriteLine("-----------------------------");

                Console.WriteLine("1. Історія транзакцій");
                Console.WriteLine("2. Розрахувати зарплату водія (Калькулятор)");
                Console.WriteLine("3. Виплатити зарплату (Створити транзакцію)");
                Console.WriteLine("0. Назад");

                int choice = InputHelper.ReadInt("Вибір", 0, 3);

                switch (choice)
                {
                    case 1: ShowTransactions(); break;
                    case 2: CalculateSalary(); break;
                    case 3: PaySalary(); break;
                    case 0: return;
                }
                Console.WriteLine("\nНатисніть будь-яку клавішу...");
                Console.ReadKey();
            }
        }

        private void ShowTransactions()
        {
            Console.WriteLine("\n--- Останні 10 транзакцій ---");
            var transactions = _financeService.GetLastTransactions(10);
            foreach (var t in transactions)
            {
                if (t.Type == FleetMaster.Core.Enums.TransactionType.Income)
                    Console.ForegroundColor = ConsoleColor.Green;
                else
                    Console.ForegroundColor = ConsoleColor.Red;

                Console.WriteLine(t);
            }
            Console.ResetColor();
        }

        private void CalculateSalary()
        {
            int driverId = InputHelper.ReadInt("ID Водія", 1, 9999);
            try
            {
                decimal salary = _financeService.CalculateDriverSalary(driverId);
                Console.WriteLine($"Розрахункова зарплата для водія #{driverId}: {salary:N2} грн");
                Console.WriteLine("(Включає ставку 15000 + 5% від замовлень + бонус за стаж)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }

        private void PaySalary()
        {
            int driverId = InputHelper.ReadInt("ID Водія для виплати", 1, 9999);
            Console.WriteLine("Ви впевнені? Це спише кошти з балансу. (y/n)");
            if (Console.ReadLine() == "y")
            {
                try
                {
                    _financeService.PaySalary(driverId);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Транзакцію успішно проведено!");
                    Console.ResetColor();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Помилка: {ex.Message}");
                }
            }
        }
    }
}