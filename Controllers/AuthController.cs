using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using LCM.Domain.Entities;
using LCM.Models;
using LCM.Persistance;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LCM.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly LCMSettings _appSettings;
        private readonly UserManager<LCMUser> _userManager;

        public AuthController(UserManager<LCMUser> userManager,
            IOptions<LCMSettings> appSettings)
        {
            _userManager = userManager;
            _appSettings = appSettings.Value;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IdentityResult> Register(User input)
        {
            var _user = new LCMUser
            {
                UserName = input.UserName,
                Email = input.Email,
                FullName = input.FullName
            };

            var result = await _userManager.CreateAsync(_user, input.Password);

            return result;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LogIn input)
        {
            var user = await _userManager.FindByNameAsync(input.UserName);
            if (user == null || !await _userManager.CheckPasswordAsync(user, input.Password))
                return BadRequest(new {message = "Username or password is incorrect."});

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("UserID", user.Id)
                }),
                Expires = DateTime.UtcNow.AddHours(.5),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Key)),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);

            return Ok(new {token});
        }
    }
}