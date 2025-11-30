using System;
using Xunit;
using FleetMaster.BusinessLogic.Services;
using FleetMaster.Core.Entities;
using FleetMaster.Tests.Fakes;
using FleetMaster.Core.Exceptions;

namespace FleetMaster.Tests
{
    public class VehicleServiceTests
    {
        [Fact]
        public void AddVehicle_ShouldLogInfo_WhenSuccess()
        {
            var fakeRepo = new FakeRepository<Vehicle>();
            var fakeLogger = new FakeLogger();
            var service = new VehicleService(fakeRepo, fakeLogger);

            var newVehicle = new Truck
            {
                Brand = "Volvo",
                Model = "FH",
                YearOfManufacture = 2020,
                LicensePlate = "AA1111AA",
                LoadCapacityKg = 20000,
                FuelConsumptionPer100Km = 30,
                CargoVolume = 80,
                IsOperational = true
            };

            service.AddVehicle(newVehicle);

            var savedVehicle = service.GetById(1);
            Assert.NotNull(savedVehicle);
            Assert.Contains(fakeLogger.Logs, log => log.Contains("успішно створена"));
        }

        [Fact]
        public void AddVehicle_ShouldLogError_WhenYearIsInvalid()
        {
            var fakeRepo = new FakeRepository<Vehicle>();
            var fakeLogger = new FakeLogger();
            var service = new VehicleService(fakeRepo, fakeLogger);

            var oldCar = new Truck
            {
                Brand = "Ford",
                Model = "OldTimer",
                LicensePlate = "AA0000AA",
                YearOfManufacture = 1960,
                LoadCapacityKg = 5000,
                FuelConsumptionPer100Km = 20
            };

            var exception = Assert.Throws<ValidationException>(() => service.AddVehicle(oldCar));

            Assert.Contains(fakeLogger.Logs, log => log.Contains("Валідація не пройшла"));
        }
    }
}