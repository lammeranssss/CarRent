using FluentValidation;
using CarRental.API.Constants;
using CarRental.API.Models.Requests.Locations;

namespace CarRental.API.Validators.Locations;

public class CreateLocationRequestValidator : AbstractValidator<CreateLocationRequest>
{
    public CreateLocationRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(ValidatorConstants.MinimumNameLength, ValidatorConstants.MaximumNameLength);

        RuleFor(x => x.Address)
            .NotEmpty();

        RuleFor(x => x.Phone)
            .NotEmpty();
    }
}
