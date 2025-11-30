using System;
using System.Collections.Generic;
using System.Linq;
using FleetMaster.BusinessLogic.Validators;
using FleetMaster.Core.Entities;
using FleetMaster.Core.Exceptions;
using FleetMaster.Core.Interfaces;

namespace FleetMaster.BusinessLogic.Services
{
    public class VehicleService
    {
        private readonly IRepository<Vehicle> _vehicleRepository;
        private readonly ILogger _logger;

        public VehicleService(IRepository<Vehicle> vehicleRepository, ILogger logger)
        {
            _vehicleRepository = vehicleRepository;
            _logger = logger;
        }

        public void AddVehicle(Vehicle vehicle)
        {
            _logger.LogInfo($"Спроба додати машину: {vehicle.Brand} {vehicle.Model} ({vehicle.LicensePlate})");

            var validator = new VehicleValidator();
            var validationErrors = validator.Validate(vehicle);

            if (validationErrors.Count > 0)
            {
                string errorMessages = string.Join("; ", validationErrors);
                _logger.LogWarning($"Валідація не пройшла. Помилки: {errorMessages}");

                throw new ValidationException($"Помилка валідації авто: {errorMessages}");
            }

            var existing = _vehicleRepository.Find(v => v.LicensePlate == vehicle.LicensePlate);
            if (existing.Any())
            {
                _logger.LogWarning($"Дублікат: {vehicle.LicensePlate}");
                throw new BusinessLogicException($"Машина з номером {vehicle.LicensePlate} вже існує в базі.");
            }

            try
            {
                _vehicleRepository.Add(vehicle);
                _logger.LogInfo($"Машина ID:{vehicle.Id} успішно створена.");
            }
            catch (Exception ex)
            {
                _logger.LogError("Критична помилка при записі машини", ex);
                throw new FleetMasterException("Помилка файлової системи при збереженні.", ex);
            }
        }

        public IEnumerable<Vehicle> GetAllVehicles()
        {
            return _vehicleRepository.GetAll();
        }

        public Vehicle GetById(int id)
        {
            var vehicle = _vehicleRepository.GetById(id);
            if (vehicle == null)
            {
                _logger.LogWarning($"Запит машини ID:{id} - не знайдено.");
                throw new EntityNotFoundException("Vehicle", id);
            }
            return vehicle;
        }

        public IEnumerable<Vehicle> FindSuitableVehicles(double cargoWeightKg)
        {
            _logger.LogInfo($"Пошук авто для вантажу {cargoWeightKg} кг...");

            return _vehicleRepository.Find(v =>
                v.IsOperational &&
                v.LoadCapacityKg >= cargoWeightKg
            );
        }
    }
}