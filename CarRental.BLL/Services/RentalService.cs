using AutoMapper;
using CarRental.BLL.Abstractions;
using CarRental.BLL.Models;
using CarRental.DAL.Abstractions;
using CarRental.DAL.Models.Entities;

namespace CarRental.BLL.Services;

public class RentalService(IGenericRepository<RentalEntity> repo, IMapper mapper) : GenericService<RentalModel, RentalEntity>(repo, mapper), IRentalService
{
}
