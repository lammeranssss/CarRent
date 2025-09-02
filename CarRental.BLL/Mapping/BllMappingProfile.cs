using AutoMapper;
using CarRental.BLL.Models;
using CarRental.DAL.Models.Entities;

namespace CarRental.BLL.Mapping;

public class BllMappingProfile : Profile
{
    public BllMappingProfile()
    {
        CreateMap<CarEntity, CarModel>().ReverseMap();
        CreateMap<CustomerEntity, CustomerModel>().ReverseMap();
        CreateMap<LocationEntity, LocationModel>().ReverseMap();
        CreateMap<RentalEntity, RentalModel>().ReverseMap();
        CreateMap<BookingEntity, BookingModel>().ReverseMap();
    }
}
