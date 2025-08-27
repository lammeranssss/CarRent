using CarRental.ntier.BLL.Models;
using CarRental.ntier.BLL.Abstractions;

namespace CarRental.ntier.BLL.Services
{
    public class CustomerService : ICustomerService
    {
        public string GetFullName(CustomerModel customer)
        {
            return $"{customer.FirstName} {customer.LastName}";
        }

        public bool HasValidLicense(CustomerModel customer)
        {
            return !string.IsNullOrEmpty(customer.LicenseNumber) &&
                   customer.LicenseNumber.Length >= 8;
        }
    }
}
