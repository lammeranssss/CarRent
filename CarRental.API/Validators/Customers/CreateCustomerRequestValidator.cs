using FluentValidation;
using CarRental.API.Constants;
using CarRental.API.Models.Requests.Customers;

namespace CarRental.API.Validators.Customers;

public class CreateCustomerRequestValidator : AbstractValidator<CreateCustomerRequest>
{
    public CreateCustomerRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .Length(ValidatorConstants.MinimumNameLength, ValidatorConstants.MaximumNameLength);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .Length(ValidatorConstants.MinimumNameLength, ValidatorConstants.MaximumNameLength);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Phone)
            .NotEmpty();

        RuleFor(x => x.Address)
            .NotEmpty();

        RuleFor(x => x.LicenseNumber)
            .NotEmpty();
    }
}
