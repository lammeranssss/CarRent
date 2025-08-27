namespace CarRental.ntier.API.Models.Requests.Customers
{
    public record CreateCustomerRequest (
        string FirstName,
        string LastName,
        string Email,
        string Phone,
        string Adress,
        string LicenseNumber
    );
}
