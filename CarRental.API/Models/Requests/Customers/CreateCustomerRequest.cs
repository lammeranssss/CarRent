namespace CarRental.API.Models.Requests.Customers;
public record CreateCustomerRequest(
    string FirstName,
    string LastName,
    string Email,
    string Phone,
    string Address,
    string LicenseNumber
);
