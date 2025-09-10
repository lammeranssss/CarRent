using FluentValidation;
using CarRental.API.Constants;
using CarRental.API.Models.Requests.Cars;

namespace CarRental.API.Validators.Cars;

public class CreateCarRequestValidator : AbstractValidator<CreateCarRequest>
{
    public CreateCarRequestValidator()
    {
        RuleFor(x => x.Brand)
            .NotEmpty().WithMessage(ValidatorMessages.CannotBeEmpty("Brand"))
            .Length(ValidatorConstants.MinimumNameLength, ValidatorConstants.MaximumNameLength)
            .WithMessage(ValidatorMessages.Length("Brand", ValidatorConstants.MinimumNameLength, ValidatorConstants.MaximumNameLength));

        RuleFor(x => x.Model)
            .NotEmpty().WithMessage(ValidatorMessages.CannotBeEmpty("Model"))
            .Length(ValidatorConstants.MinimumNameLength, ValidatorConstants.MaximumNameLength)
            .WithMessage(ValidatorMessages.Length("Model", ValidatorConstants.MinimumNameLength, ValidatorConstants.MaximumNameLength));

        RuleFor(x => x.Year)
            .InclusiveBetween(ValidatorConstants.MinimumYear, ValidatorConstants.MaximumYear)
            .WithMessage(ValidatorMessages.YearRange("Year", ValidatorConstants.MinimumYear, ValidatorConstants.MaximumYear));

        RuleFor(x => x.LicensePlate)
            .NotEmpty().WithMessage(ValidatorMessages.CannotBeEmpty("License Plate"));

        RuleFor(x => x.Color)
            .NotEmpty().WithMessage(ValidatorMessages.CannotBeEmpty("Color"));

        RuleFor(x => x.DailyRate)
            .GreaterThanOrEqualTo(ValidatorConstants.MinimumDailyRate)
            .WithMessage(ValidatorMessages.PositiveValue("Daily Rate"));

        RuleFor(x => x.LocationId)
            .NotEqual(Guid.Empty).WithMessage(ValidatorMessages.Required("LocationId"));
    }
}
