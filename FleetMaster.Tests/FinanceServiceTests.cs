using System;
using Xunit;
using FleetMaster.BusinessLogic.Services;
using FleetMaster.Core.Entities;
using FleetMaster.Core.Enums;
using FleetMaster.Tests.Fakes;

namespace FleetMaster.Tests
{
    public class FinanceServiceTests
    {
        [Fact]
        public void CalculateDriverSalary_ShouldReturnBaseSalary_WhenDriverHasNoExperienceAndNoOrders()
        {
            var transRepo = new FakeRepository<FinancialTransaction>();
            var orderRepo = new FakeRepository<Order>();
            var driverRepo = new FakeRepository<Driver>();
            var maintRepo = new FakeRepository<MaintenanceRecord>();
            var logger = new FakeLogger();

            var service = new FinanceService(transRepo, orderRepo, driverRepo, maintRepo, logger);

            var newbieDriver = new Driver
            {
                Id = 1,
                FirstName = "Ivan",
                LastName = "Novak",
                ExperienceYears = 0
            };
            driverRepo.Add(newbieDriver);

            decimal salary = service.CalculateDriverSalary(1);

            Assert.Equal(15000m, salary);
        }

        [Fact]
        public void CalculateDriverSalary_ShouldIncludeSeniorityBonus_WhenDriverIsExperienced()
        {
            var transRepo = new FakeRepository<FinancialTransaction>();
            var orderRepo = new FakeRepository<Order>();
            var driverRepo = new FakeRepository<Driver>();
            var maintRepo = new FakeRepository<MaintenanceRecord>();
            var logger = new FakeLogger();

            var service = new FinanceService(transRepo, orderRepo, driverRepo, maintRepo, logger);

            var proDriver = new Driver
            {
                Id = 2,
                ExperienceYears = 5
            };
            driverRepo.Add(proDriver);

            decimal salary = service.CalculateDriverSalary(2);

            Assert.Equal(20000m, salary);
        }

        [Fact]
        public void CalculateDriverSalary_ShouldIncludeOrderBonuses()
        {
            var transRepo = new FakeRepository<FinancialTransaction>();
            var orderRepo = new FakeRepository<Order>();
            var driverRepo = new FakeRepository<Driver>();
            var maintRepo = new FakeRepository<MaintenanceRecord>();
            var logger = new FakeLogger();

            var service = new FinanceService(transRepo, orderRepo, driverRepo, maintRepo, logger);

            var driver = new Driver { Id = 3, ExperienceYears = 0 };
            driverRepo.Add(driver);

            var order = new Order
            {
                Id = 100,
                AssignedDriverId = 3,
                Status = OrderStatus.Completed,
                Price = 10000
            };
            orderRepo.Add(order);

            decimal salary = service.CalculateDriverSalary(3);

            Assert.Equal(15500m, salary);
        }

        [Fact]
        public void GetCompanyBalance_ShouldReturnPositive_WhenOnlyIncomeExists()
        {
            var transRepo = new FakeRepository<FinancialTransaction>();
            var orderRepo = new FakeRepository<Order>();
            var driverRepo = new FakeRepository<Driver>();
            var maintRepo = new FakeRepository<MaintenanceRecord>();
            var logger = new FakeLogger();

            var service = new FinanceService(transRepo, orderRepo, driverRepo, maintRepo, logger);

            transRepo.Add(new FinancialTransaction
            {
                Type = TransactionType.Income,
                Amount = 5000
            });

            transRepo.Add(new FinancialTransaction
            {
                Type = TransactionType.Income,
                Amount = 2000
            });

            decimal balance = service.GetCompanyBalance();

            Assert.Equal(7000m, balance);
        }

        [Fact]
        public void GetCompanyBalance_ShouldSubtractExpenses()
        {
            var transRepo = new FakeRepository<FinancialTransaction>();
            var orderRepo = new FakeRepository<Order>();
            var driverRepo = new FakeRepository<Driver>();
            var maintRepo = new FakeRepository<MaintenanceRecord>();
            var logger = new FakeLogger();

            var service = new FinanceService(transRepo, orderRepo, driverRepo, maintRepo, logger);

            transRepo.Add(new FinancialTransaction { Type = TransactionType.Income, Amount = 10000 });
            transRepo.Add(new FinancialTransaction { Type = TransactionType.Expense, Amount = 3000 });
            transRepo.Add(new FinancialTransaction { Type = TransactionType.SalaryPayment, Amount = 2000 });

            decimal balance = service.GetCompanyBalance();

            Assert.Equal(5000m, balance);
        }

        [Fact]
        public void PaySalary_ShouldCreateTransactionAndLog()
        {
            var transRepo = new FakeRepository<FinancialTransaction>();
            var driverRepo = new FakeRepository<Driver>();
            var orderRepo = new FakeRepository<Order>();
            var maintRepo = new FakeRepository<MaintenanceRecord>();
            var logger = new FakeLogger();

            var service = new FinanceService(transRepo, orderRepo, driverRepo, maintRepo, logger);

            var driver = new Driver
            {
                Id = 1,
                FirstName = "Test",
                LastName = "Driver",
                ExperienceYears = 0
            };
            driverRepo.Add(driver);

            transRepo.Add(new FinancialTransaction
            {
                Type = TransactionType.Income,
                Amount = 100000
            });

            service.PaySalary(1);

            var transactions = transRepo.GetAll();
            Assert.Equal(2, transactions.Count());

            var salaryTrans = transactions.Last();
            Assert.Equal(TransactionType.SalaryPayment, salaryTrans.Type);
            Assert.Equal(15000m, salaryTrans.Amount);

            Assert.Contains(logger.Logs, l => l.Contains("Виплачено зарплату"));
        }

        [Fact]
        public void GenerateFinancialReport_ShouldSyncOrdersToTransactions()
        {
            var transRepo = new FakeRepository<FinancialTransaction>();
            var orderRepo = new FakeRepository<Order>();
            var driverRepo = new FakeRepository<Driver>();
            var maintRepo = new FakeRepository<MaintenanceRecord>();
            var logger = new FakeLogger();

            var service = new FinanceService(transRepo, orderRepo, driverRepo, maintRepo, logger);

            orderRepo.Add(new Order { Id = 99, Status = OrderStatus.Completed, Price = 5000 });

            service.GenerateFinancialReport();

            var transactions = transRepo.GetAll();
            Assert.Single(transactions);
            Assert.Equal(5000m, transactions.First().Amount);
            Assert.Equal(TransactionType.Income, transactions.First().Type);
        }
    }
}