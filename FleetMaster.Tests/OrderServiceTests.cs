using System;
using System.Linq;
using Xunit;
using FleetMaster.BusinessLogic.Services;
using FleetMaster.Core.Entities;
using FleetMaster.Core.Enums;
using FleetMaster.Core.Exceptions;
using FleetMaster.Tests.Fakes;

namespace FleetMaster.Tests
{
    public class OrderServiceTests
    {
        [Fact]
        public void CreateOrder_ShouldThrowValidationException_WhenPriceIsNegative()
        {
            var orderRepo = new FakeRepository<Order>();
            var vehicleRepo = new FakeRepository<Vehicle>();
            var driverRepo = new FakeRepository<Driver>();
            var logger = new FakeLogger();

            var service = new OrderService(orderRepo, vehicleRepo, driverRepo, logger);

            var invalidOrder = new Order
            {
                Description = "Bad Order",
                Destination = "Kyiv",
                WeightKg = 100,
                Price = -500
            };

            Assert.Throws<ValidationException>(() => service.CreateOrder(invalidOrder));
            Assert.Contains(logger.Logs, l => l.Contains("Невалідні дані"));
        }

        [Fact]
        public void AssignVehicleToOrder_ShouldThrowEntityNotFound_WhenOrderDoesNotExist()
        {
            var orderRepo = new FakeRepository<Order>();
            var vehicleRepo = new FakeRepository<Vehicle>();
            var driverRepo = new FakeRepository<Driver>();
            var logger = new FakeLogger();
            var service = new OrderService(orderRepo, vehicleRepo, driverRepo, logger);

            var ex = Assert.Throws<EntityNotFoundException>(() => service.AssignVehicleToOrder(999));

            Assert.Contains("Order", ex.Message);
        }

        [Fact]
        public void AssignVehicleToOrder_ShouldThrowBusinessLogicException_WhenNoDriversAvailable()
        {
            var orderRepo = new FakeRepository<Order>();
            var vehicleRepo = new FakeRepository<Vehicle>();
            var driverRepo = new FakeRepository<Driver>();
            var logger = new FakeLogger();
            var service = new OrderService(orderRepo, vehicleRepo, driverRepo, logger);

            var order = new Order { Id = 1, Status = OrderStatus.New, WeightKg = 1000, Destination = "Lviv" };
            orderRepo.Add(order);

            var truck = new Truck
            {
                Id = 1,
                IsOperational = true,
                LoadCapacityKg = 5000,
                Brand = "MAN",
                Model = "TGX",
                LicensePlate = "AA2222AA",
                YearOfManufacture = 2020
            };
            vehicleRepo.Add(truck);

            var ex = Assert.Throws<BusinessLogicException>(() => service.AssignVehicleToOrder(1));
            Assert.Equal("Немає вільних водіїв у штаті.", ex.Message);
        }

        [Fact]
        public void AssignVehicleToOrder_ShouldSuccess_WhenEverythingIsReady()
        {
            var orderRepo = new FakeRepository<Order>();
            var vehicleRepo = new FakeRepository<Vehicle>();
            var driverRepo = new FakeRepository<Driver>();
            var logger = new FakeLogger();
            var service = new OrderService(orderRepo, vehicleRepo, driverRepo, logger);

            orderRepo.Add(new Order { Id = 1, Status = OrderStatus.New, WeightKg = 500, Destination = "Odesa" });

            vehicleRepo.Add(new Truck
            {
                Id = 10,
                IsOperational = true,
                LoadCapacityKg = 1000,
                LicensePlate = "BB0000BB",
                Brand = "Volvo",
                Model = "FH",
                YearOfManufacture = 2021
            });

            driverRepo.Add(new Driver
            {
                Id = 5,
                FirstName = "Good",
                LastName = "Driver",
                ExperienceYears = 5
            });

            service.AssignVehicleToOrder(1);

            var updatedOrder = orderRepo.GetById(1);
            var updatedDriver = driverRepo.GetById(5);

            Assert.Equal(OrderStatus.InProgress, updatedOrder.Status);
            Assert.Equal(10, updatedOrder.AssignedVehicleId);
            Assert.Equal(5, updatedOrder.AssignedDriverId);

            Assert.Equal(10, updatedDriver.CurrentVehicleId);

            Assert.Contains(logger.Logs, l => l.Contains("Успішний розподіл"));
        }
    }
}