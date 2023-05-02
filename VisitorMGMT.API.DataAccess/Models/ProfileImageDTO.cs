using Microsoft.AspNetCore.Http;
using VisitorMGMT.API.DataAccess.Attributes;

namespace VisitorMGMT.API.DataAccess.Models
{
    public class ProfileImageDTO
    {
        [MaxFileSize(5 * 1024 * 1024)]
        [AllowedExtensions(new string[] { ".png", ".jpg", ".jpeg" })]
        public IFormFile ProfileImage { get; set; }
    }
}
