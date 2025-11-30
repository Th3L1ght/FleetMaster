using System;
using System.Collections.Generic;
using System.Linq;
using FleetMaster.Core.Entities;
using FleetMaster.Core.Exceptions;
using FleetMaster.Core.Interfaces;

namespace FleetMaster.BusinessLogic.Services
{
    public class MaintenanceService
    {
        private readonly IRepository<MaintenanceRecord> _repo;
        private readonly IRepository<Vehicle> _vehicleRepo;
        private readonly ILogger _logger;

        public MaintenanceService(
            IRepository<MaintenanceRecord> repo,
            IRepository<Vehicle> vehicleRepo,
            ILogger logger)
        {
            _repo = repo;
            _vehicleRepo = vehicleRepo;
            _logger = logger;
        }

        public void AddRecord(MaintenanceRecord record)
        {
            _logger.LogInfo($"Додавання ремонту для авто #{record.VehicleId}");

            var vehicle = _vehicleRepo.GetById(record.VehicleId);
            if (vehicle == null)
            {
                throw new EntityNotFoundException("Vehicle", record.VehicleId);
            }

            if (record.Cost < 0)
            {
                throw new ValidationException("Вартість ремонту не може бути від'ємною.");
            }

            record.CreatedAt = DateTime.Now;
            _repo.Add(record);

            if (!record.IsScheduled)
            {
                vehicle.IsOperational = false;
                _vehicleRepo.Update(vehicle);
                _logger.LogWarning($"Машина #{vehicle.Id} виведена з експлуатації (Аварійний ремонт).");
            }

            _logger.LogInfo($"Запис ремонту створено. Сума: {record.Cost}");
        }

        public IEnumerable<MaintenanceRecord> GetHistory(int vehicleId)
        {
            var vehicle = _vehicleRepo.GetById(vehicleId);
            if (vehicle == null) throw new EntityNotFoundException("Vehicle", vehicleId);

            return _repo.Find(x => x.VehicleId == vehicleId);
        }

        public double GetTotalMaintenanceCost(int vehicleId)
        {
            var records = GetHistory(vehicleId);
            return records.Sum(x => x.Cost);
        }

        public void FinishRepair(int vehicleId)
        {
            var vehicle = _vehicleRepo.GetById(vehicleId);
            if (vehicle == null)
            {
                throw new EntityNotFoundException("Vehicle", vehicleId);
            }

            vehicle.IsOperational = true;
            _vehicleRepo.Update(vehicle);
            _logger.LogInfo($"Машина #{vehicleId} повернута в стрій.");
        }
    }
}