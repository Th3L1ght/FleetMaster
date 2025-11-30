using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FleetMaster.BusinessLogic.Mappers;
using FleetMaster.Core.Entities;
using FleetMaster.Core.Interfaces;

namespace FleetMaster.BusinessLogic.Services
{
    public class ReportService
    {
        private readonly IRepository<Order> _orderRepo;
        private readonly IRepository<Vehicle> _vehicleRepo;
        private readonly IRepository<Driver> _driverRepo;
        private readonly IRepository<FinancialTransaction> _transRepo;
        private readonly ILogger _logger;

        private readonly string _reportsPath = "reports";

        public ReportService(
            IRepository<Order> orderRepo,
            IRepository<Vehicle> vehicleRepo,
            IRepository<Driver> driverRepo,
            IRepository<FinancialTransaction> transRepo,
            ILogger logger)
        {
            _orderRepo = orderRepo;
            _vehicleRepo = vehicleRepo;
            _driverRepo = driverRepo;
            _transRepo = transRepo;
            _logger = logger;

            if (!Directory.Exists(_reportsPath))
                Directory.CreateDirectory(_reportsPath);
        }

        public string ExportOrdersToCsv()
        {
            _logger.LogInfo("Генерація звіту по замовленнях...");
            var sb = new StringBuilder();
            sb.AppendLine("ID,Description,Destination,Price,Status,Driver,Vehicle");

            var orders = _orderRepo.GetAll();
            foreach (var order in orders)
            {
                var vehicle = order.AssignedVehicleId.HasValue ? _vehicleRepo.GetById(order.AssignedVehicleId.Value) : null;
                var driver = order.AssignedDriverId.HasValue ? _driverRepo.GetById(order.AssignedDriverId.Value) : null;
                var dto = OrderMapper.ToReportDto(order, vehicle, driver);

                sb.AppendLine($"{dto.OrderId},{dto.Description},{dto.ClientDestination},{dto.Price},{dto.OrderStatus},{dto.DriverName},{dto.VehiclePlate}");
            }

            string fileName = $"Orders_Report_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            string fullPath = Path.Combine(_reportsPath, fileName);
            File.WriteAllText(fullPath, sb.ToString(), Encoding.UTF8);

            _logger.LogInfo($"Звіт збережено: {fullPath}");
            return fullPath;
        }

        public string ExportVehiclesToCsv()
        {
            _logger.LogInfo("Генерація звіту по автопарку...");
            var sb = new StringBuilder();
            sb.AppendLine("Brand/Model,Plate,Type,Year,Status");

            var vehicles = _vehicleRepo.GetAll();
            foreach (var v in vehicles)
            {
                var dto = VehicleMapper.ToDto(v);
                sb.AppendLine($"{dto.FullName},{dto.LicensePlate},{dto.Type},{dto.Year},{dto.Status}");
            }

            string fileName = $"Fleet_Report_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            string fullPath = Path.Combine(_reportsPath, fileName);
            File.WriteAllText(fullPath, sb.ToString(), Encoding.UTF8);

            return fullPath;
        }

        public string ExportTransactionsToCsv()
        {
            _logger.LogInfo("Генерація фінансового звіту...");
            var sb = new StringBuilder();
            sb.AppendLine("ID,Date,Type,Amount,Description,Formatted");

            var transactions = _transRepo.GetAll();
            foreach (var t in transactions)
            {
                var dto = TransactionMapper.ToDto(t);

                sb.AppendLine($"{dto.Id},{dto.Date},{dto.Type},{dto.Amount},{dto.Description},{dto.FormattedAmount}");
            }

            string fileName = $"Finance_Report_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            string fullPath = Path.Combine(_reportsPath, fileName);
            File.WriteAllText(fullPath, sb.ToString(), Encoding.UTF8);

            return fullPath;
        }
    }
}