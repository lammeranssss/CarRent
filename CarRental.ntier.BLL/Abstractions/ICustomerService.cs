using CarRental.ntier.BLL.Models;

namespace CarRental.ntier.BLL.Abstractions
{
    public interface ICustomerService
    {
        string GetFullName(CustomerModel customer);
        bool HasValidLicense(CustomerModel customer);
    }
}
