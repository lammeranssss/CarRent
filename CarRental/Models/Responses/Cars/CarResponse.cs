namespace CarRental.ntier.API.Models.Responses.Cars
{
    public record CarResponse(
        Guid Id,
        DateTime CreatedAt,
        DateTime UpdatedAt,
        string Brand,
        string Model,
        int Year,
        string LicensePlate,
        string Color,
        Guid LocationId,
        string CarStatus,
        decimal DailyRate,
        decimal Mileage,
        string LocationName,
        bool IsAvailable,
        bool RequiresMaintenance
    );
}
