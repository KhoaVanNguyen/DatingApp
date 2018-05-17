using System.ComponentModel.DataAnnotations;

namespace DatingApp.Api.Dtos {
    public class UserForRegisterDto {
        [Required]
        public string UserName { get; set; }

        [Required]
        [StringLength (8, MinimumLength = 4,ErrorMessage="you must specify a password between 4 and 8 characters!")]
        public string Password { get; set; }
    }
}