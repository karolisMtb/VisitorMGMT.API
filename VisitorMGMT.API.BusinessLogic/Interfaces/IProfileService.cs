using System.Net;
using VisitorMGMT.API.DataAccess.Entities;
using VisitorMGMT.API.DataAccess.Models;

namespace VisitorMGMT.API.BusinessLogic.Interfaces
{
    public interface IProfileService
    {
        Task UpdateIdentityNumberAsync(Visitor visitor, int identityNumber);
        Task UpdateNameAsync(Visitor visitor, string name);
        Task UpdateAddressAsync(Visitor visitor, AddressDTO addressDTO);
        Task UpdatePhoneNumberAsync(Visitor currentVisitor, string phoneNumber);

    }
}
