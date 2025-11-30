using FleetMaster.Core.DTOs;
using FleetMaster.Core.Entities;

namespace FleetMaster.BusinessLogic.Mappers
{
    public static class OrderMapper
    {
        public static OrderReportDto ToReportDto(Order order, Vehicle vehicle, Driver driver)
        {
            return new OrderReportDto
            {
                OrderId = order.Id,
                Description = order.Description,
                ClientDestination = order.Destination,
                Price = order.Price,
                OrderStatus = order.Status.ToString(),

                VehiclePlate = vehicle != null ? vehicle.LicensePlate : "---",
                DriverName = driver != null ? driver.FullName : "---"
            };
        }
    }
}