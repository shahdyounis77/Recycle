using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;


namespace Recycle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<Data.Models.User> _userManager;

        private readonly IConfiguration _configuration;

        public AccountController(UserManager<Data.Models.User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost("register")]

        public async Task<IActionResult> Register([FromBody] Data.Dto.RegsiterUser model)

        {
            var user = new Data.Models.User
            {
                UserName = model.UserName,

                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return Ok(new { Message = "User registered successfully" });
            }


            return BadRequest(result.Errors);
        }


        [HttpPost("login")]

        public async Task<IActionResult> Login([FromBody] Data.Dto.LoginUser model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {

               var claims=new List<Claim>
               {
                   new Claim(ClaimTypes.NameIdentifier,user.Id),
                   new Claim(ClaimTypes.Name,user.UserName)
               };

                 var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:Key"]));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
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
            return Unauthorized(new { Message = "Invalid username or password" });
        }
    }
}