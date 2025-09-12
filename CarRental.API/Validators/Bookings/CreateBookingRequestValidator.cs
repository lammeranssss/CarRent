using FluentValidation;
using CarRental.API.Models.Requests.Bookings;

namespace CarRental.API.Validators.Bookings;

public class CreateBookingRequestValidator : AbstractValidator<CreateBookingRequest>
{
    public CreateBookingRequestValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty();

        RuleFor(x => x.CarId)
            .NotEmpty();

        RuleFor(x => x.StartDate)
            .LessThan(x => x.EndDate);
    }
}
