using AutoMapper;
using CarRental.API.Models.Requests.Bookings;
using CarRental.API.Models.Requests.Cars;
using CarRental.API.Models.Requests.Customers;
using CarRental.API.Models.Requests.Locations;
using CarRental.API.Models.Requests.Rentals;
using CarRental.BLL.Models;

namespace CarRental.API.Mapping;
public class ApiToBllProfile : Profile
{
    public ApiToBllProfile()
    {
        CreateMap<CreateCarRequest, CarModel>();
        CreateMap<CreateCustomerRequest, CustomerModel>();
        CreateMap<CreateBookingRequest, BookingModel>();
        CreateMap<CreateLocationRequest, LocationModel>();
        CreateMap<CreateRentalRequest, RentalModel>();
        CreateMap<UpdateRentalRequest, RentalModel>();
    }
}
