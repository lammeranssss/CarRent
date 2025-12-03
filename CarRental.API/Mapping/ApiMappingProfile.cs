using AutoMapper;
using CarRental.API.Models.Requests.Bookings;
using CarRental.API.Models.Requests.Cars;
using CarRental.API.Models.Requests.Customers;
using CarRental.API.Models.Requests.Locations;
using CarRental.API.Models.Requests.Rentals;
using CarRental.API.Models.Responses.Bookings;
using CarRental.API.Models.Responses.Cars;
using CarRental.API.Models.Responses.Customers;
using CarRental.API.Models.Responses.Locations;
using CarRental.API.Models.Responses.Rentals;
using CarRental.BLL.Models;
using CarRental.DAL.Models.Enums;

namespace CarRental.API.Mapping;

public class ApiMappingProfile : Profile
{
    public ApiMappingProfile()
    {
        CreateMap<CreateCarRequest, CarModel>();
        CreateMap<CreateCustomerRequest, CustomerModel>();
        CreateMap<CreateBookingRequest, BookingModel>();
        CreateMap<CreateLocationRequest, LocationModel>();
        CreateMap<CreateRentalRequest, RentalModel>();
        CreateMap<UpdateRentalRequest, RentalModel>();

        CreateMap<CarModel, CarResponse>()
            .ForMember(dest => dest.CarStatus, opt => opt.MapFrom(src => src.CarStatus.ToString()))
            .ForMember(dest => dest.LocationName, opt => opt.MapFrom(src => src.Location.Name))
            .ForMember(dest => dest.IsAvailable, opt => opt.Ignore())
            .ForMember(dest => dest.RequiresMaintenance, opt => opt.Ignore());

        CreateMap<CustomerModel, CustomerResponse>()
            .ForMember(dest => dest.FullName, opt => opt.Ignore())
            .ForMember(dest => dest.HasValidLicense, opt => opt.Ignore())
            .ForMember(dest => dest.LoyaltyLevel, opt => opt.Ignore())
            .ForMember(dest => dest.CompletedBookingsCount, opt => opt.MapFrom(src => src.Bookings.Count));

        CreateMap<BookingModel, BookingResponse>()
            .ForMember(dest => dest.BookingStatus, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.CustomerName, opt => opt.Ignore())
            .ForMember(dest => dest.CarDetails, opt => opt.MapFrom(src => $"{src.Car.Brand} {src.Car.Model} {src.Car.LicensePlate}"))
            .ForMember(dest => dest.DurationInDays, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.Ignore())
            .ForMember(dest => dest.CanBeCancelled, opt => opt.Ignore());

        CreateMap<LocationModel, LocationResponse>()
            .ForMember(dest => dest.AvailableCarsCount, opt => opt.Ignore())
            .ForMember(dest => dest.TotalCarsCount, opt => opt.MapFrom(src => src.Cars.Count))
            .ForMember(dest => dest.CanAcceptReturns, opt => opt.Ignore());

        CreateMap<RentalModel, RentalResponse>()
            .ForMember(dest => dest.PickUpLocationName, opt => opt.MapFrom(src => src.PickUpLocation.Name))
            .ForMember(dest => dest.DropOffLocationName, opt => opt.MapFrom(src => src.DropOffLocation.Name))
            .ForMember(dest => dest.MileageUsed, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.Ignore())
            .ForMember(dest => dest.HasExceededMileageLimit, opt => opt.Ignore());
    }
}
