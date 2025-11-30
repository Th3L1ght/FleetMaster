using System;
using System.Collections.Generic;
using System.Linq;
using FleetMaster.Core.Entities;
using FleetMaster.Core.Enums;
using FleetMaster.Core.Exceptions;
using FleetMaster.Core.Interfaces;

namespace FleetMaster.BusinessLogic.Services
{
    public class FinanceService
    {
        private readonly IRepository<FinancialTransaction> _transactionRepo;
        private readonly IRepository<Order> _orderRepo;
        private readonly IRepository<Driver> _driverRepo;
        private readonly IRepository<MaintenanceRecord> _maintenanceRepo;
        private readonly ILogger _logger;

        private const decimal BaseSalary = 15000m;
        private const decimal BonusPercentage = 0.05m;

        public FinanceService(
            IRepository<FinancialTransaction> transRepo,
            IRepository<Order> orderRepo,
            IRepository<Driver> driverRepo,
            IRepository<MaintenanceRecord> maintRepo,
            ILogger logger)
        {
            _transactionRepo = transRepo;
            _orderRepo = orderRepo;
            _driverRepo = driverRepo;
            _maintenanceRepo = maintRepo;
            _logger = logger;
        }

        public decimal GetCompanyBalance()
        {
            var transactions = _transactionRepo.GetAll();
            decimal income = transactions.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount);
            decimal expenses = transactions.Where(t => t.Type != TransactionType.Income).Sum(t => t.Amount);

            return income - expenses;
        }

        public decimal CalculateDriverSalary(int driverId)
        {
            var driver = _driverRepo.GetById(driverId);
            if (driver == null)
            {
                throw new EntityNotFoundException("Driver", driverId);
            }

            var completedOrders = _orderRepo.Find(o =>
                o.AssignedDriverId == driverId &&
                o.Status == OrderStatus.Completed
            );

            decimal bonuses = 0;
            foreach (var order in completedOrders)
            {
                bonuses += (decimal)order.Price * BonusPercentage;
            }

            decimal seniorityBonus = driver.ExperienceYears * 1000;

            return BaseSalary + bonuses + seniorityBonus;
        }

        public void PaySalary(int driverId)
        {
            decimal currentBalance = GetCompanyBalance();
            decimal amountToPay = CalculateDriverSalary(driverId);

            if (currentBalance < amountToPay)
            {
                _logger.LogError($"Спроба виплати зарплати при недостатньому балансі. Треба: {amountToPay}, Є: {currentBalance}");
                throw new FinancialException("Недостатньо коштів на балансі компанії для виплати зарплати.", amountToPay);
            }

            var driver = _driverRepo.GetById(driverId);
            if (driver == null) throw new EntityNotFoundException("Driver", driverId);

            var transaction = new FinancialTransaction
            {
                Amount = amountToPay,
                Type = TransactionType.SalaryPayment,
                Description = $"Зарплата: {driver.FullName}",
                TransactionDate = DateTime.Now,
                RelatedDriverId = driverId,
                CreatedAt = DateTime.Now
            };

            _transactionRepo.Add(transaction);
            _logger.LogInfo($"Виплачено зарплату {amountToPay} грн водію {driver.FullName}");
        }

        public void GenerateFinancialReport()
        {
            var repairs = _maintenanceRepo.GetAll();
            foreach (var repair in repairs)
            {
                var exists = _transactionRepo.Find(t => t.Description.Contains($"Ремонт #{repair.Id}")).Any();
                if (!exists)
                {
                    _transactionRepo.Add(new FinancialTransaction
                    {
                        Amount = (decimal)repair.Cost,
                        Type = TransactionType.Expense,
                        Description = $"Авто-синхронізація: Ремонт #{repair.Id}",
                        TransactionDate = repair.ServiceDate,
                        CreatedAt = DateTime.Now
                    });
                }
            }

            var finishedOrders = _orderRepo.Find(o => o.Status == OrderStatus.Completed);
            foreach (var order in finishedOrders)
            {
                var exists = _transactionRepo.Find(t => t.RelatedOrderId == order.Id).Any();
                if (!exists)
                {
                    _transactionRepo.Add(new FinancialTransaction
                    {
                        Amount = (decimal)order.Price,
                        Type = TransactionType.Income,
                        Description = $"Оплата замовлення #{order.Id}",
                        TransactionDate = DateTime.Now,
                        RelatedOrderId = order.Id,
                        CreatedAt = DateTime.Now
                    });
                }
            }
        }

        public IEnumerable<FinancialTransaction> GetLastTransactions(int count)
        {
            return _transactionRepo.GetAll()
                .OrderByDescending(t => t.TransactionDate)
                .Take(count);
        }
    }
}