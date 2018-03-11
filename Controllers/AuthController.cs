using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthRepository _repo;

        public AuthController(IAuthRepository repo){
            this._repo = repo;
        }
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] UserForRegisterDto userForRegisterDto) {


            //
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

            if (await _repo.UserExists(userForRegisterDto.Username))
            {
                ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại");
                // return BadRequest("This user is exist");
            }
            /// validate user by model state
            if ( !ModelState.IsValid ) {
                return BadRequest(ModelState);
            }

            

             var newUser = new User {
                 Username = userForRegisterDto.Username
             };

             var createdUser = await _repo.Register(newUser,userForRegisterDto.Password);
             return StatusCode(201);
            
        }
    }
}