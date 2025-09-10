using FluentValidation;
using CarRental.API.Constants;
using CarRental.API.Models.Requests.Rentals;

namespace CarRental.API.Validators.Rentals;

public class CreateRentalRequestValidator : AbstractValidator<CreateRentalRequest>
{
    public CreateRentalRequestValidator()
    {
        RuleFor(x => x.BookingId)
            .NotEqual(Guid.Empty).WithMessage(ValidatorMessages.Required("BookingId"));

        RuleFor(x => x.PickUpDate)
            .LessThan(x => x.DropOffDate)
            .WithMessage("PickUpDate must be before DropOffDate");

        RuleFor(x => x.PickUpLocationId)
            .NotEqual(Guid.Empty).WithMessage(ValidatorMessages.Required("PickUpLocationId"));

        RuleFor(x => x.DropOffLocationId)
            .NotEqual(Guid.Empty).WithMessage(ValidatorMessages.Required("DropOffLocationId"));

        RuleFor(x => x.InitialMileage)
            .GreaterThanOrEqualTo(ValidatorConstants.MinimumMileage)
            .WithMessage(ValidatorMessages.PositiveValue("Initial Mileage"));
    }
}
