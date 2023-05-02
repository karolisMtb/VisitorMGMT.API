using VisitorMGMT.API.DataAccess.Entities;

namespace VisitorMGMT.API.DataAccess.Interfaces
{
    public interface IVisitorRepository
    {
        Task AddAsync(Visitor visitor);
        Task<Visitor> GetVisitorByUserNameAsync(string userName);
        Task<Visitor> GetVisitorByIdAsync(Guid visitorId);
        Task DeleteAsync(Guid visitorId);
        Task<bool> CheckIfVisitorExistsAsync(string username);
    }
}
