using FluentValidation;
using VisitorMGMT.API.DataAccess.Entities;
using VisitorMGMT.API.DataAccess.Models;

namespace VisitorMGMT.API.DataAccess.Validators
{
    public class AddressValidator : AbstractValidator<Address>
    {
        public AddressValidator()
        {
            RuleFor(x => x.StreetName).NotEmpty().WithMessage("Street name cannot be empty");
            RuleFor(x => x.HouseNumber).NotEmpty().WithMessage("House number cannot be empty");
            RuleFor(x => x.City).NotEmpty().WithMessage("City name cannot be empty");
            RuleFor(x => x.PostalCode).NotEmpty().WithMessage("Postal code cannot be empty or start with zero");
        }
    }
}
