using Microsoft.EntityFrameworkCore;
using VisitorMGMT.API.DataAccess.DatabaseContext;
using VisitorMGMT.API.DataAccess.Entities;
using VisitorMGMT.API.DataAccess.Interfaces;

namespace VisitorMGMT.API.DataAccess.Repositories
{
    public class VisitorRepository : IVisitorRepository
    {
        private readonly DatabaseMGMTContext _databaseContext;
        public VisitorRepository(DatabaseMGMTContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task AddAsync(Visitor visitor)
        {
            await _databaseContext.Visitors.AddAsync(visitor);
            await SaveChangesAsync();
        }

        public async Task<Visitor> GetVisitorByIdAsync(Guid visitorId)
        {
            var requiredVisitor = await _databaseContext.Visitors.Include(x => x.Profile).ThenInclude(x => x.Address).Include(x => x.Profile).ThenInclude(x => x.ProfileImage).FirstOrDefaultAsync(x => x.Id == visitorId);

            if(requiredVisitor == null)
            {
                throw new FileNotFoundException($"User with an id {visitorId} could not be found");
            }

            return requiredVisitor;
        }

        public async Task DeleteAsync(Guid visitorId)
        {
            Visitor visitor = await GetVisitorByIdAsync(visitorId);
            _databaseContext.Visitors.Remove(visitor);
            await SaveChangesAsync();
        }

        public async Task<Visitor> GetVisitorByUserNameAsync(string userName)
        {
            var visitor =  await _databaseContext.Visitors.Include(x => x.Profile).FirstOrDefaultAsync(x => x.UserName == userName);

            if (visitor == null)
            {
                throw new FileNotFoundException($"User was not found. Please enter valid username and password.");
            }

            return visitor;
        }

        public async Task<bool> CheckIfVisitorExistsAsync(string username)
        {
            return await _databaseContext.Visitors.AnyAsync(x => x.UserName == username);
        }

        private async Task SaveChangesAsync()
        {
            await _databaseContext.SaveChangesAsync();
        }
    }
}
