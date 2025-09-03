using AutoMapper;
using CarRental.BLL.Abstractions;
using CarRental.BLL.Models;
using CarRental.DAL.Abstractions;
using CarRental.DAL.Models.Entities;

namespace CarRental.BLL.Services;

public class CarService(IGenericRepository<CarEntity> repo, IMapper mapper) : GenericService<CarModel, CarEntity>(repo, mapper), ICarService
{
}
