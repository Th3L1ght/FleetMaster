using System;
using System.Linq;
using System.Collections.Generic;
using FleetMaster.BusinessLogic.Validators;
using FleetMaster.Core.Entities;
using FleetMaster.Core.Enums;
using FleetMaster.Core.Exceptions;
using FleetMaster.Core.Interfaces;

namespace FleetMaster.BusinessLogic.Services
{
    public class OrderService
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<Vehicle> _vehicleRepository;
        private readonly IRepository<Driver> _driverRepository;
        private readonly ILogger _logger;

        public OrderService(
            IRepository<Order> orderRepo,
            IRepository<Vehicle> vehicleRepo,
            IRepository<Driver> driverRepo,
            ILogger logger)
        {
            _orderRepository = orderRepo;
            _vehicleRepository = vehicleRepo;
            _driverRepository = driverRepo;
            _logger = logger;
        }

        public Order CreateOrder(Order order)
        {
            _logger.LogInfo($"Створення замовлення: {order.Description}, Вага: {order.WeightKg}");

            var validator = new OrderValidator();
            var errors = validator.Validate(order);

            if (errors.Count > 0)
            {
                string msg = string.Join(", ", errors);
                _logger.LogWarning($"Невалідні дані замовлення: {msg}");
                throw new ValidationException(msg);
            }

            order.Status = OrderStatus.New;
            order.CreatedAt = DateTime.Now;

            _orderRepository.Add(order);
            _logger.LogInfo($"Замовлення #{order.Id} збережено.");
            return order;
        }

        public void AssignVehicleToOrder(int orderId)
        {
            _logger.LogInfo($"Розподіл замовлення #{orderId}");

            var order = _orderRepository.GetById(orderId);
            if (order == null)
            {
                throw new EntityNotFoundException("Order", orderId);
            }

            if (order.Status != OrderStatus.New)
            {
                throw new BusinessLogicException($"Неможливо розподілити замовлення зі статусом {order.Status}.");
            }

            var suitableVehicle = _vehicleRepository.Find(v =>
                v.IsOperational &&
                v.LoadCapacityKg >= order.WeightKg
            ).FirstOrDefault();

            if (suitableVehicle == null)
            {
                _logger.LogWarning($"Дефіцит транспорту для ваги {order.WeightKg}");
                throw new BusinessLogicException("Не знайдено вільного автомобіля відповідної вантажопідйомності.");
            }

            var freeDriver = _driverRepository.Find(d => d.CurrentVehicleId == null).FirstOrDefault();

            if (freeDriver == null)
            {
                _logger.LogWarning("Дефіцит кадрів: немає вільних водіїв.");
                throw new BusinessLogicException("Немає вільних водіїв у штаті.");
            }

            order.AssignedVehicleId = suitableVehicle.Id;
            order.AssignedDriverId = freeDriver.Id;
            order.Status = OrderStatus.InProgress;

            freeDriver.CurrentVehicleId = suitableVehicle.Id;

            _orderRepository.Update(order);
            _driverRepository.Update(freeDriver);

            _logger.LogInfo($"Успішний розподіл: Order #{orderId} -> {suitableVehicle.LicensePlate} -> {freeDriver.LastName}");
        }
    }
}