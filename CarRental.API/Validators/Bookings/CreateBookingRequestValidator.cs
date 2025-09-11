using FluentValidation;
using CarRental.API.Constants;
using CarRental.API.Models.Requests.Bookings;

namespace CarRental.API.Validators.Bookings;

public class CreateBookingRequestValidator : AbstractValidator<CreateBookingRequest>
{
    public CreateBookingRequestValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEqual(Guid.Empty).WithMessage(ValidatorMessages.Required("CustomerId"));

        RuleFor(x => x.CarId)
            .NotEqual(Guid.Empty).WithMessage(ValidatorMessages.Required("CarId"));

        RuleFor(x => x.StartDate)
            .LessThan(x => x.EndDate);
    }
}
