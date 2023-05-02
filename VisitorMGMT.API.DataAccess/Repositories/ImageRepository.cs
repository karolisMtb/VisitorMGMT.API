using VisitorMGMT.API.DataAccess.Interfaces;
using VisitorMGMT.API.DataAccess.DatabaseContext;
using VisitorMGMT.API.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace VisitorMGMT.API.DataAccess.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly DatabaseMGMTContext _databaseContext;
        public ImageRepository(DatabaseMGMTContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task AddAsync(ProfileImage profileImage, Visitor newVisitor)
        {
            if(profileImage == null)
            {
                throw new ArgumentNullException(nameof(profileImage));
            }

            profileImage.ProfileId = newVisitor.Profile.Id;
            await _databaseContext.ProfileImages.AddAsync(profileImage);
            await SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid profileImageId)
        {
            var profileImage = await _databaseContext.ProfileImages.FirstAsync(x => x.Id == profileImageId);

            if(profileImage == null)
            {
                throw new FileNotFoundException($"Image with an id {profileImageId} was not found.");
            }

            _databaseContext.ProfileImages.Remove(profileImage);
            await SaveChangesAsync();
        }

        private async Task SaveChangesAsync()
        {
            await _databaseContext.SaveChangesAsync();
        }

    }
}
