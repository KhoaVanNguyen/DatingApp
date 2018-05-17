using System;
using AutoMapper;
using DatingApp.Api.Data;
using DatingApp.Api.Dtos;
using DatingApp.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.Api.Controllers {
    [Route ("api/[controller]")]
    public class AuthController : Controller {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        public AuthController (IAuthRepository repo, IConfiguration config, IMapper mapper) {
            _mapper = mapper;
            _config = config;
            _repo = repo;
        }

        [HttpPost ("register")]
        public async Task<IActionResult> Register ([FromBody] UserForRegisterDto userForRegisterDto) {
            if (!string.IsNullOrEmpty (userForRegisterDto.UserName))
                userForRegisterDto.UserName = userForRegisterDto.UserName.ToLower ();

            if (await _repo.UserExist (userForRegisterDto.UserName))
                ModelState.AddModelError ("UserName", "user name is already taken!");
            // return BadRequest ("user name is already taken!");

            if (!ModelState.IsValid)
                return BadRequest (ModelState);

            var userToCreate = new User {
                UserName = userForRegisterDto.UserName
            };

            var createUser = await _repo.Register (userToCreate, userForRegisterDto.Password);

            return StatusCode (201);
        }

        [HttpPost ("login")]

        public async Task<IActionResult> login ([FromBody] UserForLoginDto userForLoginDto) {

            // throw new Exception("error");

            var userFromRepo = await _repo.Login (userForLoginDto.UserName.ToLower (), userForLoginDto.Password);

            if (userFromRepo == null) {
                return Unauthorized ();
            }

            //generate token
            var tokenHandler = new JwtSecurityTokenHandler ();
            var key = Encoding.ASCII.GetBytes (_config.GetSection ("AppSettings:Token").Value);
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity (new Claim[] {
                new Claim (ClaimTypes.NameIdentifier, userFromRepo.Id.ToString ()),
                new Claim (ClaimTypes.Name, userFromRepo.UserName)
                }),
                Expires = DateTime.Now.AddDays (1),
                SigningCredentials = new SigningCredentials (new SymmetricSecurityKey (key),
                SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken (tokenDescriptor);
            var tokenString = tokenHandler.WriteToken (token);

            var user = _mapper.Map<UserForListDto> (userFromRepo);
            return Ok (new { tokenString, user });
        }
    }
}