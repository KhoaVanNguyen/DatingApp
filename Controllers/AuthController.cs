using System.Threading.Tasks;
using DatingApp.API.Data;
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
        public async Task<ActionResult> Register(string username, string password) {
             /// validate user
             username = username.ToLower();

             if ( await _repo.UserExists(username) ) {
                 return BadRequest("This user is exist");
             }

             var newUser = new User {
                 Username = username
             };

             var createdUser = await _repo.Register(newUser,password);
             return StatusCode(201);
            
        }
    }
}