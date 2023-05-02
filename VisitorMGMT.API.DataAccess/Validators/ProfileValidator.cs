using FluentValidation;
using VisitorMGMT.API.DataAccess.Entities;

namespace VisitorMGMT.API.DataAccess.Validations
{
    public class ProfileValidator : AbstractValidator<Profile>
    {
        public ProfileValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name cannot be empty");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name cannot be empty");
            RuleFor(x => x.IdentityNumber).NotEmpty().WithMessage("You must enter valid identity number");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Phone number cannot be empty");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Please enter valid email address");
        }
    }
}
