using VisitorMGMT.API.DataAccess.Entities;

namespace VisitorMGMT.API.DataAccess.Interfaces
{
    public interface IImageRepository
    {
        Task DeleteAsync(Guid profileImageId);
        Task AddAsync(ProfileImage profileImage, Visitor newVisitor);
    }
}
