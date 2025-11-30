using FleetMaster.Core.Entities;
using FleetMaster.Core.Enums;
using FleetMaster.Core.Interfaces;

namespace FleetMaster.Infrastructure.Data
{
    public class DataSeeder
    {
        private readonly IRepository<Driver> _driverRepo;
        private readonly IRepository<Vehicle> _vehicleRepo;
        private readonly IRepository<Order> _orderRepo;

        public DataSeeder(IRepository<Driver> driverRepo, IRepository<Vehicle> vehicleRepo, IRepository<Order> orderRepo)
        {
            _driverRepo = driverRepo;
            _vehicleRepo = vehicleRepo;
            _orderRepo = orderRepo;
        }

        public void SeedAll()
        {
            if (!_driverRepo.GetAll().Any())
            {
                SeedDrivers();
            }

            if (!_vehicleRepo.GetAll().Any())
            {
                SeedVehicles();
            }
        }

        private void SeedDrivers()
        {
            var firstNames = new[]
            {
                "Oleksandr", "Dmytro", "Maksym", "Ivan", "Andrii", "Serhii", "Vitalii", "Yurii", "Artem", "Vladyslav",
                "Bohdan", "Volodymyr", "Denys", "Pavlo", "Mykola", "Roman", "Taras", "Ihor", "Oleh", "Nazar",
                "Anatolii", "Ruslan", "Yevhen", "Vadym", "Kostiantyn", "Viktor", "Stanislav", "Petro", "Mykhailo", "Yaroslav"
            };

            var lastNames = new[]
            {
                "Shevchenko", "Bondarenko", "Kovalenko", "Boiko", "Tkachenko", "Kravchenko", "Koval", "Oliinyk", "Shevchuk", "Polishchuk",
                "Lysenko", "Melnyk", "Moroz", "Havryliuk", "Rudenko", "Savchenko", "Ponomarenko", "Vasylenko", "Karpenko", "Symonenko",
                "Hryhorenko", "Pavlenko", "Muzyka", "Khomenko", "Usenko", "Lytvyn", "Kozlov", "Petrenko", "Didenko", "Shvets"
            };

            var random = new Random();

            for (int i = 0; i < 50; i++)
            {
                var driver = new Driver
                {
                    FirstName = firstNames[random.Next(firstNames.Length)],
                    LastName = lastNames[random.Next(lastNames.Length)],
                    DateOfBirth = DateTime.Now.AddYears(-random.Next(20, 60)),
                    ExperienceYears = random.Next(1, 40),
                    LicenseNumber = $"B{random.Next(100000, 999999)}UA",
                    CreatedAt = DateTime.Now
                };
                _driverRepo.Add(driver);
            }
        }

        private void SeedVehicles()
        {
            var brands = new[] { "Mercedes-Benz", "Volvo", "MAN", "Scania", "DAF", "Renault", "Iveco", "Ford" };
            var models = new[] { "Actros", "FH16", "TGX", "R-Series", "XF", "T-Range", "Stralis", "F-Max" };

            var random = new Random();

            for (int i = 0; i < 30; i++)
            {
                if (random.NextDouble() > 0.3)
                {
                    var truck = new Truck
                    {
                        Brand = brands[random.Next(brands.Length)],
                        Model = models[random.Next(models.Length)],
                        LicensePlate = $"AA{random.Next(1000, 9999)}KK",
                        YearOfManufacture = random.Next(2010, 2024),
                        LoadCapacityKg = random.Next(5000, 22000),
                        FuelConsumptionPer100Km = random.Next(25, 40),
                        Type = VehicleType.Truck,
                        HasTrailer = random.Next(0, 2) == 1,
                        CargoVolume = random.Next(60, 120),
                        CreatedAt = DateTime.Now
                    };
                    _vehicleRepo.Add(truck);
                }
                else
                {
                    var van = new CargoVan
                    {
                        Brand = "Volkswagen",
                        Model = "Crafter",
                        LicensePlate = $"KA{random.Next(1000, 9999)}BB",
                        YearOfManufacture = random.Next(2015, 2024),
                        LoadCapacityKg = random.Next(1000, 3500),
                        FuelConsumptionPer100Km = random.Next(10, 15),
                        Type = VehicleType.Van,
                        IsRefrigerated = random.Next(0, 2) == 1,
                        CreatedAt = DateTime.Now
                    };
                    _vehicleRepo.Add(van);
                }
            }
        }
    }
}