using VisitorMGMT.API.DataAccess.Entities;
using VisitorMGMT.API.DataAccess.Models;

namespace VisitorMGMT.API.BusinessLogic.Interfaces
{
    public interface IVisitorService
    {
        Task<string> AuthenticateAsync(LoginDTO visitorLogin);
        Task<Visitor> SignUpNewAsync(VisitorDTO visitorDTO);
        Task DeleteAsync(Guid visitorId);
        Task<Visitor> GetVisitorByUserNameAsync(string username);
        Task<Visitor> GetVisitorByIdAsync(Guid id);
    }
}
