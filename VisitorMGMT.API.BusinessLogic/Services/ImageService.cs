using VisitorMGMT.API.BusinessLogic.Interfaces;
using VisitorMGMT.API.DataAccess.Interfaces;
using VisitorMGMT.API.DataAccess.Entities;
using VisitorMGMT.API.DataAccess.Models;

namespace VisitorMGMT.API.BusinessLogic.Services
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository _imageRepository;
        public ImageService(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        public async Task UploadProfileImageAsync(ProfileImage profileImage, Visitor newVisitor)
        {
            await _imageRepository.AddAsync(profileImage, newVisitor);

        }

        public async Task<ProfileImage> GenerateProfileImageAsync(ProfileImageDTO profileImageDTO)
        {
            using var memoryStream = new MemoryStream();
            await profileImageDTO.ProfileImage.CopyToAsync(memoryStream);

            ProfileImage uploadedImage = new ProfileImage()
            {
                Name = profileImageDTO.ProfileImage.FileName,
                ImageData = memoryStream.ToArray(),
                ContentType = profileImageDTO.ProfileImage.ContentType
            };

            return uploadedImage;
        }
    }
}
