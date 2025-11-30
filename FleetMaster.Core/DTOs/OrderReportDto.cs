namespace FleetMaster.Core.DTOs
{
    public class OrderReportDto
    {
        public int OrderId { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string ClientDestination { get; set; }
        public string DriverName { get; set; }
        public string VehiclePlate { get; set; }
        public string OrderStatus { get; set; }
    }
}