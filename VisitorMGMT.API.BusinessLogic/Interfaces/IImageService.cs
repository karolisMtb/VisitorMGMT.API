using VisitorMGMT.API.DataAccess.Entities;
using VisitorMGMT.API.DataAccess.Models;

namespace VisitorMGMT.API.BusinessLogic.Interfaces
{
    public interface IImageService
    {
        Task UploadProfileImageAsync(ProfileImage profileImage, Visitor newVisitor);
        Task<ProfileImage> GenerateProfileImageAsync(ProfileImageDTO profileImageDTO);
    }
}
