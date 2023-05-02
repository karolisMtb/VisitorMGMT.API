using System.Net;
using VisitorMGMT.API.DataAccess.Entities;
using VisitorMGMT.API.DataAccess.Models;

namespace VisitorMGMT.API.DataAccess.Interfaces
{
    public interface IProfileRepository
    {
        Task<Profile> GetProfileByUserName(string username);
        Task UpdateNameAsync(Profile profileToUpdate, string newName);
        Task UpdateIdentityNumberAsync(Profile profileToUpdate, int identityNumber);
        Task UpdateAddressAsync(Profile profileToUpdate, AddressDTO addressDTO, bool addressIsSet);
        Task UpdatePhoneNumberAsync(Profile profileToUpdate, string phoneNumber);
        Task<bool> CheckIfAddressExists(Visitor currentVisitor);
    }
}
