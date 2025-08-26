namespace CarRental.ntier.API.Models.Requests.Cars
{
    public class UpdateCarRequest
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string LicensePlate { get; set; }
        public string Color { get; set; }
        public Guid LocationId { get; set; }
        public decimal DailyRate { get; set; }
    }
}
