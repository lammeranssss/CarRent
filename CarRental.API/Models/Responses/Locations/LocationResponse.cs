namespace CarRental.API.Models.Responses.Locations;
public record LocationResponse(
    Guid Id,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    string Name,
    string Address,
    string Phone,
    int AvailableCarsCount,
    int TotalCarsCount,
    bool CanAcceptReturns
);
