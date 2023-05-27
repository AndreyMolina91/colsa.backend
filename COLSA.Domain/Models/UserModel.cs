using System.ComponentModel.DataAnnotations;

namespace COLSA.Domain.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string EmailAddress { get; set; }
        [Required]
        public string UserCountry { get; set; }
        [Required]
        public string UserDateOfBirth { get; set; }
        [Required]
        public string InGameName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

    }
}