using AutoMapper;
using CarRental.BLL.Abstractions;
using CarRental.BLL.Models;
using CarRental.DAL.Abstractions;
using CarRental.DAL.Models.Entities;

namespace CarRental.BLL.Services;

public class RentalService(IRentalRepository repository, IMapper mapper) : GenericService<RentalModel, RentalEntity>(repository, mapper), IRentalService
{
}
