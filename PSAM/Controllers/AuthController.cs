using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json;
using PSAM.Models;
using PSAM.Services.IServices;
using System.Security.Claims;
using System.Text;

namespace PSAM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public readonly IAccountService _accountService;

        public AuthController(IAccountService account)
        {
                _accountService = account;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            bool isValidUser = await _accountService.CheckAccount(loginModel.Login, loginModel.Password);

            if (!isValidUser) { return Unauthorized(); }

            var userId = await _accountService.GetId(loginModel.Login, loginModel.Password);
            var username = await _accountService.GetUsername(userId);

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Name, username),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("9xHWS9p07XIkL/J4kI2XV6FeGrXramDG0JVvSwmKF50="));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
               issuer: @"https://localhost:4200/",
               audience: @"https://localhost:4200/",
               claims: claims,
               expires: DateTime.Now.AddHours(24), // Token ważny przez 24h
               signingCredentials: creds
            );

            var response = new Token { Jwt = new JwtSecurityTokenHandler().WriteToken(token) };
            return Ok(JsonConvert.SerializeObject(response));
        }
    }
}
