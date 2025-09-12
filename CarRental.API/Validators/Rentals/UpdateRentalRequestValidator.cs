using FluentValidation;
using CarRental.API.Constants;
using CarRental.API.Models.Requests.Rentals;

namespace CarRental.API.Validators.Rentals;

public class UpdateRentalRequestValidator : AbstractValidator<UpdateRentalRequest>
{
    public UpdateRentalRequestValidator()
    {
        RuleFor(x => x.PickUpDate)
            .LessThan(x => x.DropOffDate);

        RuleFor(x => x.PickUpLocationId)
            .NotEmpty();

        RuleFor(x => x.DropOffLocationId)
            .NotEmpty();

        RuleFor(x => x.FinalMileage)
            .GreaterThanOrEqualTo(ValidatorConstants.MinimumMileage);
    }
}
