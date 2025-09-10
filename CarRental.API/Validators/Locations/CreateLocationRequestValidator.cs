using FluentValidation;
using CarRental.API.Constants;
using CarRental.API.Models.Requests.Locations;

namespace CarRental.API.Validators.Locations;

public class CreateLocationRequestValidator : AbstractValidator<CreateLocationRequest>
{
    public CreateLocationRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(ValidatorMessages.CannotBeEmpty("Name"))
            .Length(ValidatorConstants.MinimumNameLength, ValidatorConstants.MaximumNameLength)
            .WithMessage(ValidatorMessages.Length("Name", ValidatorConstants.MinimumNameLength, ValidatorConstants.MaximumNameLength));

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage(ValidatorMessages.CannotBeEmpty("Address"));

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage(ValidatorMessages.CannotBeEmpty("Phone"));
    }
}
