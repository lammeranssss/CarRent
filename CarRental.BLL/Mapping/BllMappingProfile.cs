using AutoMapper;
using CarRental.BLL.Extensions;
using CarRental.BLL.Models;
using CarRental.DAL.Models.Entities;
using CarRental.Messaging.Events;

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

        CreateMap<CustomerModel, CustomerRegisteredEvent>()
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.Id));

        CreateMap<BookingEntity, BookingCreatedEvent>()
            .ForMember(dest => dest.BookingId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CustomerEmail, opt => opt.MapFrom((src, dest, destMember, context) =>
                (context.Items["Customer"] as CustomerEntity)?.Email))
            .ForMember(dest => dest.CustomerFirstName, opt => opt.MapFrom((src, dest, destMember, context) =>
                (context.Items["Customer"] as CustomerEntity)?.FirstName))
            .ForMember(dest => dest.CarModel, opt => opt.MapFrom((src, dest, destMember, context) =>
                (context.Items["Car"] as CarEntity) is not null ? $"{(context.Items["Car"] as CarEntity).Brand} {(context.Items["Car"] as CarEntity).Model}" : null));

        CreateMap<BookingEntity, BookingConfirmedEvent>()
            .ForMember(dest => dest.BookingId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CustomerEmail, opt => opt.MapFrom((src, dest, destMember, context) =>
                (context.Items["Customer"] as CustomerEntity)?.Email))
            .ForMember(dest => dest.CustomerFirstName, opt => opt.MapFrom((src, dest, destMember, context) =>
                (context.Items["Customer"] as CustomerEntity)?.FirstName))
            .ForMember(dest => dest.CarModel, opt => opt.MapFrom((src, dest, destMember, context) =>
                (context.Items["Car"] as CarEntity) is not null ? $"{(context.Items["Car"] as CarEntity).Brand} {(context.Items["Car"] as CarEntity).Model}" : null));

        CreateMap<BookingEntity, BookingCancelledEvent>()
            .ForMember(dest => dest.BookingId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CustomerEmail, opt => opt.MapFrom((src, dest, destMember, context) =>
                (context.Items["Customer"] as CustomerEntity)?.Email))
            .ForMember(dest => dest.CustomerFirstName, opt => opt.MapFrom((src, dest, destMember, context) =>
                (context.Items["Customer"] as CustomerEntity)?.FirstName))
            .ForMember(dest => dest.CarModel, opt => opt.MapFrom((src, dest, destMember, context) =>
                (context.Items["Car"] as CarEntity) is not null ? $"{(context.Items["Car"] as CarEntity).Brand} {(context.Items["Car"] as CarEntity).Model}" : null));

        CreateMap<RentalModel, RentalStartedEvent>()
            .ForMember(dest => dest.RentalId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CustomerEmail, opt => opt.MapFrom((src, dest, destMember, context) =>
                (context.Items["Customer"] as CustomerEntity)?.Email))
            .ForMember(dest => dest.CustomerFirstName, opt => opt.MapFrom((src, dest, destMember, context) =>
                (context.Items["Customer"] as CustomerEntity)?.FirstName))
            .ForMember(dest => dest.CarModel, opt => opt.MapFrom((src, dest, destMember, context) =>
                (context.Items["Car"] as CarEntity) is not null ? $"{(context.Items["Car"] as CarEntity).Brand} {(context.Items["Car"] as CarEntity).Model}" : null))
            .ForMember(dest => dest.CarLicensePlate, opt => opt.MapFrom((src, dest, destMember, context) =>
                (context.Items["Car"] as CarEntity)?.LicensePlate));

        CreateMap<RentalModel, RentalCompletedEvent>()
            .ForMember(dest => dest.RentalId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.KilometersDriven, opt => opt.MapFrom(src => src.CalculateMileageUsed()))
            .ForMember(dest => dest.CustomerEmail, opt => opt.MapFrom((src, dest, destMember, context) =>
                (context.Items["Customer"] as CustomerEntity)?.Email))
            .ForMember(dest => dest.CarModel, opt => opt.MapFrom((src, dest, destMember, context) =>
                (context.Items["Car"] as CarEntity) is not null ? $"{(context.Items["Car"] as CarEntity).Brand} {(context.Items["Car"] as CarEntity).Model}" : null));
    }
}
