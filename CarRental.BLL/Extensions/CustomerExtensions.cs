using CarRental.BLL.Models;

namespace CarRental.BLL.Extensions;
public static class CustomerExtensions
{
    public static string GetFullName(this CustomerModel customer) =>
        $"{customer.FirstName} {customer.LastName}";

    public static bool HasValidLicense(this CustomerModel customer) =>
        !string.IsNullOrEmpty(customer.LicenseNumber) && customer.LicenseNumber.Length >= 8;
}
