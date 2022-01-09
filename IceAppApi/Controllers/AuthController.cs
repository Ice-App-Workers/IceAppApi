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
                .Where(row => row.Password == user.Password)
                .FirstOrDefaultAsync();

            if (dbUser == null)
            {
                return Unauthorized();
            }
            else
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, "Manager")
                };
                var tokeOptions = new JwtSecurityToken(
                    issuer: "https://localhost:44309",
                    audience: "https://localhost:44309",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: signinCredentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                return Ok(new { Token = tokenString, login = user.Email, isProvider = true });
            }
        }
    }
}
