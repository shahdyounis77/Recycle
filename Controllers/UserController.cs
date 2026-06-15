using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Recycle.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Recycle.Services;


namespace Recycle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly UserManager<Data.Models.User> _userManager;

        private readonly IConfiguration _configuration;

        private readonly UserServices _userService;

        public UserController(UserManager<Data.Models.User> userManager, IConfiguration configuration, UserServices userService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _userService = userService;
        }

        [HttpPost("Register")]

        public async Task<IActionResult> Register([FromBody] RegsiterUser model)

        {
            var user = new Data.Models.User
            {
                UserName = model.UserName,

                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {


                await _userManager.AddToRoleAsync(user, "User");

                return Ok(new { Message = "User registered successfully" });
            }


            return BadRequest(result.Errors);
        }


        [HttpPost("Login")]

        public async Task<IActionResult> Login([FromBody] LoginUser model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var claims = new List<Claim> {

                  new Claim(ClaimTypes.NameIdentifier, user.Id),
                         new Claim(ClaimTypes.Name, user.UserName)};
    

                var roles = await _userManager.GetRolesAsync(user);

                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration["jwt:Key"]));

                var creds = new SigningCredentials(
                    key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _configuration["jwt:Issuer"],
                    audience: _configuration["jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: creds
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(new { Token = tokenString });
            }


            else
            {
                return Unauthorized(new { Message = "Invalid username or password" });
            }
        }
        [HttpGet("Coins")]
        public async Task<IActionResult> GetCoins()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized(new { Message = "User not authenticated" });
            }
            try
            {
                var coins = await _userService.Getcoins(userId);
                return Ok(new { Coins = coins });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }

        }
    }
}