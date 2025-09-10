using FluentValidation;
using CarRental.API.Constants;
using CarRental.API.Models.Requests.Customers;

namespace CarRental.API.Validators.Customers;

public class CreateCustomerRequestValidator : AbstractValidator<CreateCustomerRequest>
{
    public CreateCustomerRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage(ValidatorMessages.CannotBeEmpty("First Name"))
            .Length(ValidatorConstants.MinimumNameLength, ValidatorConstants.MaximumNameLength)
            .WithMessage(ValidatorMessages.Length("First Name", ValidatorConstants.MinimumNameLength, ValidatorConstants.MaximumNameLength));

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage(ValidatorMessages.CannotBeEmpty("Last Name"))
            .Length(ValidatorConstants.MinimumNameLength, ValidatorConstants.MaximumNameLength)
            .WithMessage(ValidatorMessages.Length("Last Name", ValidatorConstants.MinimumNameLength, ValidatorConstants.MaximumNameLength));

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(ValidatorMessages.CannotBeEmpty("Email"))
            .EmailAddress().WithMessage(ValidatorMessages.MustBeValid("Email"));

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage(ValidatorMessages.CannotBeEmpty("Phone"));

        RuleFor(x => x.Adress)
            .NotEmpty().WithMessage(ValidatorMessages.CannotBeEmpty("Address"));

        RuleFor(x => x.LicenseNumber)
            .NotEmpty().WithMessage(ValidatorMessages.CannotBeEmpty("License Number"));
    }
}
