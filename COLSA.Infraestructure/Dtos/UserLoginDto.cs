using System.ComponentModel.DataAnnotations;

namespace COLSA.Infraestructure.Dtos
{
    public class UserLoginDto
    {
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
