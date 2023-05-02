using FluentValidation;
using Microsoft.EntityFrameworkCore;
using VisitorMGMT.API.DataAccess.DatabaseContext;
using VisitorMGMT.API.DataAccess.Entities;
using VisitorMGMT.API.DataAccess.Interfaces;
using VisitorMGMT.API.DataAccess.Models;
using VisitorMGMT.API.DataAccess.Validations;

namespace VisitorMGMT.API.DataAccess.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly DatabaseMGMTContext _databaseContext;
        private readonly IValidator<Address> _addressValidator;
        private readonly IValidator<Profile> _profileValidator;
        public ProfileRepository(DatabaseMGMTContext databaseContext, IValidator<Address> addressValidator, IValidator<Profile> profileValidator)
        {
            _databaseContext = databaseContext;
            _addressValidator = addressValidator;
            _profileValidator = profileValidator;
        }

        public async Task UpdateNameAsync(Profile profileToUpdate, string newName)
        {
            if (newName == null)
            {
                throw new ArgumentException("Name cannot be empty");
            }

            profileToUpdate.FirstName = newName;
            await RunFluentValidation(profileToUpdate);
            await SaveChangesAsync();
        }

        public async Task UpdateIdentityNumberAsync(Profile profileToUpdate, int identityNumber)
        {
            if(identityNumber == 0)
            {
                throw new ArgumentException("Phone number cannot be empty");
            }
            profileToUpdate.IdentityNumber = identityNumber;
            await RunFluentValidation(profileToUpdate);
            await SaveChangesAsync();
        }

        public async Task UpdateAddressAsync(Profile profileToUpdate, AddressDTO addressDTO, bool addressIsSet)
        {
            if (addressIsSet == false)
            {
                profileToUpdate.Address = new Address()
                {
                    StreetName = addressDTO.StreetName,
                    HouseNumber = addressDTO.HouseNumber,
                    City = addressDTO.City,
                    PostalCode = addressDTO.PostalCode
                };
            }

            profileToUpdate.Address.StreetName = addressDTO.StreetName;
            profileToUpdate.Address.HouseNumber = addressDTO.HouseNumber;
            profileToUpdate.Address.City = addressDTO.City;
            profileToUpdate.Address.PostalCode = addressDTO.PostalCode;

            await RunFluentValidation(profileToUpdate.Address);
            await SaveChangesAsync();
        }

        public async Task UpdatePhoneNumberAsync(Profile profileToUpdate, string phoneNumber)
        {
            profileToUpdate.PhoneNumber = phoneNumber;
            await RunFluentValidation(profileToUpdate);
            await SaveChangesAsync();
        }

        private async Task SaveChangesAsync()
        {
            await _databaseContext.SaveChangesAsync();
        }

        public async Task<Profile> GetProfileByUserName(string username)
        {
            var profile = await _databaseContext.Profiles.Include(x => x.Address).FirstOrDefaultAsync(x => x.Visitor.UserName == username);
            if (profile == null)
            {
                throw new InvalidOperationException("Visitor does not have his profile set.");
            }

            return profile;
        }

        public async Task<bool> CheckIfAddressExists(Visitor currentVisitor)
        {
            return await _databaseContext.Addresss.Where(x => x.ProfileId == currentVisitor.Profile.Id).AnyAsync();
        }

        private async Task RunFluentValidation(Address address)
        {
            var result = await _addressValidator.ValidateAsync(address);
            if (!result.IsValid)
            {
                FluentValidation.Results.ValidationResult validationResult = result;
                throw new ValidationException(result.Errors[0].ErrorMessage);
            }
        }

        private async Task RunFluentValidation(Profile profile)
        {
            var result = await _profileValidator.ValidateAsync(profile);
            if (!result.IsValid)
            {
                FluentValidation.Results.ValidationResult validationResult = result;
                throw new ValidationException(result.Errors[0].ErrorMessage);
            }
        }
    }
}
