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
        CreateMap<CarModel, CarEntity>()
                    .ForMember(dest => dest.Location, opt => opt.Ignore());

        CreateMap<CustomerModel, CustomerEntity>()
            .ForMember(dest => dest.Bookings, opt => opt.Ignore());

        CreateMap<LocationModel, LocationEntity>()
            .ForMember(dest => dest.Cars, opt => opt.Ignore());

        CreateMap<RentalModel, RentalEntity>()
            .ForMember(dest => dest.Booking, opt => opt.Ignore())
            .ForMember(dest => dest.PickUpLocation, opt => opt.Ignore())
            .ForMember(dest => dest.DropOffLocation, opt => opt.Ignore());

        CreateMap<BookingModel, BookingEntity>()
            .ForMember(dest => dest.Car, opt => opt.Ignore())
            .ForMember(dest => dest.Customer, opt => opt.Ignore())
            .ForMember(dest => dest.Rental, opt => opt.Ignore());
    }
}
