namespace CarRental.API.Models.Responses.Cars;

public class CarResponse
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public string LicensePlate { get; set; }
    public string Color { get; set; }
    public Guid LocationId { get; set; }
    public string CarStatus { get; set; }
    public decimal DailyRate { get; set; }
    public decimal Mileage { get; set; }
    public string LocationName { get; set; }
    public bool IsAvailable { get; set; }
    public bool RequiresMaintenance { get; set; }
}
