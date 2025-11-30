using FleetMaster.Core.DTOs;
using FleetMaster.Core.Entities;

namespace FleetMaster.BusinessLogic.Mappers
{
    public static class VehicleMapper
    {
        public static VehicleDto ToDto(Vehicle vehicle)
        {
            return new VehicleDto
            {
                FullName = $"{vehicle.Brand} {vehicle.Model}",
                LicensePlate = vehicle.LicensePlate,
                Year = vehicle.YearOfManufacture,
                Type = vehicle.Type.ToString(),
                Status = vehicle.IsOperational ? "Ready" : "In Service"
            };
        }
    }
}