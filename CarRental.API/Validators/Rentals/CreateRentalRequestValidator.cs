using FluentValidation;
using CarRental.API.Constants;
using CarRental.API.Models.Requests.Rentals;

namespace CarRental.API.Validators.Rentals;

public class CreateRentalRequestValidator : AbstractValidator<CreateRentalRequest>
{
    public CreateRentalRequestValidator()
    {
        RuleFor(x => x.BookingId)
            .NotEmpty();

        RuleFor(x => x.PickUpDate)
            .LessThan(x => x.DropOffDate);

        RuleFor(x => x.PickUpLocationId)
            .NotEmpty();

        RuleFor(x => x.DropOffLocationId)
            .NotEmpty();

        RuleFor(x => x.InitialMileage)
            .GreaterThanOrEqualTo(ValidatorConstants.MinimumMileage);
    }
}
