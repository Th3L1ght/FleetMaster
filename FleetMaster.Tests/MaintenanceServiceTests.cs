using System;
using Xunit;
using FleetMaster.BusinessLogic.Services;
using FleetMaster.Core.Entities;
using FleetMaster.Tests.Fakes;

namespace FleetMaster.Tests
{
    public class MaintenanceServiceTests
    {
        [Fact]
        public void AddRecord_ShouldMakeCarNonOperational_WhenRepairIsNotScheduled()
        {
            // Arrange
            var maintRepo = new FakeRepository<MaintenanceRecord>();
            var vehicleRepo = new FakeRepository<Vehicle>();
            var logger = new FakeLogger();

            var service = new MaintenanceService(maintRepo, vehicleRepo, logger);

            var car = new Truck { Id = 1, IsOperational = true, Brand = "Test" };
            vehicleRepo.Add(car);

            var record = new MaintenanceRecord
            {
                VehicleId = 1,
                IsScheduled = false,
                Cost = 1000
            };
            service.AddRecord(record);

            var updatedCar = vehicleRepo.GetById(1);
            Assert.False(updatedCar.IsOperational);
            Assert.Contains(logger.Logs, l => l.Contains("виведена з експлуатації"));
        }

        [Fact]
        public void FinishRepair_ShouldMakeCarOperational()
        {
            var maintRepo = new FakeRepository<MaintenanceRecord>();
            var vehicleRepo = new FakeRepository<Vehicle>();
            var logger = new FakeLogger();
            var service = new MaintenanceService(maintRepo, vehicleRepo, logger);

            var car = new Truck
            {
                Id = 5,
                IsOperational = false,
                Brand = "Test",
                Model = "Test",
                LicensePlate = "TEST1111",
                YearOfManufacture = 2020,
                LoadCapacityKg = 1000,
                FuelConsumptionPer100Km = 10
            };
            vehicleRepo.Add(car);

            service.FinishRepair(5);

            var updatedCar = vehicleRepo.GetById(5);
            Assert.NotNull(updatedCar);
            Assert.True(updatedCar.IsOperational);
        }
    }
}