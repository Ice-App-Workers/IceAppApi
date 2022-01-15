using IceAppApi.Models;
using IceAppApi.RequestModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace IceAppApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IceAppDbContext _context;
        public AuthController(IceAppDbContext context)
        {
            _context = context;
        }
        [HttpPost, Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel user)
        {

            if (user == null)
            {
                return BadRequest("Invalid client request");
            }
            var dbUser = await _context.IceShopOwners
                .Where(row => row.Email == user.Email)
                .FirstOrDefaultAsync();
            var verified = Crypto.VerifyHashedPassword(dbUser.Password, user.Password);
            if (verified == false)
            {
                return Unauthorized();
            }
            else
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var claims = new List<Claim>
                {
                    new Claim("Name", user.Email),
                    new Claim("Role", "Manager"),
                    new Claim("providerId", dbUser.Id.ToString())
                };
                var tokeOptions = new JwtSecurityToken(
                    issuer: "https://localhost:44385",
                    audience: "https://localhost:44385",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(60),
                    signingCredentials: signinCredentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                return Ok(new { Token = tokenString, login = user.Email, isProvider = true });
            }
        }
    }
}
