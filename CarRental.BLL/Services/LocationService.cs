using AutoMapper;
using CarRental.BLL.Abstractions;
using CarRental.BLL.Models;
using CarRental.DAL.Abstractions;
using CarRental.DAL.Models.Entities;

namespace CarRental.BLL.Services;

public class LocationService(IGenericRepository<LocationEntity> repo, IMapper mapper) : GenericService<LocationModel, LocationEntity>(repo, mapper), ILocationService
{
}
