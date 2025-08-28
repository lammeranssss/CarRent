namespace CarRental.ntier.API.Models.Requests.Cars;
public record CreateCarRequest(
    string Brand,
    string Model,
    int Year,
    string LicensePlate,
    string Color,
    Guid LocationId,
    decimal DailyRate
);
