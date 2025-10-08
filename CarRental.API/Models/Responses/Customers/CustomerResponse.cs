namespace CarRental.API.Models.Responses.Customers;

public class CustomerResponse
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string LicenseNumber { get; set; }
    public string FullName { get; set; }
    public bool HasValidLicense { get; set; }
    public int LoyaltyLevel { get; set; }
    public int CompletedBookingsCount { get; set; }
}
