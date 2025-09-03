using AutoMapper;
using CarRental.BLL.Abstractions;
using CarRental.BLL.Models;
using CarRental.DAL.Abstractions;
using CarRental.DAL.DataContext;
using CarRental.DAL.Models.Entities;

namespace CarRental.BLL.Services;

public class LocationService(ILocationRepository repository, IMapper mapper) : GenericService<LocationModel, LocationEntity>(repository, mapper), ILocationService
{
    private readonly ILocationRepository _repository = repository;
}
