namespace CarRental.API.Models.Responses.Customers;
public record CustomerResponse(
    Guid Id,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    string FirstName,
    string LastName,
    string Email,
    string Phone,
    string Address,
    string LicenseNumber,
    string FullName,
    bool HasValidLicense,
    int LoyaltyLevel,
    int CompletedBookingsCount
);
