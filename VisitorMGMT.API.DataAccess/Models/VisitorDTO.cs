namespace VisitorMGMT.API.DataAccess.Models
{
    public class VisitorDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public ProfileImageDTO ProfileImage { get; set; }
        public ProfileDTO SignUpProfile { get; set; }
    }
}
