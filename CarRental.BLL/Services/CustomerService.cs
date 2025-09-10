using AutoMapper;
using CarRental.BLL.Abstractions;
using CarRental.BLL.Models;
using CarRental.DAL.Abstractions;
using CarRental.DAL.Models.Entities;

namespace CarRental.BLL.Services;

public class CustomerService(ICustomerRepository repository, IMapper mapper) : GenericService<CustomerModel, CustomerEntity>(repository, mapper), ICustomerService
{
}
