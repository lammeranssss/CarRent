using FluentValidation;
using CarRental.API.Constants;
using CarRental.API.Models.Requests.Cars;

namespace CarRental.API.Validators.Cars;

public class CreateCarRequestValidator : AbstractValidator<CreateCarRequest>
{
    public CreateCarRequestValidator()
    {
        RuleFor(x => x.Brand)
            .NotEmpty()
            .Length(ValidatorConstants.MinimumNameLength, ValidatorConstants.MaximumNameLength);

        RuleFor(x => x.Model)
            .NotEmpty()
            .Length(ValidatorConstants.MinimumNameLength, ValidatorConstants.MaximumNameLength);

        RuleFor(x => x.Year)
            .InclusiveBetween(ValidatorConstants.MinimumYear, ValidatorConstants.MaximumYear);

        RuleFor(x => x.LicensePlate)
            .NotEmpty();

        RuleFor(x => x.Color)
            .NotEmpty();

        RuleFor(x => x.DailyRate)
            .GreaterThanOrEqualTo(ValidatorConstants.MinimumDailyRate);

        RuleFor(x => x.LocationId)
            .NotEqual(Guid.Empty).WithMessage(ValidatorMessages.Required("LocationId"));
    }
}
