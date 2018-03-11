using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(10, MinimumLength = 4, ErrorMessage = "Bạn phải nhập mật khẩu độ dài từ 4-10 ký tự")]
        public string Password { get; set; }
    }
}