using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VisitorMGMT.API.DataAccess.Entities
{
    public class ProfileImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public byte[] ImageData { get; set; }
        public string ContentType { get; set; }
        public Guid ProfileId { get; set; }
        public Profile Profile { get; set; }
    }
}
